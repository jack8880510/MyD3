using MyD3.Business.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyD3.Entity;
using MyD3.Entity.DBEntity;
using MyD3.Data.Interface;
using MyD3.Common.Log;
using MyD3.Common.Cache;
using MyD3.Business.Interface.DataModel;
using MyD3.Entity.DBView;
using MyD3.Common;
using Newtonsoft.Json;
using System.Transactions;

namespace MyD3.Business
{
    /// <summary>
    /// 数据字典业务模型
    /// </summary>
    public class DictionaryBusiness : IDictionaryBusiness
    {
        public const string CACHE_DICTIONARY_DATA = "dictionary";

        private readonly IRepository<DictionaryDataDetail> _dictionaryDetailRps;
        private readonly IRepository<DictionaryIndex> _dictionaryIndexRps;
        private readonly IRepository<DictionaryData> _dictionaryDataRps;
        private readonly ICacheManager _cacheManager;
        private readonly ILogger _logger;

        /// <summary>
        /// 初始化分组业务模型
        /// </summary>
        /// <param name="groupRps">分组数据仓库</param>
        /// <param name="logger">日志管理</param>
        public DictionaryBusiness(IRepository<DictionaryDataDetail> dictionaryDetailRps,
            IRepository<DictionaryIndex> dictionaryIndexRps,
            IRepository<DictionaryData> dictionaryDataRps,
            ILogger logger, ICacheManager cacheManager)
        {
            this._dictionaryDetailRps = dictionaryDetailRps;
            this._dictionaryIndexRps = dictionaryIndexRps;
            this._dictionaryDataRps = dictionaryDataRps;
            this._logger = logger;
            this._cacheManager = cacheManager;
        }

        /// <summary>
        /// 加载所有数据字典结构化数据到Cache中
        /// </summary>
        protected ResultValue<bool> LoadDictionaryStructData()
        {
            //获取所有的索引
            var allIndex = this._dictionaryIndexRps.Table.Where(x => x.Status != -1).ToList();

            //获取所有的数据
            var allData = this._dictionaryDataRps.Table.Where(x => x.Status != -1).ToList();

            this._logger.I("原始数据获取成功，开始转换成struct结构");
            var dictionaryStruct = allIndex.Select(x => new DictionaryStruct()
            {
                ID = x.ID,
                Index = x.Index,
                Name = x.Name,
                Status = x.Status,
                Data = allData.Where(y => y.IndexID == x.ID).Select(y => new DictionaryDataStruct()
                {
                    ID = y.ID,
                    Name = y.Name,
                    Value = y.Value,
                    OrderNo = y.OrderNo,
                    Status = y.Status
                }).ToList()
            }).ToList();

            this._logger.I("转换成功，写入缓存");
            this._cacheManager.Set<IList<DictionaryStruct>>(CACHE_DICTIONARY_DATA, dictionaryStruct);
            return new ResultValue<bool>(true);
        }

        /// <summary>
        /// 根据搜索条件，搜索索引信息
        /// </summary>
        /// <param name="args">搜索条件</param>
        /// <returns>搜索结果</returns>
        public ResultValue<SearchResult<DictionaryStruct>> Search(SearchArgs args)
        {
            this._logger.I("开始搜索索引信息");

            if (!_cacheManager.Exists(CACHE_DICTIONARY_DATA))
            {
                this._logger.I("由于缓存中没有字典索引信息，开始从数据库获取原始数据");
                var loadResult = this.LoadDictionaryStructData();
                if (loadResult.RCode != 0 || !loadResult.Data)
                {
                    //如果发生错误则直接返回载入时的错误信息
                    return new ResultValue<SearchResult<DictionaryStruct>>(loadResult.RCode, loadResult.RMsg);
                }
            }

            this._logger.I("从缓存中获取字典原始数据");
            var dictionaryInCache = this._cacheManager.Get<IList<DictionaryStruct>>(CACHE_DICTIONARY_DATA);

            this._logger.I("对数据进行过滤");
            var data = dictionaryInCache.Where(x =>
                x.Name.Contains(args.ConditionString) ||
                x.Index.Contains(args.ConditionString)
            ).OrderBy(args.SortName, args.SortDESC)
            .Skip(args.PageIndex * args.PageSize).Take(args.PageSize)
            .ToArray();

            this._logger.I("数据内存过滤成功");
            var result = new ResultValue<SearchResult<DictionaryStruct>>();
            result.Data = new SearchResult<DictionaryStruct>(args, data, dictionaryInCache.Count);

            return result;
        }

