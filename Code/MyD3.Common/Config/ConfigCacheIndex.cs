using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyD3.Common.Config
{
    /// <summary>
    /// 配置缓存索引
    /// </summary>
    class ConfigCacheIndex
    {
        public ConfigCacheIndex(string adapterName, long configVersion, string key)
        {
            this.AdapterName = adapterName;
            this.ConfigVersion = configVersion;
            this.ConfigKey = key;
        }

        /// <summary>
        /// 适配器名称
        /// </summary>
        public string AdapterName { get; set; }

        /// <summary>
        /// 适配器配置版本
        /// </summary>
        public long ConfigVersion { get; set; }

        /// <summary>
        /// 配置的Key
        /// </summary>
        public string ConfigKey { get; set; }
    }
}
