using MyD3.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyD3.Business.Interface;
using MyD3.Common.Cache;
using MyD3.Common.Log;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using DS.Business.Tools;
using System.Threading;
using System.Threading.Tasks;
using DS.Business.Tools.Request;
using MyD3.Common.Config;
using DS.Business.Tools.Response;
using System.Net;
using DS.Business.Tools.Request.D3;

namespace DemoSite.Controllers
{
    public class CallbackController : BaseController
    {
        private BattleNetOAuthClient _battleNetOAuthClient;
        private IConfigManager _configManager;

        public CallbackController(ILogger logger, ICacheManager cacheManager, IPageBusiness pageBLL,
            BattleNetOAuthClient battleNetOAuthClient,
            IConfigManager configManager) : base(logger, cacheManager, pageBLL)
        {
            this._battleNetOAuthClient = battleNetOAuthClient;
            this._configManager = configManager;
        }

        public ActionResult D3Profile(string id)
        {
            ProfileRequest request = new ProfileRequest(this._configManager, BattleNetLocation.CN, id);
            BattleNetClient client = new BattleNetClient(this._logger);
            try
            {
                var result = client.InvokeAPI<Object>(request);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (WebException ex)
            {
                if (!ex.Response.IsMutuallyAuthenticated)
                {
                    Response.Write("<p>AppKet授权失败</p>");
                }
                Response.Write("BattleNet远程服务器原始消息：" + ex.Message);
                Response.Flush();
                Response.End();
                return new EmptyResult();
            }
        }


        public ActionResult AccountUser(string id)
        {
            AccountUserRequest request = new AccountUserRequest(this._configManager, BattleNetLocation.CN);
            request.AccessToken = id;
            BattleNetClient client = new BattleNetClient(this._logger);
            try
            {
                var result = client.InvokeAPI<AccountUserResponse>(request);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (WebException ex)
            {
                if (!ex.Response.IsMutuallyAuthenticated)
                {
                    Response.Write("<p>AccessToken授权失败</p>");
                }
                Response.Write("BattleNet远程服务器原始消息：" + ex.Message);
                Response.Flush();
                Response.End();
                return new EmptyResult();
            }
        }

        /// <summary>
        /// 用户在暴雪OAuth界面登录并授权后的回调
        /// 此回调可以得到AuthorizationCode
        /// 使用该Code可以向服务器请求Token
        /// </summary>
        /// <returns></returns>
        public ActionResult AuthorizationRequestCallback()
        {
            var authorizationCode = Request["code"];
            var param = Request["state"];

            var accessTokenTask = Task.Run(() => this._battleNetOAuthClient.GetAccessToken<BattleNetAccessToken>(authorizationCode));
            try
            {
                accessTokenTask.Wait();

                //获取访问的结果
                var accessToken = accessTokenTask.Result;
            }
            catch
            {

            }

            return new EmptyResult();
        }
    }
}