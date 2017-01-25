using MyD3.Business.Interface;
using MyD3.Business.Interface.DataModel.User;
using MyD3.Common.Cache;
using MyD3.Common.Log;
using MyD3.Core;
using MyD3.Core.Security;
using MyD3.Entity;
using System;
using System.Web.Mvc;

namespace DemoSite.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IUserBusiness _userBLL;

        public HomeController(IUserBusiness userBLL, ILogger logger,
            ICacheManager cacheManager, IPageBusiness pageBusiness)
            : base(logger, cacheManager, pageBusiness)
        {
            this._userBLL = userBLL;
        }

        [HttpGet]
        [MyD3Authorize]
        public ActionResult Index()
        {
            return View();
        }
    }
}