using MyD3.Core;
using System.Web.Mvc;
using MyD3.Common.Cache;
using MyD3.Common.Log;
using MyD3.Business.Interface;
using MyD3.Entity;
using DemoSite.Models;
using MyD3.Core.Security;

namespace DemoSite.Controllers
{
    /// <summary>
    /// 授权控制器
    /// </summary>
    public class AuthorizationController : BaseController
    {
        private readonly ISecurityBusiness _securityBLL;

        /// <summary>
        /// 初始化授权控制器
        /// </summary>
        /// <param name="securityBLL">安全业务对象</param>
        /// <param name="logger">日志管理</param>
        /// <param name="cacheManager">缓存管理</param>
        public AuthorizationController(ISecurityBusiness securityBLL,
            ILogger logger, ICacheManager cacheManager, IPageBusiness pageBusiness) 
            : base(logger, cacheManager, pageBusiness)
        {
            this._securityBLL = securityBLL;
        }

        /// <summary>
        /// 分组授权数据接口
        /// </summary>
        /// <param name="args">分组授权参数</param>
        /// <returns></returns>
        [HttpPost]
        [MyD3Authorize]
        public ActionResult GroupAuthoriz(AuthorizArgs args)
        {
            if (args == null)
            {
                this._logger.W("授权参数args为NULL，授权失败");
                return Json(new ResultValue<bool>(-1, "参数信息异常，无法完成授权"));
            }
            return Json(this._securityBLL.GroupAuthoriz(args.TargetID, args.Permissions, this.LoginUser.User.UserData));
        }

        /// <summary>
        /// 用户授权数据接口
        /// </summary>
        /// <param name="args">分组授权参数</param>
        /// <returns></returns>
        [HttpPost]
        [MyD3Authorize]
        public ActionResult UserAuthoriz(AuthorizArgs args)
        {
            if (args == null)
            {
                this._logger.W("授权参数args为NULL，授权失败");
                return Json(new ResultValue<bool>(-1, "参数信息异常，无法完成授权"));
            }
            return Json(this._securityBLL.UserAuthoriz(args.TargetID, args.Permissions, this.LoginUser.User.UserData));
        }

        /// <summary>
        /// 获取分组授权信息
        /// </summary>
        /// <param name="id">需要获取授权的分组编号</param>
        /// <returns></returns>
        [HttpGet]
        [MyD3Authorize]
        public ActionResult GetGroupAuthorization(string id)
        {
            this._logger.I("从业务层获取全部权限信息");
            var allPermissionResult = this._securityBLL.LoadAllModulePermission();

            if (allPermissionResult.RCode != 0)
            {
                this._logger.W("从业务层获取全部权限信息失败：" + allPermissionResult.RMsg);
                return Json(allPermissionResult, JsonRequestBehavior.AllowGet);
            }

            this._logger.I("从业务层获取目标授权");
            var targetAuthorizationResult = this._securityBLL.LoadGroupAuthorization(id);

            if (targetAuthorizationResult.RCode != 0)
            {
                this._logger.W("从业务层获取目标授权失败：" + targetAuthorizationResult.RMsg);
                return Json(targetAuthorizationResult, JsonRequestBehavior.AllowGet);
            }

            this._logger.I("开始封装返回数据");
            ResultValue<TargetAuthorizationVM> result = new ResultValue<TargetAuthorizationVM>();
            result.Data = new TargetAuthorizationVM()
            {
                AllPermissions = allPermissionResult.Data,
                TargetAuthorization = targetAuthorizationResult.Data
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取用户授权信息
        /// </summary>
        /// <param name="id">需要获取授权的用户编号</param>
        /// <returns></returns>
        [HttpGet]
        [MyD3Authorize]
        public ActionResult GetUserAuthorization(string id)
        {
            this._logger.I("从业务层获取全部权限信息");
            var allPermissionResult = this._securityBLL.LoadAllModulePermission();

            if (allPermissionResult.RCode != 0)
            {
                this._logger.W("从业务层获取全部权限信息失败：" + allPermissionResult.RMsg);
                return Json(allPermissionResult, JsonRequestBehavior.AllowGet);
            }

            this._logger.I("从业务层获取目标授权");
            var targetAuthorizationResult = this._securityBLL.LoadUserAuthorizations(id);

            if (targetAuthorizationResult.RCode != 0)
            {
                this._logger.W("从业务层获取目标授权失败：" + targetAuthorizationResult.RMsg);
                return Json(targetAuthorizationResult, JsonRequestBehavior.AllowGet);
            }

            this._logger.I("开始封装返回数据");
            ResultValue<TargetAuthorizationVM> result = new ResultValue<TargetAuthorizationVM>();
            result.Data = new TargetAuthorizationVM()
            {
                AllPermissions = allPermissionResult.Data,
                TargetAuthorization = targetAuthorizationResult.Data
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}