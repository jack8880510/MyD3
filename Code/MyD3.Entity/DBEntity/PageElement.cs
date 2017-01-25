using Newtonsoft.Json;

namespace MyD3.Entity.DBEntity
{
    /// <summary>
    /// 页面元素表
    /// </summary>
    public class PageElement : MyD3Entity
    {
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

        /// <summary>
        /// 图标资源的路径
        /// </summary>
        [JsonProperty]
        public string Icon { get; set; }

        /// <summary>
        /// 元素的类型，例如按钮0，超级链接1，文本框2等等
        /// </summary>
        [JsonProperty]
        public int Type { get; set; }

        /// <summary>
        /// 页面编号
        /// </summary>
        [JsonProperty]
        public string PageID { get; set; }

        /// <summary>
        /// 需要权限的ID
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
        /// </summary>
        [JsonProperty]
        public int BlockMethod { get; set; }
    }
}
