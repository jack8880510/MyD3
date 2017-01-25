using MyD3.Business.Interface;
using MyD3.Common.Cache;
using MyD3.Common.Config;
using MyD3.Common.Log;
using MyD3.Data.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyD3.Entity;
using MyD3.Entity.DBEntity;
using MyD3.Entity.DBView;
using System.Text.RegularExpressions;
using System.Transactions;

namespace MyD3.Business
{
    /// <summary>
    /// 系统设置业务模型
    /// </summary>
    public class SystemSettingBusiness : ISystemSettingBusiness, IMultipleConfigAdapter
    {
        public const string CACHE_CONFIG_VERSION = "db_config_version";

        private readonly ILogger _logger;
        private readonly IRepository<SystemSettingDetail> _sysSettingDetailRps;
        private readonly IRepository<SystemSetting> _sysSettingRps;
        private readonly ICacheManager _cacheManager;

        public SystemSettingBusiness(IRepository<SystemSettingDetail> sysSettingDetailRps,
            IRepository<SystemSetting> sysSettingRps,
            ILogger logger, ICacheManager cacheManager)
        {
            this._logger = logger;
            this._sysSettingDetailRps = sysSettingDetailRps;
            this._sysSettingRps = sysSettingRps;
            this._cacheManager = cacheManager;
        }

        #region ISystemSettingBusiness的成员
        /// <summary>
        /// 获取全部的系统设置
        /// </summary>
        /// <returns>系统设置信息</returns>
        public ResultValue<SystemSettingDetail[]> AllSystemSettingDetail()
        {
            //直接从数据层返回
            var allSetting = this._sysSettingDetailRps.Table.ToArray();
            return new ResultValue<SystemSettingDetail[]>(allSetting);
        }

        /// <summary>
        /// 保存系统设置
        /// </summary>
        /// <param name="args">需要保存的系统设置列表</param>
        /// <param name="creator">创建人</param>
        /// <returns>是否保存成功</returns>
        public ResultValue<bool> Save(IDictionary<string, string> args, User creator)
        {
            this._logger.I("开始保存系统配置数据");

            this._logger.I("开始从数据库获取系统配置原始数据");
            var allSysSetting = this._sysSettingRps.Table.ToList();

            //储存变更的配置集合
            var changedSetting = new List<SystemSetting>();

            //开始逐一进行验证并赋值
            foreach (var config in allSysSetting)
            {
                this._logger.D("开始处理配置：" + config.Name);

                if (!args.ContainsKey(config.Name))
                {
                    this._logger.D("新配置中不包含该就配置的修改信息，跳过处理");
                    continue;
                }
                else if (args[config.Name] == config.Value)
                {
                    this._logger.D("如果新旧配置一致，跳过处理");
                    continue;
                }

                var newValue = args[config.Name];

                if (!config.Required && string.IsNullOrEmpty(newValue))
                {
                    this._logger.D("该配置值允许为NULL且为NULL，则跳过其他检查");
                }
                else
                {
                    //开始验证
                    if (!RequiredCheck(config, newValue))
                    {
                        return new ResultValue<bool>(-1, config.DisplayName + "必须填写");
                    }
                    else if (!MinLengthCheck(config, newValue))
                    {
                        return new ResultValue<bool>(-1, config.DisplayName + "不符合最小值要求");
                    }
                    else if (!MaxLengthCheck(config, newValue))
                    {
                        return new ResultValue<bool>(-1, config.DisplayName + "不符合最大值要求");
                    }
                    else if (!RegexCheck(config, newValue))
                    {
                        return new ResultValue<bool>(-1, config.DisplayName + "不符合规则");
                    }
                }

                //如果验证通过则写入数据到对象
                config.Value = newValue;
                config.ModifyDate = DateTime.Now;
                config.ModifyUser = creator.ID;

                //加入到变更集合中
                changedSetting.Add(config);

                this._logger.D("配置处理成功");
            }

            if (changedSetting.Count == 0)
            {
                this._logger.I("配置信息与原始数据一致，没有改变，跳过更新");
                return new ResultValue<bool>(true);
            }

            using (TransactionScope tran = new TransactionScope())
            {
                this._sysSettingRps.UpdateRanges(changedSetting);

                tran.Complete();
            }

            this._logger.D("开始更新配置版本（缓存适配器使用）");
            this.ConfigVersion++;

            //返回更新成功
            return new ResultValue<bool>(true);
        }
        #endregion

        /// <summary>
        /// 验证是否必填
        /// </summary>
        /// <param name="setting">配置选项</param>
        /// <param name="value">需要设置的值</param>
        /// <returns>验证是否通过</returns>
        public bool RequiredCheck(SystemSetting setting, string value)
        {
            this._logger.I("开始验证是否必填");
            return !setting.Required || setting.Required && !string.IsNullOrEmpty(value);
        }