        /// <summary>
        /// 修改索引状态
        /// </summary>
        /// <param name="id">需要修改的索引编号</param>
        /// <param name="status">目标状态</param>
        /// <param name="modify_user">修改用户</param>
        /// <returns>是否修改成功</returns>
        protected ResultValue<bool> ChangeIndexStatus(string id, int status, User modify_user)
        {
            this._logger.I("开始修改字典索引状态");
            if (string.IsNullOrEmpty(id))
            {
                this._logger.W("参数ID不合法，状态修改失败：" + id);
                return new ResultValue<bool>(-1, "参数ID不合法，状态修改失败");
            }

            this._logger.I("开始获取索引原始数据");
            var dictionaryIndex = this._dictionaryIndexRps.Table.Where(x => x.ID == id && x.Status != -1).FirstOrDefault();
            if (dictionaryIndex == null)
            {
                this._logger.W("获取索引原始数据失败，无法完成状态修改操作");
                return new ResultValue<bool>(-1, "获取索引原始数据失败，无法完成状态修改操作");
            }

            //修改原始数据
            dictionaryIndex.ModifyDate = DateTime.Now;
            dictionaryIndex.ModifyUser = modify_user.ID;
            dictionaryIndex.Status = status; //表示禁用

            //修改到数据库
            this._dictionaryIndexRps.Update(dictionaryIndex);

            this._logger.I("数据层更新成功，开始缓存更新");
            var cacheList = this._cacheManager.Get<IList<DictionaryStruct>>(CACHE_DICTIONARY_DATA);
            var cacheData = cacheList.Where(x => x.ID == dictionaryIndex.ID).FirstOrDefault();
            if (cacheData != null)
            {
                cacheData.Status = status;
                this._cacheManager.Set(CACHE_DICTIONARY_DATA, cacheList);
            }
            this._logger.I("状态更新成功");

            return new ResultValue<bool>();
        }

        /// <summary>
        /// 禁用指定的索引
        /// </summary>
        /// <param name="id">需要禁用的索引ID</param>
        /// <param name="modify_user">修改人</param>
        /// <returns>是否禁用成功</returns>
        public ResultValue<bool> Disable(string id, User modify_user)
        {
            this._logger.I("开始索引禁用");
            return this.ChangeIndexStatus(id, 1, modify_user);
        }

        /// <summary>
        /// 启用指定的索引
        /// </summary>
        /// <param name="id">需要启用的索引ID</param>
        /// <param name="modify_user">修改人</param>
        /// <returns>是否启用成功</returns>
        public ResultValue<bool> Enable(string id, User modify_user)
        {
            this._logger.I("开始索引启用");
            return this.ChangeIndexStatus(id, 0, modify_user);
        }

        /// <summary>
        /// 根据索引编号获取数据字典结构化数据对象
        /// </summary>
        /// <param name="indexID">索引编号</param>
        /// <returns>结构化数据对象</returns>
        public ResultValue<DictionaryStruct> GetStruct(string indexID)
        {
            this._logger.I("开始根据ID获取结构化索引信息：" + indexID);

            if (!_cacheManager.Exists(CACHE_DICTIONARY_DATA))
            {
                this._logger.I("由于缓存中没有字典索引信息，开始从数据库获取原始数据");
                var loadResult = this.LoadDictionaryStructData();
                if (loadResult.RCode != 0 || !loadResult.Data)
                {
                    //如果发生错误则直接返回载入时的错误信息
                    return new ResultValue<DictionaryStruct>(loadResult.RCode, loadResult.RMsg);
                }
            }

            this._logger.I("从缓存中获取字典原始数据");
            var dictionaryInCache = this._cacheManager.Get<IList<DictionaryStruct>>(CACHE_DICTIONARY_DATA);

            var result = dictionaryInCache.Where(x => x.ID == indexID).FirstOrDefault();

            return new ResultValue<DictionaryStruct>(result);
        }

