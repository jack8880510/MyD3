using MyD3.Business.Interface;
using MyD3.Business.Interface.DataModel.User;
using MyD3.Common.Cache;
using MyD3.Common.Config;
using MyD3.Common.Log;
using MyD3.Core;
using MyD3.Entity;
using System;
using System.Web.Mvc;

namespace DemoSite.Controllers
{
    public class IndexController : BaseController
    {
        private readonly IUserBusiness _userBLL;

        public IndexController(IUserBusiness userBLL,
            ILogger logger, ICacheManager cacheManager, IPageBusiness pageBusiness)
            : base(logger, cacheManager, pageBusiness)
        {
            this._userBLL = userBLL;
        }

        /// <summary>
        /// 登录页面Action
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// 注销请求Action
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Logout()
        {
            this.ClearCache();
            return Redirect("login");
        }

        /// <summary>
        /// 登录请求处理Action
        /// </summary>
        /// <param name="loginArgs"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Login(LoginArgs loginArgs)
        {
            this._logger.I("收到登录请求，开始登录流程");

            ResultValue<LoginInfo> result = null;

            try
            {
                this._logger.I("调用业务层登录处理方法");
                result = this._userBLL.Login(loginArgs);

                if (result.RCode == 0)
                {
                    this._logger.I("登录成功，设置登录结果到Session");
                    Session["loginInfo"] = result.Data;
                }
            }
            catch (Exception ex)
            {
                this._logger.E("登录时发生异常", ex);
                result = new ResultValue<LoginInfo>(-1, "登录时发生异常：" + ex.ToString());
            }

            return Json(result);
        }

        /// <summary>
        /// 清空所有的缓存
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ClearCache()
        {
            try
            {
                Session.Remove("loginInfo");
                this._cacheManager.RemoveAll();
            }
            catch
            {

            }
            finally
            {
            }
            return new EmptyResult();
        }

        /// <summary>
        /// 无权限界面
        /// </summary>
        /// <returns></returns>
        public ActionResult NoPermission()
        {
            return View();
        }
    }
}