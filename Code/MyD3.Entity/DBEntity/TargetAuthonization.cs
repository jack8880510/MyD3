using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyD3.Entity.DBEntity
{
    /// <summary>
    /// 对象授权类
    /// </summary>
    public class TargetAuthonization : MyD3Entity
    {
        /// <summary>
        /// 模块权限编号
        /// </summary>
        [JsonProperty]
        public string ModulePermissionID { get; set; }

        /// <summary>
        /// 授权目标编号
        /// </summary>
        [JsonProperty]
        public string TargetID { get; set; }

        /// <summary>
        /// 授权目标类型
        /// </summary>
        [JsonProperty]
        public int TargetType { get; set; }
    }
}
