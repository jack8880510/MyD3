using MyD3.Business.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using MyD3.Business.Interface.DataModel.User;
using MyD3.Entity;
using MyD3.Entity.DBView;
using MyD3.Data.Interface;
using MyD3.Common.Log;
using MyD3.Common.Cache;
using MyD3.Data.Interface.DataModel;
using Newtonsoft.Json;
using MyD3.Entity.DBEntity;
using System.Transactions;

namespace MyD3.Business
{
    /// <summary>
    /// 安全业务处理类
    /// </summary>
    public class SecurityBusiness : ISecurityBusiness
    {
        public const string CACHE_GROUP_AUTH_LIST = "group_auth";
        public const string CACHE_USER_AUTH_LIST = "user_auth";
        public const string CACHE_MODULE_PERMISSION_DATA = "module_permission";
        public const string CACHE_PAGE_ELEMENT_DATA = "page_element";

        private readonly IRepository<ModulePermissionDetail> _permissionDetailRps;
        private readonly IRepository<PageElementDetail> _pageElementDetailRps;
        private readonly IRepository<TargetAuthonizationDetail> _targetAuthonizationDetailRps;
        private readonly IRepository<TargetPageElementDetail> _targetElementDetailRps;
        private readonly IRepository<TargetAuthonization> _targetAuthonizationRps;
        private readonly IGroupBusiness _groupBll;
        private readonly IRepository<User> _userRps;
        private readonly ILogger _logger;
        private readonly ICacheManager _cacheManager;

        /// <summary>
        /// 安全业务类
        /// </summary>
        /// <param name="securityRps">安全信息数据仓库</param>
        /// <param name="groupRps">分组数据层</param>
        /// <param name="userRps">用户数据层</param>
        /// <param name="logger">日志管理组件</param>
        /// <param name="cacheManager">缓存管理组件</param>
        public SecurityBusiness(IRepository<ModulePermissionDetail> permissionDetailRps, IGroupBusiness groupBll, IRepository<User> userRps,
            IRepository<PageElementDetail> pageElementDetailRps, IRepository<TargetAuthonizationDetail> authonizationDetailRps,
            IRepository<TargetPageElementDetail> targetElementDetailRps,
            IRepository<TargetAuthonization> targetAuthonizationRps,
            ILogger logger, ICacheManager cacheManager)
        {
            this._permissionDetailRps = permissionDetailRps;
            this._pageElementDetailRps = pageElementDetailRps;
            this._targetAuthonizationDetailRps = authonizationDetailRps;
            this._targetElementDetailRps = targetElementDetailRps;
            this._targetAuthonizationRps = targetAuthonizationRps;
            this._groupBll = groupBll;
            this._userRps = userRps;
            this._logger = logger;
            this._cacheManager = cacheManager;
        }

        /// <summary>
        /// 载入分组授权
        /// </summary>
        /// <param name="groupID">需要获取授权的分组ID</param>
        /// <returns>分组授权信息</returns>
        public ResultValue<Authorization> LoadGroupAuthorization(string groupID)
        {
            this._logger.I("开始载入分组授权");
            if (string.IsNullOrEmpty(groupID))
            {
                this._logger.W("分组编号参数不合法，分组编号不能为NULL或空");
                return new ResultValue<Authorization>(-1, "分组编号参数不合法，分组编号不能为NULL或空");
            }

            var result = new ResultValue<Authorization>();

            this._logger.I("判断缓存是否包含分组授权数据");
            if (!this._cacheManager.ExistsFromList<Authorization>(CACHE_GROUP_AUTH_LIST, groupID))
            {
                //封装授权所需的对象
                Authorization data = new Authorization();

                this._logger.I("缓存中没有分组授权数据，开始从数据库获取数据");
                this._logger.I("开始获取所有的模块权限信息");
                var allModulePermissionResult = this.LoadAllModulePermission();

                if (allModulePermissionResult.RCode != 0)
                {
                    this._logger.W("模块权限信息获取失败：" + allModulePermissionResult.RMsg);
                    return new ResultValue<Authorization>(-1, "模块权限信息获取失败：" + allModulePermissionResult.RMsg);
                }

                this._logger.I("开始获取分组授权数据");
                var groupModulePermission = this._targetAuthonizationDetailRps.Table.Where(x => x.TargetID == groupID && x.TargetType == (int)TargetType.Group).ToList();

                this._logger.I("开始获取所有的页面元素权限信息");
                var allPageElementResult = this.LoadAllPageElement();

                if (allPageElementResult.RCode != 0)
                {
                    this._logger.W("页面元素信息获取失败：" + allPageElementResult.RMsg);
                    return new ResultValue<Authorization>(-1, "页面元素信息获取失败：" + allPageElementResult.RMsg);
                }

                this._logger.I("开始获取分组页面元素授权数据");
                var groupPageElement = this._targetElementDetailRps.Table.Where(x => x.TargetID == groupID && x.TargetType == (int)TargetType.Group).ToList();

                data.ModulePermissionAuthorizations = groupModulePermission.Select(x => new ModulePermissionDetail(x) { }).ToList();
                this._logger.I("分组授权信息获取成功");
                data.Non_ModulePermissionAuthorizations = allModulePermissionResult.Data.Where(x => !groupModulePermission.Any(y => y.PermissionID == x.PermissionID)).ToArray();
                this._logger.I("分组未授权分析成功");
                data.PageElementAuthorizations = groupPageElement.Select(x => new PageElementDetail(x)).ToList();
                this._logger.I("分组页面元素授权信息获取成功");
                data.Non_PageElementAuthorizations = allPageElementResult.Data.Where(x => !groupPageElement.Any(y => y.NeedPermissionID == x.NeedPermissionID)).ToArray();
                this._logger.I("分组页面元素未授权分析成功");

                this._logger.I("开始写入分组授权信息到缓存");
                this._cacheManager.AddToList<Authorization>(CACHE_GROUP_AUTH_LIST, groupID, data);
            }

            this._logger.I("尝试从缓存中读取该分组的授权信息");
            result.Data = this._cacheManager.GetFromList<Authorization>(CACHE_GROUP_AUTH_LIST, groupID);

            this._logger.I("数据读取完成，开始返回");
            return result;
        }

