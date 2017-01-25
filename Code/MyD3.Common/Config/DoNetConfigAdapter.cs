using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyD3.Common.Config
{
    /// <summary>
    /// DoNet配置文件适配器
    /// </summary>
    public class DoNetConfigAdapter : IMultipleConfigAdapter
    {
        /// <summary>
        /// 获取一个值，表示配置的版本号
        /// 如果使用了缓存加速，则会在获取前匹配配置版本，以保证获取的数据始终为最新版本
        /// </summary>
        public long ConfigVersion
        {
            get
            {
                //由于该适配器不适用缓存，所以无需维护配置版本
                return -1;
            }
        }

        /// <summary>
        /// 获取一个值，表示Key的集合
        /// </summary>
        public string[] AllKeys
        {
            get
            {
                return ConfigurationManager.AppSettings.AllKeys;
            }
        }

        /// <summary>
        /// 获取一个值，表示该适配器的名称
        /// </summary>
        public string Name
        {
            get
            {
                return "doNetConfig";
            }
        }

        /// <summary>
        /// 获取一个值，表示是否使用缓存进行加速，
        /// 如果使用缓存进行加速，则必须为Adapter维护ConfigVersion,已确保可以正常跟新配置信息
        /// </summary>
        public bool UseCache
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// 获取配置信息
        /// </summary>
        /// <param name="name">配置的名称</param>
        /// <returns>配置的值</returns>
        public string Get(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                //如果name不存在则返回null
                return null;
            }
            return ConfigurationManager.AppSettings[name];
        }

        /// <summary>
        /// 设置一个配置项，如果存在则覆盖，不存在则创建
        /// </summary>
        /// <param name="key">配置的key</param>
        /// <param name="value">配置的值</param>
        /// <returns>是否修改成功</returns>
        public bool Set(string key, string value)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取所有的配置信息
        /// </summary>
        /// <returns></returns>
        public IDictionary<string, string> All()
        {
            return ConfigurationManager.AppSettings.AllKeys.ToDictionary
                (
                    x => x,
                    y => ConfigurationManager.AppSettings[y]
                );
        }
    }
}
