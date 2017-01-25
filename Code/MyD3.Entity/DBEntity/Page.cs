using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyD3.Entity.DBEntity
{
    /// <summary>
    /// 页面
    /// </summary>
    public class Page : MyD3Entity
    {
        /// <summary>
        /// 模块编号
        /// </summary>
        [JsonProperty]
        public string ModuleID { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [JsonProperty]
        public string Name { get; set; }

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
        /// 父页面编号
        /// </summary>
        [JsonProperty]
        public string ParentPageID { get; set; }

        /// <summary>
        /// 树形结构层级路径
        /// </summary>
        [JsonProperty]
        public string TreePath { get; set; }

        /// <summary>
        /// 所在树形结构的节点级别
        /// </summary>
        [JsonProperty]
        public int TreeLevel { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [JsonProperty]
        public string Description { get; set; }
    }
}
