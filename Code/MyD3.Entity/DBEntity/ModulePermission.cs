using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyD3.Entity.DBEntity
{
    /// <summary>
    /// 模块权限类
    /// </summary>
    public class ModulePermission : MyD3Entity
    {
        /// <summary>
        /// 模块编号
        /// </summary>
        [JsonProperty]
        public string ModuleID { get; set; }

        /// <summary>
        /// MVC Controler所在的分区
        /// </summary>
        [JsonProperty]
        public string Area { get; set; }

        /// <summary>
        /// MVC Controller
        /// </summary>
        [JsonProperty]
        public string Controler { get; set; }

        /// <summary>
        /// MVC Action
        /// </summary>
        [JsonProperty]
        public string Action { get; set; }

        /// <summary>
        /// Request Method
        /// </summary>
        [JsonProperty]
        public string Method { get; set; }

        /// <summary>
        /// ActionName
        /// </summary>
        [JsonProperty]
        public string ActionName { get; set; }

        /// <summary>
        /// ActionDescription
        /// </summary>
        [JsonProperty]
        public string ActionDescription { get; set; }
    }
}
