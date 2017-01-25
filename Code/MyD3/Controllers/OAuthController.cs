using MyD3.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyD3.Business.Interface;
using MyD3.Common.Cache;
using MyD3.Common.Log;
using DS.Business.Tools;

namespace DemoSite.Controllers
{
    public class OAuthController : BaseController
    {
        private BattleNetOAuthClient _battleNetOAuthClient;

        public OAuthController(ILogger logger, ICacheManager cacheManager, IPageBusiness pageBLL,
            BattleNetOAuthClient battleNetOAuthClient) : base(logger, cacheManager, pageBLL)
        {
            _battleNetOAuthClient = battleNetOAuthClient;
        }

        // GET: OAuth
        public ActionResult Index()
        {
            var authorizUrl = _battleNetOAuthClient.GetAuthorizationReuqestUrl("1");
            return Redirect(authorizUrl);
        }
    }
}