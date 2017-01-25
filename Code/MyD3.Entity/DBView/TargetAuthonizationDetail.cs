using MyD3.Data.Entities;
using Newtonsoft.Json;

namespace MyD3.Entity.DBView
{
    /// <summary>
    /// 模块权限授权信息
    /// </summary>
    public class TargetAuthonizationDetail : DbEntity
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
        /// 权限编号
        /// </summary>
        [JsonProperty]
        public string PermissionID { get; set; }

        /// <summary>
        /// 权限名称
        /// </summary>
        [JsonProperty]
        public string PermissionName { get; set; }

        /// <summary>
        /// 区域
        /// </summary>
        [JsonProperty]
        public string Area { get; set; }

        /// <summary>
        /// 控制器名称
        /// </summary>
        [JsonProperty]
        public string Controler { get; set; }

        /// <summary>
        /// Action名称
        /// </summary>
        [JsonProperty]
        public string Action { get; set; }

        /// <summary>
        /// 请求方式
        /// </summary>
        [JsonProperty]
        public string Method { get; set; }

        /// <summary>
        /// 请求名称
        /// </summary>
        [JsonProperty]
        public string ActionName { get; set; }

        [JsonProperty]
        public string TargetID { get; set; }

        [JsonProperty]
        public int TargetType { get; set; }
    }
}
