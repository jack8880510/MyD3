using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DS.Business.Tools
{
    /// <summary>
    /// 暴雪访问会话
    /// </summary>
    public class BattleNetAccessToken
    {
        /// <summary>
        /// 会话编号
        /// </summary>
        [JsonProperty(PropertyName = "access_token")]
        public string AccessToken { get; set; }

        /// <summary>
        /// 会话类型
        /// </summary>
        [JsonProperty(PropertyName = "token_type")]
        public string TokenType { get; set; }

        /// <summary>
        /// 会话过期的秒数
        /// </summary>
        [JsonProperty(PropertyName = "expires_in")]
        public long ExpiresIn { get; set; }
    }
}
