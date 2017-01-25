using MyD3.Business.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyD3.Entity.DBEntity;
using MyD3.Entity;
using MyD3.Data.Interface;
using MyD3.Common.Cache;
using MyD3.Common.Log;

namespace MyD3.Business
{
    public class PageBusiness : IPageBusiness
    {
        public const string CACHE_PAGE_DATA = "page";

        private readonly IRepository<Page> _pageRps;
        private readonly ICacheManager _cacheManager;
        private readonly ILogger _logger;

        public PageBusiness(IRepository<Page> pageRps, ICacheManager cacheManager, ILogger logger)
        {
            this._pageRps = pageRps;
            this._cacheManager = cacheManager;
            this._logger = logger;
        }

        /// <summary>
        /// 获取所有的页面信息
        /// </summary>
        /// <returns></returns>
        public ResultValue<Page[]> All()
        {
            this._logger.I("开始载入所有页面信息");

            var result = new ResultValue<Page[]>();

            this._logger.I("判断缓存是否包含页面数据");
            if (!this._cacheManager.Exists(CACHE_PAGE_DATA))
            {
                this._logger.I("缓存中不包含页面数据信息，开始从数据库读取页面数据信息");
                var allPage = this._pageRps.Table.ToArray();
                this._logger.I("页面数据获取成功，开始存入缓存");
                this._cacheManager.Set<Page[]>(CACHE_PAGE_DATA, allPage);
            }

            this._logger.I("尝试从缓存中读取页面信息");
            result.Data = this._cacheManager.Get<Page[]>(CACHE_PAGE_DATA);

            this._logger.I("数据读取完成，开始返回");
            return result;
        }

        /// <summary>
        /// 根据页面路由信息获取页面信息对象
        /// </summary>
        /// <param name="area">区域</param>
        /// <param name="controller">控制器</param>
        /// <param name="action">动作</param>
        /// <returns>页面信息对象</returns>
        public ResultValue<Page> Get(string area, string controller, string action)
        {
            this._logger.I("开始获取页面信息：" + area + "/" + controller + "/" + action);
            var allResult = this.All();
            if (allResult.RCode != 0)
            {
                return new ResultValue<Page>(allResult.RCode, allResult.RMsg);
            }
            var resultData = allResult.Data.Where(x =>
            ((string.IsNullOrEmpty(x.Area) && string.IsNullOrEmpty(area) || x.Area.ToLower() == area.ToLower()))
            && x.Controler.ToLower() == controller.ToLower()
            && x.Action.ToLower() == action.ToLower()).FirstOrDefault();
            return new ResultValue<Page>(resultData);
        }
    }
}
