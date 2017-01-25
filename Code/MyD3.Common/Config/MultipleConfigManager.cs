using MyD3.Common.Cache;
using MyD3.Common.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyD3.Common.Config
{
    /// <summary>
    /// 混合配置管理器
    /// 用于整合不同的配置资源，并以统一的方式提供给外部程序使用
    /// 提供缓存加速
    /// </summary>
    public class MultipleConfigManager : IConfigManager
    {
        public const string CACHE_CONFIG_VALUE_LIST = "config_value";
        public const string CACHE_CONFIG_INDEX_LIST = "config_index";

        private readonly ILogger _logger;
        private readonly ICacheManager _cacheManager;
        private readonly IDictionary<string, IMultipleConfigAdapter> _adapters;

        /// <summary>
        /// 初始化混合配置管理器
        /// </summary>
        /// <param name="logger">日志管理器</param>
        /// <param name="cacheManager">缓存管理器</param>
        /// <param name="adapters">适配器集合，用于获取配置的数据源</param>
        public MultipleConfigManager(ILogger logger, ICacheManager cacheManager, params IMultipleConfigAdapter[] adapters)
        {
            this._logger = logger;
            this._cacheManager = cacheManager;

            if (adapters.GroupBy(x => x.Name, y => y).Count() < adapters.Length)
            {
                this._logger.W("适配器列表中包含重名的适配器");
                throw new ArgumentException("适配器列表中包含重名的适配器");
            }

            //转换为key value固定适配器名称
            this._adapters = adapters.ToDictionary(x => x.Name, y => y);

            //将所有需要进行缓存的适配器进行数据缓存
            RefreshCache();
        }

        /// <summary>
        /// 更新缓存信息
        /// </summary>
        public void RefreshCache()
        {
            this._logger.I("开始更新所有适配器缓存");

            //验证需要缓存的配置中是否包含重复的key
            List<string> tempList = new List<string>();
            this._adapters.Where(x => x.Value.UseCache).Select(x => x.Value.AllKeys).ToList().ForEach(x => tempList.AddRange(x));
            if (tempList.GroupBy(x => x).Any(x => x.Count() > 1))
            {
                this._logger.W("包含重复名称的配置项");
                throw new ArgumentException("包含重复名称的配置项");
            }

            foreach (var adapter in this._adapters)
            {
                //循环处理所有的Adapter
                if (!adapter.Value.UseCache)
                {
                    this._logger.I("适配器：" + adapter.Key + " 没有启用缓存，开始清理");
                    //如果Adapter设置为不使用Cache则清楚缓存
                    this.ClearCache(adapter.Key);
                }
                else
                {
                    this._logger.I("适配器：" + adapter.Key + " 启用了缓存，开始刷新");
                    //如果使用Cache则开始更新逻辑
                    this.RefreshCache(adapter.Value, adapter.Key);
                }
            }
        }

        /// <summary>
        /// 更新缓存信息
        /// </summary>
        /// <param name="adapter">配置适配器</param>
        /// <param name="adapterName">适配器名称</param>
        protected void RefreshCache(IMultipleConfigAdapter adapter, string adapterName)
        {
            this._logger.I("开始刷新缓存");

            if (adapter == null)
            {
                this._logger.W("配置适配器为空，无法完成刷新");
                throw new ArgumentNullException("无法从空适配器中读取配置信息");
            }
            else if (string.IsNullOrEmpty(adapterName))
            {
                this._logger.W("配置适配器的名称不合法，不能使用空的适配器名称");
                throw new ArgumentNullException("配置适配器的名称不合法，不能使用空的适配器名称");
            }

            //判断版本是否一致，如果一致则不进行更新
            if (this._cacheManager.GetList<ConfigCacheIndex>(CACHE_CONFIG_INDEX_LIST).Any(x => x.AdapterName == adapter.Name && x.ConfigVersion == adapter.ConfigVersion))
            {
                this._logger.I("本地缓存数据版本与适配器版本一直，跳过更新过程");
                return;
            }

            //获取所有的配置信息
            var allData = adapter.All();

            if (allData.GroupBy(x => x.Key).Any(x => x.Count() > 1))
            {
                this._logger.W("适配器中的配置项包含重复的key");
                throw new ArgumentException("适配器中的配置项包含重复的key");
            }
            else
            {
                foreach (var data in allData)
                {
                    var cacheData = this._cacheManager.GetFromList<ConfigCacheIndex>(CACHE_CONFIG_INDEX_LIST, data.Key);
                    if (cacheData != null && cacheData.AdapterName != adapterName)
                    {
                        this._logger.W("适配器中的部分配置Key已经在其他适配器中出现了");
                        throw new ArgumentException("适配器中的部分配置Key已经在其他适配器中出现了");
                    }
                }
            }

            this._logger.I("清除缓存以准备更新");
            this.ClearCache(adapterName);

            this._logger.I("开始写入缓存索引");
            allData.ToList().ForEach(x => this._cacheManager.AddToList(CACHE_CONFIG_INDEX_LIST, x.Key, new ConfigCacheIndex(adapterName, adapter.ConfigVersion, x.Key)));
            this._logger.I("开始写入缓存数据");
            allData.ToList().ForEach(x => this._cacheManager.AddToList(CACHE_CONFIG_VALUE_LIST, adapterName + "/" + adapter.ConfigVersion + "/" + x.Key, x.Value));
            this._logger.I("缓存更新完毕");
        }

        /// <summary>
        /// 清除缓存
        /// </summary>
        /// <param name="adapterName">需要清除缓存的适配器名称</param>
        protected void ClearCache(string adapterName)
        {
            this._logger.I("开始移除配置缓存");

            if (string.IsNullOrEmpty(adapterName))
            {
                this._logger.W("适配器名称为空");
                throw new ArgumentNullException("适配器名称为空");
            }
            else if (!this._adapters.Any(x => x.Key == adapterName))
            {
                this._logger.W("指定适配器不在已知的列表内");
                throw new ArgumentException("指定适配器不在已知的列表内");
            }

            this._logger.I("开始获取所有相关索引");
            var indexList = this._cacheManager.GetList<ConfigCacheIndex>(CACHE_CONFIG_INDEX_LIST).Where(x => x.AdapterName == adapterName).ToList();

            this._logger.I("开始清理索引");
            indexList.ForEach(x => this._cacheManager.RemoveFromList<ConfigCacheIndex>(CACHE_CONFIG_INDEX_LIST, x.ConfigKey));
            this._logger.I("开始清理配置数据");
            indexList.ForEach(x => this._cacheManager.RemoveFromList<string>(CACHE_CONFIG_VALUE_LIST, x.AdapterName + "/" + x.ConfigVersion + "/" + x.ConfigKey));
            this._logger.I("清理完毕");
        }

        /// <summary>
        /// 获取配置信息
        /// </summary>
        /// <param name="key">配置的Key</param>
        /// <returns>配置的值</returns>
        public string Get(string key)
        {
            this._logger.I("开始获取配置,尝试搜索索引缓存");

            //尝试在索引缓存中查找
            if (!this._cacheManager.ExistsFromList<ConfigCacheIndex>(CACHE_CONFIG_INDEX_LIST, key))
            {
                this._logger.I("未在索引缓存中找到指定的key，开始搜索未缓存适配器");
                var existsAdapter = this._adapters.Where(x => !x.Value.UseCache && x.Value.AllKeys.Contains(key)).Select(x => x.Value).FirstOrDefault();

                if (existsAdapter != null)
                {
                    this._logger.I("找到指定的非缓存适配器，开始返回配置");
                    return existsAdapter.Get(key);
                }

                this._logger.I("没有找到包含Key的非缓存适配器，开始逐一刷新缓存适配器的缓存(仅版本不匹配的)，重新查找");
                var adapterFirstItem = this._cacheManager.GetList<ConfigCacheIndex>(CACHE_CONFIG_INDEX_LIST).GroupBy(x => x.AdapterName).Select(x => x.First()).ToList();
                foreach (var adapter in this._adapters)
                {
                    if (!adapter.Value.UseCache)
                    {
                        //如果该适配器不适用缓存则跳过
                        continue;
                    }

                    //获取当前缓存中的第一个匹配Key
                    var tempItem = adapterFirstItem.Where(x => x.AdapterName == adapter.Key).FirstOrDefault();

                    //匹配版本号是否一致
                    if (tempItem != null && tempItem.ConfigVersion == adapter.Value.ConfigVersion)
                    {
                        //如果版本号一致则不刷新，跳过该过程
                        continue;
                    }

                    //如果版本号不一致则开始刷新过程
                    this.RefreshCache(adapter.Value, adapter.Key);

                    //刷新后再次尝试索引中是否可以找到
                    if (this._cacheManager.ExistsFromList<ConfigCacheIndex>(CACHE_CONFIG_INDEX_LIST, key))
                    {
                        var newVersionIndex = this._cacheManager.Get<ConfigCacheIndex>(key);
                        //如果找到则返回数据
                        return this._cacheManager.GetFromList<string>(CACHE_CONFIG_VALUE_LIST, newVersionIndex.AdapterName + "/" + newVersionIndex.ConfigVersion + "/" + key);
                    }
                }

                return null;
            }
            else
            {
                this._logger.I("在索引中找到，开始匹配数据版本");
                var configIndex = this._cacheManager.GetFromList<ConfigCacheIndex>(CACHE_CONFIG_INDEX_LIST, key);

                if (configIndex.ConfigVersion != this._adapters[configIndex.AdapterName].ConfigVersion)
                {
                    this._logger.I("与适配器当前版本不匹配，开始刷新适配器当前缓存数据");
                    this.RefreshCache(this._adapters[configIndex.AdapterName], configIndex.AdapterName);
                    this._logger.W("递归调用一次，获取刷新后的结果");
                    return this.Get(key);
                }
                else
                {
                    this._logger.I("与适配器当前版本匹配，开始返回数据");
                    return this._cacheManager.GetFromList<string>(CACHE_CONFIG_VALUE_LIST, configIndex.AdapterName + "/" + configIndex.ConfigVersion + "/" + key);
                }
            }
        }

        /// <summary>
        /// 设置配置信息
        /// </summary>
        /// <param name="key">配置的Key</param>
        /// <param name="value">需要设置的值</param>
        /// <returns>是否设置成功</returns>
        public bool Set(string key, string value)
        {
            throw new NotImplementedException();
        }
    }
}
