using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DemoSite.Models
{
    /// <summary>
    /// 保存系统配置页面参数类
    /// </summary>
    public class SaveSystemSettingArgs
    {
        /// <summary>
        /// 系统配置的名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 系统配置的值
        /// </summary>
        public string Value { get; set; }
    }
}