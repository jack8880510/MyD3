using Newtonsoft.Json;

namespace MyD3.Entity.DBEntity
{
    /// <summary>
    /// 用户分组关系类
    /// </summary>
    public class UserGroup : MyD3Entity
    {
        /// <summary>
        /// 用户编号
        /// </summary>
        [JsonProperty]
        public string UserID { get; set; }

        /// <summary>
        /// 分组编号
        /// </summary>
        [JsonProperty]
        public string GroupID { get; set; }
    }
}