        /// <summary>
        /// 新增或修改字典数据
        /// </summary>
        /// <param name="data">需要处理的数据</param>
        /// <param name="creator">修改人</param>
        /// <returns>编辑后的数据</returns>
        public ResultValue<DictionaryStruct> Save(DictionaryStruct data, User creator)
        {
            this._logger.I("开始保存数据字典信息");

            if (data == null)
            {
                this._logger.W("由于数据词典不存在，保存失败了");
                return new ResultValue<DictionaryStruct>(-1, "由于数据词典不存在，保存失败");
            }
            this._logger.D(JsonConvert.SerializeObject(data));

            if (string.IsNullOrEmpty(data.Index) || data.Name.Length > 50)
            {
                this._logger.W("数据字典索引不合法，保存失败");
                return new ResultValue<DictionaryStruct>(-1, "数据字典索引不合法,长度必须在1-50位之间，保存失败");
            }
            else if (string.IsNullOrEmpty(data.Name) || data.Name.Length > 25)
            {
                this._logger.W("数据字典名称不合法，保存失败");
                return new ResultValue<DictionaryStruct>(-1, "数据字典名称不合法,长度必须在1-25位之间，保存失败");
            }
            else if (data.Status != 0 && data.Status != 1)
            {
                this._logger.W("数据字典状态不合法，保存失败");
                return new ResultValue<DictionaryStruct>(-1, "数据字典状态不合法,只能是0或1，保存失败");
            }

            this._logger.I("开始获取结构化索引原始数据");
            var getStructByIndexResult = this.GetStruct(data.Index);
            if (getStructByIndexResult.RCode != 0)
            {
                this._logger.W("根据索引获取结构化原始数据失败，保存失败：" + getStructByIndexResult);
                return new ResultValue<DictionaryStruct>(getStructByIndexResult.RCode, getStructByIndexResult.RMsg);
            }
            else if (getStructByIndexResult.Data != null && getStructByIndexResult.Data.ID != data.ID)
            {
                this._logger.W("指定索引已被使用，无法完成保存");
                return new ResultValue<DictionaryStruct>(-1, "指定索引已被使用，请更换索引后重试，冲突索引名称为：" + getStructByIndexResult.Data.Name);
            }

            //开始验证数据字典数据
            foreach (var temp in data.Data)
            {
                if (string.IsNullOrEmpty(temp.Name) || temp.Name.Length > 25)
                {
                    this._logger.W("数据字典数据名称不合法，保存失败");
                    return new ResultValue<DictionaryStruct>(-1, "数据字典数据（" + temp.OrderNo + "）名称不合法,长度必须在1-25位之间，保存失败");
                }
                else if (string.IsNullOrEmpty(temp.Value) || temp.Value.Length > 100)
                {
                    this._logger.W("数据字典数据值不合法，保存失败");
                    return new ResultValue<DictionaryStruct>(-1, "数据字典数据（" + temp.OrderNo + "）值不合法,长度必须在1-100位之间，保存失败");
                }
            }

            //验证完毕开始封装数据
            DictionaryIndex dIndex = null;
            List<DictionaryData> dData = new List<DictionaryData>();

            this._logger.I("开始封装Index");
            if (!string.IsNullOrEmpty(data.ID))
            {
                //修改模式处理逻辑
                this._logger.I("开始获取索引原始数据");
                var dictionaryIndex = this._dictionaryIndexRps.Table.Where(x => x.ID == data.ID && x.Status != -1).FirstOrDefault();
                if (dictionaryIndex == null)
                {
                    this._logger.W("根据索引获取原始数据失败，保存失败");
                    return new ResultValue<DictionaryStruct>(-1, "根据索引获取原始数据失败，保存失败");
                }

                this._logger.I("开始获取字典数据原始数据");
                var dictionaryDataList = this._dictionaryDataRps.Table.Where(x => x.IndexID == data.ID && x.Status != -1).ToList();

                //修改数据
                dIndex = dictionaryIndex;
                dIndex.ModifyDate = DateTime.Now;
                dIndex.ModifyUser = creator.ID;
                dIndex.Name = data.Name;

                //初始化原始数据
                dData.AddRange(dictionaryDataList);
            }
            else
            {
                //新增模式处理逻辑
                dIndex = new DictionaryIndex()
                {
                    CreateDate = DateTime.Now,
                    CreatorID = creator.ID,
                    Index = data.Index,
                    Name = data.Name
                };
            }
            this._logger.I("开始封装Data");
            data.Data.ToList().ForEach(new Action<DictionaryDataStruct>(
                delegate (DictionaryDataStruct args)
                {
                    if (string.IsNullOrEmpty(args.ID))
                    {
                        //如果该数据是新增的
                        DictionaryData temp = new DictionaryData()
                        {
                            CreateDate = DateTime.Now,
                            CreatorID = creator.ID,
                            Name = args.Name,
                            Value = args.Value,
                            Status = args.Status,
                            OrderNo = args.OrderNo
                        };
                        dData.Add(temp);
                    }
                    else
                    {
                        //如果是修改，则获取原始数据，并修改数据
                        DictionaryData temp = dData.Where(x => x.ID == args.ID).First();
                        temp.ModifyDate = DateTime.Now;
                        temp.ModifyUser = creator.ID;
                        temp.Name = args.Name;
                        temp.Status = args.Status;
                        temp.OrderNo = args.OrderNo;
                    }
                }));

            this._logger.I("开始保存数据");
            using (TransactionScope tran = new TransactionScope())
            {
                if (string.IsNullOrEmpty(data.ID))
                {
                    //新增
                    dIndex.ID = Guid.NewGuid().ToString();
                    this._dictionaryIndexRps.Insert(dIndex);
                }
                else
                {
                    this._dictionaryIndexRps.Update(dIndex);
                }

                foreach (var d in dData)
                {
                    if (string.IsNullOrEmpty(d.ID))
                    {
                        //新增
                        d.ID = Guid.NewGuid().ToString();
                        d.IndexID = dIndex.ID;
                        this._dictionaryDataRps.Insert(d);
                    }
                    else
                    {
                        this._dictionaryDataRps.Update(d);
                    }
                }
                tran.Complete();
            }

            this._logger.I("数据保存成功开始刷新缓存");
            //生成新的结构化对象
            DictionaryStruct dStruct = new DictionaryStruct()
            {
                ID = dIndex.ID,
                Index = dIndex.Index,
                Name = dIndex.Name,
                Status = dIndex.Status,
                Data = dData.Select(x => new DictionaryDataStruct()
                {
                    ID = x.ID,
                    Name = x.Name,
                    OrderNo = x.OrderNo,
                    Status = x.Status,
                    Value = x.Value
                }).ToList()
            };

            //获取缓存列表
            if (!this._cacheManager.Exists(CACHE_DICTIONARY_DATA))
            {
                this._logger.I("由于缺少缓存数据，开始尝试载入");
                this.LoadDictionaryStructData();
            }
            IList<DictionaryStruct> structList = this._cacheManager.Get<IList<DictionaryStruct>>(CACHE_DICTIONARY_DATA);

            if (string.IsNullOrEmpty(data.ID))
            {
                //如果本次存储的数据是一个新的数据字典则加入到缓存中
                structList.Add(dStruct);
            }
            else
            {
                //如果本次存储的数据是一个被更新的数据字典项，则尝试修改
                var existsCache = structList.Where(x => x.ID == dIndex.ID).First();
                structList.Remove(existsCache);
                structList.Add(dStruct);
            }

            //保存到缓存中
            this._cacheManager.Set(CACHE_DICTIONARY_DATA, structList);

            return new ResultValue<DictionaryStruct>();
        }
    }
}
