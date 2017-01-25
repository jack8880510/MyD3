using MyD3.Data.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyD3.Entity.DBView
{
    /// <summary>
    /// 页面元素详细信息
    /// </summary>
    public class TargetPageElementDetail : DbEntity
    {
        /// <summary>
        /// 模块编号
        /// </summary>
        [JsonProperty]
        public string ModuleID { get; set; }

        /// <summary>
        /// 模块名称
        /// </summary>
        [JsonProperty]
        public string ModuleName { get; set; }

        /// <summary>
        /// 模块描述
        /// </summary>
        [JsonProperty]
        public string ModuleDescription { get; set; }

        /// <summary>
        /// 模块图标资源的路径
        /// </summary>
        [JsonProperty]
        public string ModuleIcon { get; set; }

        /// <summary>
        /// 页面编号
        /// </summary>
        [JsonProperty]
        public string PageID { get; set; }

        /// <summary>
        /// 页面名称
        /// </summary>
        [JsonProperty]
        public string PageName { get; set; }

        /// <summary>
        /// 页面描述
        /// </summary>
        [JsonProperty]
        public string PageDescription { get; set; }

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
        /// 页面树形结构层级路径
        /// </summary>
        [JsonProperty]
        public string PageTreePath { get; set; }

        /// <summary>
        /// 页面所在树形结构的节点级别
        /// </summary>
        [JsonProperty]
        public int PageTreeLevel { get; set; }

        /// <summary>
        /// 元素编号
        /// </summary>
        [JsonProperty]
        public string ElementID { get; set; }

        /// <summary>
        /// 元素名称
        /// </summary>
        [JsonProperty]
        public string ElementName { get; set; }

        /// <summary>
        /// 元素描述
        /// </summary>
        [JsonProperty]
        public string ElementDescription { get; set; }

        /// <summary>
        /// 元素图标资源的路径
        /// </summary>
        [JsonProperty]
        public string ElementIcon { get; set; }

        /// <summary>
        /// 元素的类型，例如按钮，超级链接，文本框等等
        /// </summary>
        [JsonProperty]
        public int ElementType { get; set; }

        /// <summary>
        /// 元素需要权限的ID
        /// </summary>
        [JsonProperty]
        public string NeedPermissionID { get; set; }

        /// <summary>
        /// 元素选择器
        /// </summary>
        [JsonProperty]
        public string Selector { get; set; }

        /// <summary>
        /// 屏蔽方式
        /// 0：默认方式，使用隐藏的方式
        /// </summary>
        [JsonProperty]
        public int BlockMethod { get; set; }

        [JsonProperty]
        public string TargetID { get; set; }

        [JsonProperty]
        public int TargetType { get; set; }
    }
}
