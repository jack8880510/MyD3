using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyD3.Common.Config
{
    /// <summary>
    /// 混合配置适配器
    /// </summary>
    public interface IMultipleConfigAdapter
    {
        /// <summary>
        /// 获取一个值，表示是否使用缓存进行加速，
        /// 如果使用缓存进行加速，则必须为Adapter维护ConfigVersion,已确保可以正常跟新配置信息
        /// </summary>
        bool UseCache { get; }

        /// <summary>
        /// 获取一个值，表示配置的版本号
        /// 如果使用了缓存加速，则会在获取前匹配配置版本，以保证获取的数据始终为最新版本
        /// </summary>
        long ConfigVersion { get; }

        /// <summary>
        /// 获取一个值，表示该适配器的名称
        /// 在设置时，需要通过该名称路由到指定的适配器中进行Set
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 获取一个值，表示Key的集合
        /// </summary>
        string[] AllKeys { get; }

        /// <summary>
        /// 获取一个值，表示所有配置项的集合
        /// </summary>
        /// <returns></returns>
        IDictionary<string, string> All();

        /// <summary>
        /// 设置指定的配置信息
        /// </summary>
        /// <param name="key">配置信息的Key</param>
        /// <param name="value">配置信息的值</param>
        /// <returns>是否设置成功</returns>
        bool Set(string key, string value);

        /// <summary>
        /// 获取指定的配置
        /// </summary>
        /// <param name="key">需要获取的配置key</param>
        /// <returns>配置的值</returns>
        string Get(string key);
    }
}
