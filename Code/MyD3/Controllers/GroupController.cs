using DemoSite.Models;
using MyD3.Business.Interface;
using MyD3.Common.Cache;
using MyD3.Common.Log;
using MyD3.Core;
using MyD3.Core.Security;
using MyD3.Entity;
using MyD3.Entity.DBEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DemoSite.Controllers
{
    public class GroupController : BaseController
    {
        private readonly IGroupBusiness _groupBLL;
        private readonly IUserBusiness _userBLL;

        /// <summary>
        /// 初始化分组控制器
        /// </summary>
        /// <param name="groupBLL"></param>
        /// <param name="logger"></param>
        /// <param name="cacheManager"></param>
        public GroupController(IGroupBusiness groupBLL, IUserBusiness userBLL,
            ILogger logger, ICacheManager cacheManager, IPageBusiness pageBusiness)
            : base(logger, cacheManager, pageBusiness)
        {
            this._groupBLL = groupBLL;
            this._userBLL = userBLL;
        }

        /// <summary>
        /// 打开分组管理界面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [MyD3Authorize]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 根据ID获取分组信息对象
        /// </summary>
        /// <param name="id">分组系统编号</param>
        /// <returns>分组信息JSON</returns>
        [HttpGet]
        [MyD3Authorize]
        public ActionResult Get(string id)
        {
            return Json(this._groupBLL.Get(id), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 新增分组信息
        /// </summary>
        /// <param name="model">需要新增的分组对象</param>
        /// <returns></returns>
        [HttpPost]
        [MyD3Authorize]
        public ActionResult Post(Group model)
        {
            //调用业务层进行处理
            return Json(this._groupBLL.Save(model, base.LoginUser.User.UserData));
        }

        /// <summary>
        /// 新增分组信息
        /// </summary>
        /// <param name="model">需要新增的分组对象</param>
        /// <returns></returns>
        [HttpPut]
        [MyD3Authorize]
        public ActionResult Put(Group model)
        {
            //调用业务层进行处理
            return Json(this._groupBLL.Save(model, base.LoginUser.User.UserData));
        }

        /// <summary>
        /// 删除分组
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpDelete]
        [MyD3Authorize]
        public ActionResult Delete(Group model)
        {
            //调用业务层进行处理
            return Json(this._groupBLL.Delete(model, base.LoginUser.User.UserData));
        }

        /// <summary>
        /// 搜索分组信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [MyD3Authorize]
        public ActionResult Search()
        {
            this._logger.I("开始分组搜索，分析搜索条件");
            var searchArgs = this.GetRequestSearchArgs<Group>(this.Request);

            this._logger.I("搜索条件分析完毕，开始调用业务层进行搜索");
            var searchResult = this._groupBLL.Search(searchArgs);

            this._logger.I("搜索执行完毕，执行结果：" + searchResult);
            return Json(searchResult.Data);
        }

        /// <summary>
        /// 获取分组下的全部成员
        /// </summary>
        /// <param name="id">分组编号</param>
        /// <returns></returns>
        [HttpGet]
        [MyD3Authorize]
        public ActionResult GetGroupMembers(string id)
        {
            //直接从业务层获取
            return Json(this._userBLL.GetGroupMembers(id), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 设置分组成员
        /// </summary>
        /// <param name="args">设置分组成员参数对象</param>
        /// <returns></returns>
        [HttpPost]
        [MyD3Authorize]
        public ActionResult SetGroupMember(SetGroupMembersArgs args)
        {
            if (args == null)
            {
                this._logger.W("参数args为NULL，授权失败");
                return Json(new ResultValue<bool>(-1, "信息异常，无法完成授权"));
            }
            //直接从业务层处理
            return Json(this._groupBLL.SetGroupMember(args.GroupID, args.UserIDList, this.LoginUser.User.UserData));
        }
    }
}