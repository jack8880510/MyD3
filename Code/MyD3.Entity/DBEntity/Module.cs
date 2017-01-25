using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyD3.Entity.DBEntity
{
    /// <summary>
    /// 模块类
    /// </summary>
    public class Module : MyD3Entity
    {
        /// <summary>
        /// 图标资源的路径
        /// </summary>
        [JsonProperty]
        public string Icon { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [JsonProperty]
        public string Name { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [JsonProperty]
        public string Description { get; set; }
    }
}
