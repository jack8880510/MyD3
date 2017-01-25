using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyD3.Common.Config
{
    /// <summary>
    /// 配置管理接口
    /// </summary>
    public interface IConfigManager
    {
        /// <summary>
        /// 获取配置信息
        /// </summary>
        /// <param name="key">配置的Key</param>
        /// <returns>配置的值</returns>
        string Get(string key);

        /// <summary>
        /// 设置配置信息
        /// </summary>
        /// <param name="key">配置的Key</param>
        /// <param name="value">需要设置的值</param>
        /// <returns>是否设置成功</returns>
        bool Set(string key, string value);
    }
}
