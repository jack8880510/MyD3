using MyD3.Common.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace DS.Common.OAuth
{
    /// <summary>
    /// 定义了OAuth访问的基础
    /// </summary>
    public abstract class OAuth2Client
    {
        protected ILogger _logger;

        public OAuth2Client(ILogger logger)
        {
            this._logger = logger;
        }

        /// <summary>
        /// 获取一个值，表示客户端的授权编号
        /// </summary>
        public abstract string ClientID { get; }

        /// <summary>
        /// 获取一个值，表示客户端的验证密匙
        /// </summary>
        public abstract string ClientSecret { get; }

        /// <summary>
        /// 获取一个值，表示用户授权的Url地址
        /// </summary>
        /// <param name="state">请求的参数信息</param>
        /// <returns></returns>
        public abstract string GetAuthorizationReuqestUrl(string state);

        /// <summary>
        /// 获取一个值，表示访问回话的Url地址
        /// </summary>
        /// <returns></returns>
        protected abstract string GetAccessTokenRequestUrl();

        /// <summary>
        /// 获取AccessToken请求的RequestParams
        /// </summary>
        /// <param name="authorizationCode">用户授权码</param>
        /// <returns></returns>
        protected abstract IEnumerable<KeyValuePair<string, string>> GetAccessTokenRequestParams(string authorizationCode);

        /// <summary>
        /// 处理AccessToken成功获取后的ResponseString
        /// </summary>
        /// <typeparam name="T">Token的类型</typeparam>
        /// <param name="responseString">RespStr内容</param>
        /// <returns>Token对象</returns>
        protected abstract Object ParseAccessTokenResponseString(string responseString);

        /// <summary>
        /// 获取访问会话
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="authorizationCode">用户授权码</param>
        /// <returns></returns>
        public async Task<T> GetAccessToken<T>(string authorizationCode)
        {
            this._logger.I("获取客户端授权所需的Base64秘钥 Http Basic Authorization规范");
            string credentialsStr = ClientID + ":" + ClientSecret;
            this._logger.I("获取成功：" + credentialsStr + "，开始Base64加密");
            var credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes(credentialsStr));
            this._logger.I("加密成功：" + credentials + "，开始创建HttpClient");
            var httpClient = new HttpClient();
            this._logger.I("开始设置请求秘钥");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);
            this._logger.I("设置自定义Http请求参数");
            var httpContent = new FormUrlEncodedContent(GetAccessTokenRequestParams(authorizationCode));
            try
            {
                this._logger.I("开始请求AccessToken");
                var response = await httpClient.PostAsync(this.GetAccessTokenRequestUrl(), httpContent);
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    this._logger.I("请求完毕，但服务器返回失败：" + response.StatusCode);
                    throw new Exception("获取AccessToken失败，服务器返回的Code：" + response.StatusCode + " - " + response.ToString());
                }
                this._logger.I("请求完毕，开始读取返回内容");
                var responseContent = await response.Content.ReadAsStringAsync();
                this._logger.I("内容：" + responseContent + "，开始解析返回内容");
                return (T)ParseAccessTokenResponseString(responseContent);
            }
            catch (Exception ex)
            {
                this._logger.E("请求或解析AccessToken时出现异常：" + ex.StackTrace, ex);
                throw ex;
            }
        }
    }
}
