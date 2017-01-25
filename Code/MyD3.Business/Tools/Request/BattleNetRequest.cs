using MyD3.Common.Config;
using MyD3.Common.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DS.Business.Tools.Request
{
    public abstract class BattleNetRequest : BaseRequest
    {
        public const string API_URL_SUFFIX = "_api_url";

        private BattleNetLocation _location;
        private IConfigManager _config;

        /// <summary>
        /// 初始化暴雪Web请求
        /// </summary>
        public BattleNetRequest(IConfigManager config, BattleNetLocation location)
        {
            this._config = config;
            this._location = location;
        }

        /// <summary>
        /// 获取客户端ID
        /// </summary>
        [RequestProperty(PropertyName = "apikey")]
        public string ClientID
        {
            get
            {
                return _config.Get("client_id");
            }
        }

        /// <summary>
        /// API数据需求的语言
        /// </summary>
        [RequestProperty(PropertyName = "locale")]
        public string Langurage { get; set; }

        public override FormMethods FormMethod
        {
            get
            {
                return FormMethods.Get;
            }
        }

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
    }
}
