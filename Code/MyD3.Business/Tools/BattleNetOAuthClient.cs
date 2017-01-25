using DS.Common.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyD3.Common.Log;
using MyD3.Common.Config;
using Newtonsoft.Json;

namespace DS.Business.Tools
{
    /// <summary>
    /// 保存游戏OAuth授权
    /// </summary>
    public class BattleNetOAuthClient : OAuth2Client
    {
        public const string AUTHORIZ_URL_SUFFIX = "_authoriz_url";
        public const string TOKEN_URL_SUFFIX = "_token_url";

        private IConfigManager _config;
        private BattleNetLocation _location;

        public BattleNetOAuthClient(ILogger logger, IConfigManager config) : base(logger)
        {
            this._config = config;
            this._location = BattleNetLocation.CN;
        }

        /// <summary>
        /// 获取用户授权页面的Url
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public override string GetAuthorizationReuqestUrl(string state)
        {
            base._logger.I("开始获取用户授权页面基础Url");
            var baseUrl = this._config.Get(this._location.ToString().ToLower() + AUTHORIZ_URL_SUFFIX);

            if (string.IsNullOrEmpty(baseUrl))
            {
                base._logger.W("尚未支持的区域：" + this._location.ToString());
                throw new Exception("尚未支持的区域：" + this._location.ToString());
            }

            base._logger.D("基础Url获取成功：" + baseUrl + "，开始组装请求参数");
            Dictionary<string, string> param = new Dictionary<string, string>()
            {
                { "client_id", this.ClientID },
                { "scope", "" },
                { "state", state },
                { "redirect_uri", this._config.Get("authoriz_callback") },
                { "response_type", "code" },
            };
            baseUrl += baseUrl.Last() == '?' ? "" : "?";
            baseUrl += string.Join("&", param.Select(x => x.Key + "=" + x.Value).ToArray());

            return baseUrl;
        }

        /// <summary>
        /// 获取客户端ID
        /// </summary>
        public override string ClientID
        {
            get
            {
                return _config.Get("client_id");
            }
        }

        /// <summary>
        /// 获取客户端密钥
        /// </summary>
        public override string ClientSecret
        {
            get
            {
                return _config.Get("client_secret");
            }
        }

        /// <summary>
        /// 获取访问会话请求Url
        /// </summary>
        protected override string GetAccessTokenRequestUrl()
        {
            base._logger.I("开始获取访问会话页面基础Url");
            var baseUrl = this._config.Get(this._location.ToString().ToLower() + TOKEN_URL_SUFFIX);

            if (string.IsNullOrEmpty(baseUrl))
            {
                base._logger.W("尚未支持的区域：" + this._location.ToString());
                throw new Exception("尚未支持的区域：" + this._location.ToString());
            }

            return baseUrl;
        }

        /// <summary>
        /// 获取AccessToken请求所需的参数
        /// </summary>
        /// <param name="authorizationCode">用户授权码</param>
        /// <returns></returns>
        protected override IEnumerable<KeyValuePair<string, string>> GetAccessTokenRequestParams(string authorizationCode)
        {
            IList<KeyValuePair<string, string>> param = new List<KeyValuePair<string, string>>();
            param.Add(new KeyValuePair<string, string>("redirect_uri", this._config.Get("authoriz_callback")));
            param.Add(new KeyValuePair<string, string>("scope", ""));
            param.Add(new KeyValuePair<string, string>("grant_type", "authorization_code"));
            param.Add(new KeyValuePair<string, string>("code", authorizationCode));
            return param;
        }

        /// <summary>
        /// 处理访问会话请求的返回数据
        /// </summary>
        /// <param name="responseString"></param>
        /// <returns></returns>
        protected override Object ParseAccessTokenResponseString(string responseString)
        {
            //返回信息
            return JsonConvert.DeserializeObject<BattleNetAccessToken>(responseString);
        }
    }
}