        /// <summary>
        /// 载入用户个人授权
        /// </summary>
        /// <param name="userID">需要获取授权信息的用户ID</param>
        /// <returns>用户个人授权信息</returns>
        public ResultValue<Authorization> LoadUserAuthorizations(string userID)
        {
            this._logger.I("开始载入用户授权");
            if (string.IsNullOrEmpty(userID))
            {
                this._logger.W("用户编号参数不合法，用户编号不能为NULL或空");
                return new ResultValue<Authorization>(-1, "用户编号参数不合法，用户编号不能为NULL或空");
            }

            //封装授权所需的对象
            Authorization data = new Authorization();

            this._logger.I("开始获取所有的模块权限信息");
            var allModulePermissionResult = this.LoadAllModulePermission();

            if (allModulePermissionResult.RCode != 0)
            {
                this._logger.W("模块权限信息获取失败：" + allModulePermissionResult.RMsg);
                return new ResultValue<Authorization>(-1, "模块权限信息获取失败：" + allModulePermissionResult.RMsg);
            }

            this._logger.I("开始获取用户授权数据");
            var userModulePermission = this._targetAuthonizationDetailRps.Table.Where(x => x.TargetID == userID && x.TargetType == (int)TargetType.User).ToList();

            this._logger.I("开始获取所有的页面元素权限信息");
            var allPageElementResult = this.LoadAllPageElement();

            if (allPageElementResult.RCode != 0)
            {
                this._logger.W("页面元素信息获取失败：" + allPageElementResult.RMsg);
                return new ResultValue<Authorization>(-1, "页面元素信息获取失败：" + allPageElementResult.RMsg);
            }

            this._logger.I("开始获取用户页面元素授权数据");
            var userPageElement = this._targetElementDetailRps.Table.Where(x => x.TargetID == userID && x.TargetType == (int)TargetType.User).ToList();

            data.ModulePermissionAuthorizations = userModulePermission.Select(x => new ModulePermissionDetail(x) { }).ToList();
            this._logger.I("用户授权信息获取成功");
            data.Non_ModulePermissionAuthorizations = allModulePermissionResult.Data.Where(x => !userModulePermission.Any(y => y.PermissionID == x.PermissionID)).ToArray();
            this._logger.I("用户未授权分析成功");
            data.PageElementAuthorizations = userPageElement.Select(x => new PageElementDetail(x)).ToList();
            this._logger.I("用户页面元素授权信息获取成功");
            data.Non_PageElementAuthorizations = allPageElementResult.Data.Where(x => !userPageElement.Any(y => y.NeedPermissionID == x.NeedPermissionID)).ToArray();
            this._logger.I("用户页面元素未授权分析成功");

            return new ResultValue<Authorization>(data);
        }

