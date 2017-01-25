using MyD3.Common.Config;
using MyD3.Common.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DS.Business.Tools.Request
{
    public abstract class BattleNetOAuthRequest : BaseRequest
    {
        public const string API_URL_SUFFIX = "_api_url";

        private BattleNetLocation _location;
        private IConfigManager _config;

        /// <summary>
        /// 初始化暴雪Web请求
        /// </summary>
        public BattleNetOAuthRequest(IConfigManager config, BattleNetLocation location)
        {
            this._config = config;
            this._location = location;
        }

        public override FormMethods FormMethod
        {
            get
            {
                return FormMethods.Get;
            }
        }

        /// <summary>
        /// 返回请求链接的前缀
        /// </summary>
        protected override string BaseUrl
        {
            get
            {
                var baseUrl = this._config.Get(this._location.ToString().ToLower() + API_URL_SUFFIX);

                if (string.IsNullOrEmpty(baseUrl))
                {
                    throw new Exception("尚未支持的区域：" + this._location.ToString());
                }

                return baseUrl;
            }
        }

        /// <summary>
        /// 访问会话
        /// </summary>
        [RequestProperty(PropertyName = "access_token")]
        public string AccessToken
        {
            get; set;
        }
    }
}
