using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DemoSite.Models
{
    /// <summary>
    /// 设置分组成员参数
    /// </summary>
    public class SetGroupMembersArgs
    {
        /// <summary>
        /// 分组编号
        /// </summary>
        public string GroupID { get; set; }

        /// <summary>
        /// 成员编号列表
        /// </summary>
        public string [] UserIDList { get; set; }
    }
}