        /// <summary>
        /// 验证最小长度是否符合要求
        /// </summary>
        /// <param name="setting">配置选项</param>
        /// <param name="value">需要设置的值</param>
        /// <returns>验证是否通过</returns>
        public bool MinLengthCheck(SystemSetting setting, string value)
        {
            if (string.IsNullOrEmpty(setting.MinLength))
            {
                //如果不需要验证最小数量则返回成功
                return true;
            }

            this._logger.I("开始验证最小值");
            if (setting.ValueType == "TEXT" || setting.ValueType == "OPTION")
            {
                int minLength = 0;
                if (!int.TryParse(setting.MinLength, out minLength))
                {
                    throw new ArgumentException("配置" + setting.Name + "的最小长度设置不是一个有效的数字");
                }
                else if (minLength <= 0)
                {
                    //如果最小长度不是一个正整数则跳过处理
                    return true;
                }
                return setting.ValueType == "TEXT" && value.Length >= minLength
                    || setting.ValueType == "OPTION" && value.Split(',').Length >= minLength;
            }
            else if (setting.ValueType == "DATE" || setting.ValueType == "TIME" || setting.ValueType == "DATE-TIME")
            {
                var minDate = DateTime.Now;
                var valueDate = DateTime.Now;
                if (!DateTime.TryParse(setting.MinLength, out minDate))
                {
                    throw new ArgumentException("配置" + setting.Name + "的最小值设置不是一个有效的时间或日期");
                }
                else if (!DateTime.TryParse(value, out valueDate))
                {
                    throw new ArgumentException("配置" + setting.Name + "的值不是一个有效的时间或日期");
                }
                return valueDate >= minDate;
            }

            return true;
        }

        /// <summary>
        /// 验证最大长度是否符合要求
        /// </summary>
        /// <param name="setting">配置选项</param>
        /// <param name="value">需要设置的值</param>
        /// <returns>验证是否通过</returns>
        public bool MaxLengthCheck(SystemSetting setting, string value)
        {
            if (string.IsNullOrEmpty(setting.MaxLength))
            {
                //如果不需要验证最大数量则返回成功
                return true;
            }

            this._logger.I("开始验证最大值");
            if (setting.ValueType == "TEXT" || setting.ValueType == "OPTION")
            {
                int maxLength = 0;
                if (!int.TryParse(setting.MaxLength, out maxLength))
                {
                    throw new ArgumentException("配置" + setting.Name + "的最大长度设置不是一个有效的数字");
                }
                else if (maxLength <= 0)
                {
                    //如果最大长度不是一个正整数则跳过处理
                    return true;
                }
                return setting.ValueType == "TEXT" && value.Length <= maxLength
                    || setting.ValueType == "OPTION" && value.Split(',').Length <= maxLength;
            }
            else if (setting.ValueType == "DATE" || setting.ValueType == "TIME" || setting.ValueType == "DATE-TIME")
            {
                var maxDate = DateTime.Now;
                var valueDate = DateTime.Now;
                if (!DateTime.TryParse(setting.MaxLength, out maxDate))
                {
                    throw new ArgumentException("配置" + setting.Name + "的最大值设置不是一个有效的时间或日期");
                }
                else if (!DateTime.TryParse(value, out valueDate))
                {
                    throw new ArgumentException("配置" + setting.Name + "的值大是一个有效的时间或日期");
                }
                return valueDate <= maxDate;
            }

            return true;
        }

        /// <summary>
        /// 正则表达式验证
        /// </summary>
        /// <param name="setting">配置选项</param>
        /// <param name="value">需要设置的值</param>
        /// <returns>验证是否通过</returns>
        public bool RegexCheck(SystemSetting setting, string value)
        {
            if (string.IsNullOrEmpty(setting.TextReg))
            {
                //如果不需要验证正则表达式则返回成功
                return true;
            }

            this._logger.I("开始验证正则表达式");
            return Regex.IsMatch(value, setting.TextReg);
        }

        #region IMultipleConfigAdapter的成员
        /// <summary>
        /// 获取所有配置的索引
        /// </summary>
        public string[] AllKeys
        {
            get
            {
                this._logger.I("开始获取所有的设置key");

                //数据层获取全部数据
                var allResult = this.AllSystemSettingDetail();

                if (allResult.RCode != 0)
                {
                    throw new Exception("从数据库获取全部配置信息时失败：" + allResult);
                }

                //否则返回全部的配置Name作为索引
                return allResult.Data.Select(x => x.Name).ToArray();
            }
        }

        /// <summary>
        /// 获取一个值，表示配置的版本
        /// </summary>
        public long ConfigVersion
        {
            get
            {
                if (!this._cacheManager.Exists(CACHE_CONFIG_VERSION))
                {
                    this._cacheManager.Set<long>(CACHE_CONFIG_VERSION, 1);
                }
                return this._cacheManager.Get<long>(CACHE_CONFIG_VERSION);
            }
            private set
            {
                this._cacheManager.Set<long>(CACHE_CONFIG_VERSION, value);
            }
        }

        /// <summary>
        /// 表示该适配器的名称
        /// </summary>
        public string Name
        {
            get
            {
                return "dbSystemSetting";
            }
        }

        /// <summary>
        /// 是否使用缓存加速
        /// </summary>
        public bool UseCache
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// 获取所有的配置信息
        /// </summary>
        /// <returns>配置信息键值对</returns>
        public IDictionary<string, string> All()
        {
            this._logger.I("开始获取所有的设置");

            //数据层获取全部数据
            var allResult = this.AllSystemSettingDetail();

            if (allResult.RCode != 0)
            {
                throw new Exception("从数据库获取全部配置信息时失败：" + allResult);
            }

            //返回所有的数据
            return allResult.Data.ToDictionary(x => x.Name, y => y.Value);
        }

        /// <summary>
        /// 获取指定的配置信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string Get(string key)
        {
            this._logger.I("开始根据索引获取配置信息");

            //数据层获取全部数据
            var setting = this._sysSettingRps.Table.Where(x => x.Name == key && x.Status != -1).FirstOrDefault();

            return setting == null ? null : setting.Value;
        }

        public bool Set(string key, string value)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
