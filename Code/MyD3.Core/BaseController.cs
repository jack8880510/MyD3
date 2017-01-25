using MyD3.Business.Interface;
using MyD3.Business.Interface.DataModel.User;
using MyD3.Common.Cache;
using MyD3.Common.Log;
using MyD3.Entity;
using MyD3.Entity.DBEntity;
using MyD3.Entity.DBView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MyD3.Core
{
    /// <summary>
    /// 控制器基类
    /// </summary>
    public class BaseController : Controller
    {
        protected readonly ILogger _logger;
        protected readonly ICacheManager _cacheManager;
        private readonly IPageBusiness _pageBLL;

        public BaseController(ILogger logger, ICacheManager cacheManager, IPageBusiness pageBLL)
        {
            this._logger = logger;
            this._cacheManager = cacheManager;
            this._pageBLL = pageBLL;
        }

        /// <summary>
        /// 重写View方法，添加获取页面信息的代码
        /// </summary>
        /// <param name="viewName"></param>
        /// <param name="masterName"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        protected override ViewResult View(string viewName, string masterName, object model)
        {
            //获取当前路由信息
            var area = this.RouteData.Values["area"] + "";
            var controller = this.RouteData.Values["controller"] + "";
            var action = this.RouteData.Values["action"] + "";

            //从业务层获取页面信息
            var pageInfo = this._pageBLL.Get(area, controller, action);

            if (pageInfo.RCode == 0 && pageInfo.Data != null)
            {
                //记录页面的结构路径
                ViewBag.PageTreePath = pageInfo.Data.TreePath;
            }

            return base.View(viewName, masterName, model);
        }

        /// <summary>
        /// 获取当前登录用户的信息
        /// </summary>
        protected LoginInfo LoginUser
        {
            get
            {
                return Session["loginInfo"] as LoginInfo;
            }
        }

        /// <summary>
        /// 重写Json序列化方法
        /// </summary>
        /// <param name="data"></param>
        /// <param name="contentType"></param>
        /// <param name="contentEncoding"></param>
        /// <param name="behavior"></param>
        /// <returns></returns>
        protected override JsonResult Json(object data, string contentType, Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new JsonNetResult
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior
            };
        }

        /// <summary>
        /// 从请求中获取搜索条件
        /// </summary>
        /// <returns></returns>
        protected SearchArgs GetRequestSearchArgs<T>(HttpRequestBase request)
        {
            return GetRequestSearchArgs<T>(request, null, false);
        }

        /// <summary>
        /// 从请求中获取搜索条件
        /// </summary>
        /// <returns></returns>
        protected SearchArgs GetRequestSearchArgs<T>(HttpRequestBase request, bool sortDesc)
        {
            return GetRequestSearchArgs<T>(request, null, sortDesc);
        }

        /// <summary>
        /// 从请求中获取搜索条件
        /// </summary>
        /// <returns></returns>
        protected SearchArgs GetRequestSearchArgs<T>(HttpRequestBase request, string sortField, bool sortDesc)
        {
            //获取所有请求参数Key
            var requestKey = Request.Params.AllKeys.ToArray();

            //创建搜索条件对象
            SearchArgs args = new SearchArgs();

            //从Request中获取对应的值
            args.ConditionString = requestKey.Contains("searchPhrase") ? Request["searchPhrase"] : "";
            args.PageNumber = requestKey.Contains("current") ? int.Parse(Request["current"]) : 0;
            args.PageSize = requestKey.Contains("rowCount") ? int.Parse(Request["rowCount"]) : 0;

            var propertys = typeof(T).GetProperties().Select(x => "sort[" + x.Name + "]").ToArray();
            var sortName = string.IsNullOrEmpty(sortField) ? propertys.Where(x => request.Params.AllKeys.Contains(x)).FirstOrDefault() : sortField.Trim();

            if (string.IsNullOrEmpty(sortName))
            {
                sortName = propertys.FirstOrDefault();
            }

            //排序方式
            if (!string.IsNullOrEmpty(sortName))
            {
                args.SortName = string.IsNullOrEmpty(sortField) ? sortName.Substring(sortName.IndexOf("[") + 1, sortName.LastIndexOf("]") - (sortName.IndexOf("[")) - 1) : sortField.Trim();
                if (sortDesc)
                {
                    args.SortDESC = request.Params.AllKeys.Contains(sortName) ? request[sortName].ToUpper() == "DESC" : true;
                }
                else
                {
                    args.SortDESC = request.Params.AllKeys.Contains(sortName) ? request[sortName].ToUpper() == "DESC" : false;
                }
            }

            //查询条件
            string key_heard = "search_condition_";
            var searchConditionKey = request.Params.AllKeys.Where(x => x.IndexOf(key_heard) == 0).ToList();
            foreach (var key in searchConditionKey)
            {
                if (!request.Params.AllKeys.Contains(key))
                {
                    continue;
                }
                else if (key.Length <= key_heard.Length)
                {
                    continue;
                }
                else if (string.IsNullOrEmpty(request[key]))
                {
                    continue;
                }

                args.SearchCondition.Add(key.Substring(key_heard.Length), request[key]);
            }

            return args;
        }
    }
}