        /// <summary>
        /// 整合授权信息
        /// </summary>
        /// <param name="authorization">需要整合的授权列表</param>
        /// <returns>整合后的授权信息对象</returns>
        public ResultValue<Authorization> MergeLoginUserAuthorization(IList<Authorization> authorization)
        {
            this._logger.I("开始授权整合");
            //定义返回变量
            Authorization resultData = new Authorization();

            if (authorization == null)
            {
                this._logger.W("需要整合的授权信息为NULL，无法完成授权");
                return new ResultValue<Authorization>(-1, "需要整合的授权信息为NULL，无法完成授权");
            }

            this._logger.I("开始循环处理每一个授权");
            foreach (var auth in authorization)
            {
                this._logger.D("开始处理模块授权，获取返回结果中还未整合的授权");
                var tempPermissiosn = auth.ModulePermissionAuthorizations.Where(
                    x =>
                    !resultData.ModulePermissionAuthorizations.Any(
                        y => y.PermissionID == x.PermissionID)
                    ).ToArray();
                this._logger.D("分析完毕，发现" + tempPermissiosn.Length + "个新的授权");
                //添加到结果中
                (resultData.ModulePermissionAuthorizations as List<ModulePermissionDetail>).AddRange(tempPermissiosn);

                this._logger.D("开始处理页面元素授权，获取返回结果中还未整合的授权");
                var tempPageElements = auth.PageElementAuthorizations.Where(
                    x =>
                    !resultData.PageElementAuthorizations.Any(
                        y => y.NeedPermissionID == x.NeedPermissionID)
                    ).ToArray();
                this._logger.D("分析完毕，发现" + tempPageElements.Length + "个新的页面元素授权");
                //添加到结果中
                (resultData.PageElementAuthorizations as List<PageElementDetail>).AddRange(tempPageElements);
            }

            //获取所有授权及页面元素信息
            var allModulePermissionResult = this.LoadAllModulePermission();

            if (allModulePermissionResult.RCode != 0)
            {
                this._logger.W("模块权限信息获取失败：" + allModulePermissionResult.RMsg);
                return new ResultValue<Authorization>(-1, "模块权限信息获取失败：" + allModulePermissionResult.RMsg);
            }

            var allPageElementResult = this.LoadAllPageElement();

            if (allPageElementResult.RCode != 0)
            {
                this._logger.W("页面元素信息获取失败：" + allPageElementResult.RMsg);
                return new ResultValue<Authorization>(-1, "页面元素信息获取失败：" + allPageElementResult.RMsg);
            }

            this._logger.I("授权整合完毕，开始分析未授权信息");
            resultData.Non_ModulePermissionAuthorizations = allModulePermissionResult.Data.Where(x => !resultData.ModulePermissionAuthorizations.Any(y => y.PermissionID == x.PermissionID)).ToArray();
            this._logger.I("未授权分析成功");
            resultData.Non_PageElementAuthorizations = allPageElementResult.Data.Where(x => !resultData.PageElementAuthorizations.Any(y => y.NeedPermissionID == x.NeedPermissionID)).ToArray();
            this._logger.I("页面元素未授权分析成功");

            //返回整合结果
            return new ResultValue<Authorization>(resultData);
        }

        /// <summary>
        /// 获取所有的模块授权信息
        /// </summary>
        /// <returns>模块授权信息列表</returns>
        public ResultValue<ModulePermissionDetail[]> LoadAllModulePermission()
        {
            this._logger.I("开始获取全部模块权限");
            this._logger.I("判断缓存中是否有可用数据");
            if (!this._cacheManager.Exists(CACHE_MODULE_PERMISSION_DATA))
            {
                this._logger.I("缓存中没有模块权限数据，开始从数据库获取");
                var allModulePermission = this._permissionDetailRps.Table.ToList();

                this._logger.I("模块权限数据获取成功，开始写入缓存");
                this._cacheManager.Set(CACHE_MODULE_PERMISSION_DATA, allModulePermission);
                this._logger.I("缓存写入完毕");
            }

            //从缓存获取权限数据
            this._logger.I("开始从缓存获取模块权限数据");
            var data = this._cacheManager.Get<ModulePermissionDetail[]>(CACHE_MODULE_PERMISSION_DATA);
            this._logger.I("获取成功");
            return new ResultValue<ModulePermissionDetail[]>(data);
        }

        /// <summary>
        /// 获取所有的页面元素信息
        /// </summary>
        /// <returns>页面元素信息列表</returns>
        private ResultValue<PageElementDetail[]> LoadAllPageElement()
        {
            this._logger.I("开始获取全部页面元素");
            this._logger.I("判断缓存中是否有可用数据");
            if (!this._cacheManager.Exists(CACHE_PAGE_ELEMENT_DATA))
            {
                this._logger.I("缓存中没有页面元素数据，开始从数据库获取");
                var allPageElement = this._pageElementDetailRps.Table.ToList();

                this._logger.I("页面元素数据获取成功，开始写入缓存");
                this._cacheManager.Set(CACHE_PAGE_ELEMENT_DATA, allPageElement);
                this._logger.I("缓存写入完毕");
            }

            this._logger.I("开始从缓存获取页面控件数据");
            var data = this._cacheManager.Get<PageElementDetail[]>(CACHE_PAGE_ELEMENT_DATA);
            this._logger.I("获取成功");
            return new ResultValue<PageElementDetail[]>(data);
        }

