using MyD3.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyD3.Common.Cache;
using MyD3.Common.Log;
using MyD3.Core.Security;
using MyD3.Entity.DBEntity;
using MyD3.Business.Interface;
using MyD3.Business.Interface.DataModel;

namespace DemoSite.Controllers
{
    public class DictionaryController : BaseController
    {
        private readonly IDictionaryBusiness _dictionaryBLL;

        public DictionaryController(IDictionaryBusiness dictionaryBLL,
            ILogger logger, ICacheManager cacheManager, IPageBusiness pageBusiness)
            : base(logger, cacheManager, pageBusiness)
        {
            this._dictionaryBLL = dictionaryBLL;
        }

        [HttpGet]
        [MyD3Authorize]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 禁用指定的索引
        /// </summary>
        /// <param name="id">需要禁用的索引ID</param>
        /// <returns></returns>
        [HttpGet]
        [MyD3Authorize]
        public ActionResult Disable(string id)
        {
            return Json(this._dictionaryBLL.Disable(id, this.LoginUser.User.UserData), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 启用指定的索引
        /// </summary>
        /// <param name="id">需要启用的索引ID</param>
        /// <returns></returns>
        [HttpGet]
        [MyD3Authorize]
        public ActionResult Enable(string id)
        {
            return Json(this._dictionaryBLL.Enable(id, this.LoginUser.User.UserData), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 新增字典数据
        /// </summary>
        /// <param name="model">需要新增的字段数据信息</param>
        /// <returns></returns>
        [HttpPost]
        [MyD3Authorize]
        public ActionResult Post(DictionaryStruct model)
        {
            //直接业务层返回
            return Json(this._dictionaryBLL.Save(model, base.LoginUser.User.UserData));
        }

        /// <summary>
        /// 修改字典数据
        /// </summary>
        /// <param name="model">需要修改的字段数据信息</param>
        /// <returns></returns>
        [HttpPut]
        [MyD3Authorize]
        public ActionResult Put(DictionaryStruct model)
        {
            //直接业务层返回
            return Json(this._dictionaryBLL.Save(model, base.LoginUser.User.UserData));
        }

        /// <summary>
        /// 根据ID获取指定的数据字典结构化数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [MyD3Authorize]
        public ActionResult Get(string id)
        {
            //从业务层返回
            return Json(this._dictionaryBLL.GetStruct(id), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 搜索字典索引信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [MyD3Authorize]
        public ActionResult Search()
        {
            this._logger.I("开始索引搜索，分析搜索条件");
            var searchArgs = this.GetRequestSearchArgs<DictionaryIndex>(this.Request);

            this._logger.I("搜索条件分析完毕，开始调用业务层进行搜索");
            var searchResult = this._dictionaryBLL.Search(searchArgs);

            this._logger.I("搜索执行完毕，执行结果：" + searchResult);
            return Json(searchResult.Data);
        }
    }
}