using MyD3.Business.Interface.DataModel.User;
using MyD3.Common.Log;
using System;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace MyD3.Core.Security
{
    /// <summary>
    /// 授权验证方法标记
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class MyD3AuthorizeAttribute : FilterAttribute, IAuthorizationFilter
    {
        private readonly ILogger _logger;

        /// <summary>
        /// 初始化权限过滤属性
        /// </summary>
        public MyD3AuthorizeAttribute()
        {
            //由于此属性没有绑定到DI，所以依赖关系只能手动注入
            this._logger = InstanceFactory.CurrentFactory.Get<ILogger>();
        }

        /// <summary>
        /// 授权验证逻辑
        /// </summary>
        /// <param name="filterContext">上下文</param>
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            this._logger.D("开始AuthorizeFilter");

            //获取用户登录信息
            this._logger.D("尝试从Session获取loginInfo");
            var loginInfo = (LoginInfo)filterContext.HttpContext.Session["loginInfo"];

            if (loginInfo == null)
            {
                this._logger.D("获取Session失败，权限验证不通过");
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    //如果ajax请求则返回403代码
                    filterContext.Result = new EmptyResult();
                    filterContext.HttpContext.Response.StatusCode = 403;
                    filterContext.HttpContext.Response.AddHeader("statusText", HttpUtility.UrlEncode("缺少本操作的授权，访问被拒绝！"));
                    filterContext.HttpContext.Response.End();
                    return;
                }
                else
                {
                    //由于Session验证失败，那么尝试跳转到登录界面
                    filterContext.Result = new RedirectResult("~/index/login");
                    return;
                }
            }

            //如果登录信息存在，则开始分析本次请求的路由
            this._logger.D("开始分析路由");
            var area = filterContext.RouteData.Values.ContainsKey("area") ? filterContext.RouteData.Values["area"].ToString().ToUpper() : "";
            var controler = filterContext.RouteData.Values["controller"].ToString().ToUpper();
            var action = filterContext.RouteData.Values["action"].ToString().ToUpper();
            var method = filterContext.HttpContext.Request.HttpMethod.ToUpper();

            this._logger.D("验证用户的授权信息中是否包含此Action对应的Method访问方式");
            if (!loginInfo.AllAuthorization.ModulePermissionAuthorizations.Any(
                x =>
                (string.IsNullOrEmpty(x.Area + area) || x.Area.ToUpper() == area) &&
                x.Controler != null && x.Controler.ToUpper() == controler &&
                x.Action != null && x.Action.ToUpper() == action &&
                x.Method != null && x.Method.ToUpper() == method))
            {
                this._logger.D("验证用户授权失败");
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    this._logger.D("Ajax返回403");
                    //如果ajax请求则返回403代码
                    filterContext.Result = new EmptyResult();
                    filterContext.HttpContext.Response.StatusCode = 403;
                    filterContext.HttpContext.Response.AddHeader("statusText", HttpUtility.UrlEncode("缺少本操作的授权，访问被拒绝！"));
                    filterContext.HttpContext.Response.End();
                    return;
                }
                else
                {
                    this._logger.D("跳转到无权界面");
                    //由于Session验证失败，那么尝试跳转到登录界面
                    filterContext.Result = new RedirectResult("~/index/NoPermission");
                    return;
                }
            }
        }
    }
}
