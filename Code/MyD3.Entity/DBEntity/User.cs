using Newtonsoft.Json;

namespace MyD3.Entity.DBEntity
{
    /// <summary>
    /// 用户
    /// </summary>
    public class User : MyD3Entity
    {
        /// <summary>
        /// 登录名
        /// </summary>
        [JsonProperty]
        public string LoginName { get; set; }

        /// <summary>
        /// 显示名称
        /// </summary>
        [JsonProperty]
        public string DisplayName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [JsonProperty]
        public string Password { get; set; }
    }
}
