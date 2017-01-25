using MyD3.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyD3.Common.Cache;
using MyD3.Common.Log;
using MyD3.Business.Interface;
using DemoSite.Models;
using MyD3.Entity;
using MyD3.Entity.DBEntity;
using MyD3.Entity.DBView;
using MyD3.Core.Security;

namespace DemoSite.Controllers
{
    public class SystemSettingController : BaseController
    {
        private readonly ISystemSettingBusiness _sysSettingBLL;
        private readonly IDictionaryBusiness _dictionaryBLL;

        public SystemSettingController(ISystemSettingBusiness sysSettingBLL, IDictionaryBusiness dictionaryBLL,
            ILogger logger, ICacheManager cacheManager, IPageBusiness pageBusiness)
            : base(logger, cacheManager, pageBusiness)
        {
            this._sysSettingBLL = sysSettingBLL;
            this._dictionaryBLL = dictionaryBLL;
        }

        [HttpGet]
        [MyD3Authorize]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 获取系统设置视图数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [MyD3Authorize]
        public ActionResult All()
        {
            this._logger.I("开始获取全部的设置数据");
            var allSettingResult = this._sysSettingBLL.AllSystemSettingDetail();

            if (allSettingResult.RCode != 0)
            {
                this._logger.W("获取全部设置数据失败，无法加载设置视图数据");
                return Json(allSettingResult);
            }

            this._logger.I("开始获取所有相关联的数据字典值");
            allSettingResult.Data.Where(x => x.ValueType == "OPTION" && x.OptionType == 1).ToList().ForEach(delegate (SystemSettingDetail x)
           {
               if (string.IsNullOrEmpty(x.OptionValue))
               {
                   this._logger.W("由于缺少关联数据字典表的编号，获取配置数据失败了：" + x.Name);
                   throw new ArgumentException("缺少关联数据字典表的编号");
               }

               //获取相关的数据字典值
               var dictionaryDataDetail = this._dictionaryBLL.GetStruct(x.OptionValue);

               if (dictionaryDataDetail.RCode != 0)
               {
                   this._logger.W("从数据库获取字典数据失败：" + dictionaryDataDetail);
                   throw new Exception("从数据库获取字典数据失败");
               }

               x.OptionValue = string.Join(",", dictionaryDataDetail.Data.Data.OrderBy(y => y.OrderNo).Select(y => y.Value).ToArray());
               x.OptionText = string.Join(",", dictionaryDataDetail.Data.Data.OrderBy(y => y.OrderNo).Select(y => y.Name).ToArray());
           });

            //组装数据
            SysSettingIndexVM result = new SysSettingIndexVM();
            result.SettingGroups = allSettingResult.Data.GroupBy(x => x.GroupID, y => y).Select(x => new SysSettingIndexGroupVM()
            {
                GroupID = x.ToList()[0].GroupID,
                GroupName = x.ToList()[0].GroupName,
                OrderNo = x.ToList()[0].GroupOrderNo,
                Settings = x.Where(y => y.GroupID == x.Key).OrderBy(y => y.OrderNo).ToArray()
            }).OrderBy(x => x.OrderNo).ToArray();

            return Json(new ResultValue<SysSettingIndexVM>(result), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 更新系统设置
        /// </summary>
        /// <param name="args">系统配置页面参数</param>
        /// <returns></returns>
        [HttpPut]
        [MyD3Authorize]
        public ActionResult Put(SaveSystemSettingArgs[] args)
        {
            //直接从业务层返回
            return Json(this._sysSettingBLL.Save(args.ToDictionary(x => x.Name, y => y.Value), base.LoginUser.User.UserData));
        }
    }
}