        /// <summary>
        /// 分组授权
        /// </summary>
        /// <param name="groupID">需要授权的分组ID</param>
        /// <param name="permissions">权限编号列表</param>
        /// <param name="creator">创建人</param>
        /// <returns>授权是否成功</returns>
        public ResultValue<bool> GroupAuthoriz(string groupID, string[] permissions, User creator)
        {
            this._logger.I("开始进行分组授权");
            if (string.IsNullOrEmpty(groupID))
            {
                this._logger.W("分组编号为空，无法完成授权");
                return new ResultValue<bool>(-1, "分组编号不合法，授权失败");
            }
            else if (permissions == null)
            {
                this._logger.W("权限分组为NULL，默认给空数组，做授权清空处理");
            }

            //获取分组信息
            var getGroupResult = this._groupBll.Get(groupID);
            if (getGroupResult.RCode != 0 || getGroupResult.Data == null)
            {
                this._logger.W("获取分组原始数据失败，分组获取结果：" + getGroupResult);
                return new ResultValue<bool>(-1, "获取分组原始数据失败，无法完成授权");
            }

            var newAuthorizations = new List<TargetAuthonization>();
            if (permissions != null)
            {
                //转换成实体对象
                newAuthorizations = permissions.Select(x => new TargetAuthonization()
                {
                    CreateDate = DateTime.Now,
                    ID = Guid.NewGuid().ToString(),
                    CreatorID = creator.ID,
                    ModulePermissionID = x,
                    TargetID = groupID,
                    TargetType = (int)TargetType.Group
                }).ToList();
            }

            var targetAuthorizations = this._targetAuthonizationRps.Table.Where(x => x.TargetID == groupID && x.TargetType == (int)TargetType.Group).ToList();

            using (TransactionScope tran = new TransactionScope())
            {
                //删除已经存在的授权
                this._targetAuthonizationRps.DeleteRanges(targetAuthorizations);

                //新增授权
                this._targetAuthonizationRps.AddRanges(newAuthorizations);

                tran.Complete();
            }

            //如果数据层处理成功则更新缓存中的分组数据
            if (this._cacheManager.ExistsList<Authorization>(CACHE_GROUP_AUTH_LIST))
            {
                this._logger.I("发现已存在的旧缓存数据，开始清理");
                this._cacheManager.RemoveFromList<Authorization>(CACHE_GROUP_AUTH_LIST, groupID);
            }
            this._logger.I("开始重新载入分组授权");
            this.LoadGroupAuthorization(groupID);
            this._logger.I("分组授权载入成功");

            return new ResultValue<bool>(true);
        }

        /// <summary>
        /// 用户授权
        /// </summary>
        /// <param name="userID">需要授权的用户ID</param>
        /// <param name="permissions">权限编号列表</param>
        /// <param name="creator">操作人</param>
        /// <returns>授权是否成功</returns>
        public ResultValue<bool> UserAuthoriz(string userID, string[] permissions, User creator)
        {
            this._logger.I("开始进行用户授权");
            if (string.IsNullOrEmpty(userID))
            {
                this._logger.W("用户编号为空，无法完成授权");
                return new ResultValue<bool>(-1, "用户编号不合法，授权失败");
            }
            else if (permissions == null)
            {
                this._logger.W("权限分组为NULL，默认给空数组，做授权清空处理");
            }

            //获取分组信息
            var user = this._userRps.Table.Where(x => x.ID == userID && x.Status != -1).FirstOrDefault();
            if (user == null)
            {
                this._logger.W("获取用户原始数据失败");
                return new ResultValue<bool>(-1, "获取用户原始数据失败，无法完成授权");
            }

            var newAuthorizations = new List<TargetAuthonization>();
            if (permissions != null)
            {
                //转换成实体对象
                newAuthorizations = permissions.Select(x => new TargetAuthonization()
                {
                    CreateDate = DateTime.Now,
                    ID = Guid.NewGuid().ToString(),
                    CreatorID = creator.ID,
                    ModulePermissionID = x,
                    TargetID = userID,
                    TargetType = (int)TargetType.User
                }).ToList();
            }

            var targetAuthorizations = this._targetAuthonizationRps.Table.Where(x => x.TargetID == userID && x.TargetType == (int)TargetType.User).ToList();

            using (TransactionScope tran = new TransactionScope())
            {
                //删除已经存在的授权
                this._targetAuthonizationRps.DeleteRanges(targetAuthorizations);

                //新增授权
                this._targetAuthonizationRps.AddRanges(newAuthorizations);

                tran.Complete();
            }

            return new ResultValue<bool>(true);
        }
    }
}
