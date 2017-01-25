using MyD3.Business.Interface;
using MyD3.Common.Cache;
using MyD3.Common.Log;
using MyD3.Core;
using MyD3.Core.Security;
using MyD3.Entity.DBEntity;
using System.Web.Mvc;

namespace DemoSite.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserBusiness _userBLL;

        public UserController(IUserBusiness userBLL, ILogger logger, ICacheManager cacheManager, IPageBusiness pageBusiness)
            : base(logger, cacheManager, pageBusiness)
        {
            this._userBLL = userBLL;
        }

        /// <summary>
        /// 查看用户管理页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [MyD3Authorize]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 根据ID获取用户信息对象
        /// </summary>
        /// <param name="id">用户系统编号</param>
        /// <returns>用户信息JSON</returns>
        [HttpGet]
        [MyD3Authorize]
        public ActionResult Get(string id)
        {
            return Json(this._userBLL.Get(id), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 新增用户信息
        /// </summary>
        /// <param name="model">需要新增的用户对象</param>
        /// <returns></returns>
        [HttpPost]
        [MyD3Authorize]
        public ActionResult Post(User model)
        {
            //调用业务层进行处理
            return Json(this._userBLL.Save(model, base.LoginUser.User.UserData));
        }

        /// <summary>
        /// 新增用户信息
        /// </summary>
        /// <param name="model">需要新增的用户对象</param>
        /// <returns></returns>
        [HttpPut]
        [MyD3Authorize]
        public ActionResult Put(User model)
        {
            //调用业务层进行处理
            return Json(this._userBLL.Save(model, base.LoginUser.User.UserData));
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpDelete]
        [MyD3Authorize]
        public ActionResult Delete(User model)
        {
            //调用业务层进行处理
            return Json(this._userBLL.Delete(model, base.LoginUser.User.UserData));
        }

        /// <summary>
        /// 搜索用户信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [MyD3Authorize]
        public ActionResult Search()
        {
            this._logger.I("开始用户搜索，分析搜索条件");
            var searchArgs = this.GetRequestSearchArgs<User>(this.Request);

            this._logger.I("搜索条件分析完毕，开始调用业务层进行搜索");
            var searchResult = this._userBLL.Search(searchArgs);

            this._logger.I("搜索执行完毕，执行结果：" + searchResult);
            return Json(searchResult.Data);
        }
    }
}