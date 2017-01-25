using MyD3.Entity.DBView;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyD3.Business.Interface.DataModel.User
{
    /// <summary>
    /// 授权信息
    /// </summary>
    public class Authorization
    {
        /// <summary>
        /// 初始化授权信息类
        /// </summary>
        public Authorization()
        {
            this.ModulePermissionAuthorizations = new List<ModulePermissionDetail>();
            this.Non_ModulePermissionAuthorizations = new List<ModulePermissionDetail>();
            this.PageElementAuthorizations = new List<PageElementDetail>();
            this.Non_PageElementAuthorizations = new List<PageElementDetail>();
        }

        /// <summary>
        /// 模块授权信息列表
        /// </summary>
        [JsonProperty]
        public IList<ModulePermissionDetail> ModulePermissionAuthorizations { get; set; }

        /// <summary>
        /// 页面元素授权信息列表
        /// </summary>
        [JsonProperty]
        public IList<PageElementDetail> PageElementAuthorizations { get; set; }

        /// <summary>
        /// 模块无权信息列表
        /// </summary>
        [JsonProperty]
        public IList<ModulePermissionDetail> Non_ModulePermissionAuthorizations { get; set; }

        /// <summary>
        /// 页面元素无权信息列表
        /// </summary>
        [JsonProperty]
        public IList<PageElementDetail> Non_PageElementAuthorizations { get; set; }
    }
}
