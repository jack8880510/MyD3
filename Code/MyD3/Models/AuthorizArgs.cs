using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DemoSite.Models
{
    /// <summary>
    /// 分组授权参数
    /// </summary>
    public class AuthorizArgs
    {
        /// <summary>
        /// 编号
        /// </summary>
        public string TargetID { get; set; }

        /// <summary>
        /// 需要进行授权的权限编号列表
        /// </summary>
        public string[] Permissions { get; set; }
    }
}