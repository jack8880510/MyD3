using MyD3.Business.Interface;
using MyD3.Business.Interface.DataModel.User;
using MyD3.Common;
using MyD3.Common.Log;
using MyD3.Data.Interface;
using MyD3.Entity;
using MyD3.Entity.DBEntity;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System;
using MyD3.Common.Data;

namespace MyD3.BusinessLayer
{
    /// <summary>
    /// 用户业务模型
    /// </summary>
    public class UserBusiness : IUserBusiness
    {
        /* 声明业务层依赖对象 */
        private readonly IGroupBusiness _groupBLL;
        private readonly ISecurityBusiness _securityBLL;
        private readonly ILogger _logger;
        private readonly IRepository<User> _userRps;
        private readonly IRepository<UserGroup> _userGroupRps;

        /// <summary>
        /// 使用依赖的业务层接口初始化用户业务层
        /// </summary>
        /// <param name="groupBLL">分组业务</param>
        /// <param name="securityBLL">安全业务</param>
        /// <param name="logger">日志</param>
        public UserBusiness(IGroupBusiness groupBLL, ISecurityBusiness securityBLL,
            IRepository<UserGroup> userGroupRps,
            ILogger logger, IRepository<User> userRps)
        {
            this._groupBLL = groupBLL;
            this._securityBLL = securityBLL;
            this._logger = logger;
            this._userRps = userRps;
            this._userGroupRps = userGroupRps;
        }

        /// <summary>
        /// 用户登录信息验证
        /// </summary>
        /// <param name="loginArgs">含了调用方传入的用户登录信息，需强转进行使用</param>
        /// <returns>如果用户登录验证成功，那么返回用户详细信息，否则在ResultValue中说明用户验证失败的原因</returns>
        protected ResultValue<User> UserLoginValidate(LoginArgs loginArgs)
        {
            this._logger.I("开始验证用户登录信息，登录参数："
                + loginArgs == null ? "null" : JsonConvert.SerializeObject(loginArgs));

            if (loginArgs == null || string.IsNullOrEmpty(loginArgs.LoginName) || string.IsNullOrEmpty(loginArgs.Password))
            {
                this._logger.D("登录验证失败");
                return new ResultValue<User>(1, "登录信息验证失败，登录信息不完整，用户名与密码不能为空");
            }

            this._logger.I("根据用户名获取用户信息");
            var user = this._userRps.Table.Where(x => x.LoginName == loginArgs.LoginName && x.Status != -1).FirstOrDefault();

            if (user == null)
            {
                this._logger.I("登录信息验证失败");
                return new ResultValue<User>(2, "登录信息验证失败，用户名不存在");
            }

            //验证密码是否一致
            if (MD5Helper.MD5(loginArgs.Password).ToUpper() != user.Password.ToUpper())
            {
                this._logger.I("登录信息验证失败，登录密码错误");
                return new ResultValue<User>(3, "登录信息验证失败，登录密码错误");
            }


            //密码一致则返回用户对象
            this._logger.I("用户登录校验成功");
            return new ResultValue<User>(user);
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="loginArgs">登录参数</param>
        /// <returns>登录结果，Code不为0时则为失败，为0则Data信息中包含登录用户的数据及其授权信息</returns>
        public virtual ResultValue<LoginInfo> Login(LoginArgs loginArgs)
        {
            this._logger.I("开始校验用户登录信息");
            var loginValidateResult = this.UserLoginValidate(loginArgs);

            //如果登录信息返回为空或者返回码不是正常码
            if (loginValidateResult == null || loginValidateResult.RCode != 0)
            {
                this._logger.I("用户登录信息校验失败：" + loginValidateResult == null ? "null" : loginValidateResult.ToString());
                //返回失败信息给调用方
                return new ResultValue<LoginInfo>(loginValidateResult == null ? -1 : loginValidateResult.RCode,
                    loginValidateResult == null ? "内部错误" : loginValidateResult.RMsg);
            }

            this._logger.I("获取分组信息");
            var userGroupsResult = this._groupBLL.GetUserGroup(loginValidateResult.Data.ID);

            //验证获取是否成功
            if (userGroupsResult == null || userGroupsResult.RCode != 0)
            {
                this._logger.I("分组信息获取失败：" + userGroupsResult == null ? "null" : userGroupsResult.ToString());
                //返回失败信息给调用方
                return new ResultValue<LoginInfo>(userGroupsResult == null ? -1 : userGroupsResult.RCode,
                    userGroupsResult == null ? "内部错误" : userGroupsResult.RMsg);
            }

            this._logger.I("获取用户个人授权信息");
            var userAuthorizationResult = this._securityBLL.LoadUserAuthorizations(loginValidateResult.Data.ID);

            //验证授权信息是否获取正确
            if (userAuthorizationResult.RCode != 0)
            {
                this._logger.I("个人授权信息获取失败：" + userAuthorizationResult.ToString());
                //不正确则直接返回
                return new ResultValue<LoginInfo>(userAuthorizationResult.RCode, userAuthorizationResult.RMsg);
            }

            this._logger.I("开始封装返回对象");
            LoginInfo resultData = new LoginInfo(loginValidateResult.Data,
                userGroupsResult == null ? null : userGroupsResult.Data.ToArray());

            this._logger.I("设置用户个人授权");
            resultData.User.Authorization = userAuthorizationResult.Data;

            //如果包含分组，则开始获取分组授权
            if (resultData.User.Groups.Count > 0)
            {
                this._logger.I("包含分组，开始获取分组授权");
                //开始获取分组授权数据
                foreach (var group in resultData.User.Groups)
                {
                    this._logger.I("分组：" + group.GroupData.ID + " " + group.GroupData.Name + " 获取授权中...");
                    //根据分组ID获取分组授权信息
                    var groupAuthorizationResult = this._securityBLL.LoadGroupAuthorization(group.GroupData.ID);

                    if (groupAuthorizationResult.RCode != 0)
                    {
                        this._logger.I("个人授权信息获取失败：" + groupAuthorizationResult.ToString());
                        return new ResultValue<LoginInfo>(groupAuthorizationResult.RCode, groupAuthorizationResult.RMsg);
                    }

                    this._logger.I("设置分组授权信息");
                    group.Authorization = groupAuthorizationResult.Data;
                }
            }

            this._logger.I("分组授权信息获取完成，开始整合授权信息");
            List<Authorization> auths = new List<Authorization>();
            auths.AddRange(resultData.User.Groups.Select(x => x.Authorization).ToArray());
            auths.Add(resultData.User.Authorization);

            this._logger.I("开始整合");
            var mergeResult = this._securityBLL.MergeLoginUserAuthorization(auths);
            if (mergeResult.RCode != 0)
            {
                this._logger.I("授权整合失败：" + mergeResult.ToString());
                return new ResultValue<LoginInfo>(mergeResult.RCode, mergeResult.RMsg);
            }
            this._logger.I("设置整合结果");
            resultData.AllAuthorization = mergeResult.Data;

            this._logger.I("登录完成，返回登录结果");
            return new ResultValue<LoginInfo>(resultData);
        }

        /// <summary>
        /// 根据搜索条件，搜索用户信息
        /// </summary>
        /// <param name="args">搜索条件</param>
        /// <returns>搜索结果</returns>
        public ResultValue<SearchResult<User>> Search(SearchArgs args)
        {
            this._logger.I("开始业务层搜索处理");
            this._logger.D("开始创建queryable对象");
            var query = this._userRps.Table.Where(x => x.Status != -1);

            if (!string.IsNullOrEmpty(args.ConditionString))
            {
                this._logger.D("开始创建name条件");
                query = query.Where(x => x.LoginName.Contains(args.ConditionString) || x.DisplayName.Contains(args.ConditionString));
            }

            this._logger.D("开始数据库查询");
            var data = query.OrderBy(args.SortName, !args.SortDESC)
                .Skip(args.PageSize * args.PageIndex)
                .Take(args.PageSize)
                .ToArray();

            this._logger.D("开始获取结果的总数量");
            var count = query.Count();

            this._logger.D("开始封装成为查询结果对象");
            var result = new SearchResult<User>(args, data, count);

            this._logger.D("返回查询结果");
            return new ResultValue<SearchResult<User>>(result);
        }

        /// <summary>
        /// 保存用户信息，如果用户存在则是编辑，否则是新增
        /// </summary>
        /// <param name="user">需要保存的用户信息</param>
        /// <param name="creator">处理人</param>
        /// <returns>用户信息保存结果</returns>
        public ResultValue<User> Save(User user, User creator)
        {
            this._logger.I("开始用户保存");
            this._logger.D(JsonConvert.SerializeObject(user));

            if (user == null)
            {
                this._logger.W("由于用户对象不存在，保存失败了");
                return new ResultValue<User>(-1, "需要保存的用户对象为空，保存失败");
            }

            if (string.IsNullOrEmpty(user.ID))
            {
                if (string.IsNullOrEmpty(user.LoginName) || user.LoginName.Length < 6 || user.LoginName.Length > 25)
                {
                    this._logger.W("用户登录名称不合法，保存失败：" + user.LoginName);
                    return new ResultValue<User>(-1, "用户登录名称不合法,长度必须在6-25位之间，保存失败");
                }
                else if (string.IsNullOrEmpty(user.Password) || user.Password.Length < 6 || user.Password.Length > 100)
                {
                    this._logger.W("密码不合法，保存失败：" + (user.Password == null ? "0" : user.Password.Length.ToString()));
                    return new ResultValue<User>(-1, "用户密码不合法，长度必须保持在6-100位之间，保存失败");
                }

                this._logger.I("开始判断用户名是否已经存在了");
                var existsUserResult = this.LoginNameValidate(user.LoginName, string.IsNullOrEmpty(user.ID) ? null : user.ID);
                if (existsUserResult.RCode != 0 || !existsUserResult.Data)
                {
                    this._logger.W("用户名唯一性验证失败：" + existsUserResult.ToString());
                    return new ResultValue<User>(-1, "用户名唯一性验证失败，保存失败");
                }
            }

            if (string.IsNullOrEmpty(user.DisplayName) || user.DisplayName.Length < 1 || user.DisplayName.Length > 50)
            {
                this._logger.W("显示名不合法，保存失败：" + (user.DisplayName == null ? "0" : user.DisplayName.Length.ToString()));
                return new ResultValue<User>(-1, "显示名不合法，长度必须保持在1-50位之间，保存失败");
            }

            this._logger.I("验证成功开始最后的数据封装");
            if (string.IsNullOrEmpty(user.ID))
            {
                user.CreateDate = DateTime.Now;
                user.CreatorID = creator.ID;
                user.Password = MD5Helper.MD5(user.Password);
                user.ID = Guid.NewGuid().ToString();
                this._userRps.Insert(user);
            }
            else
            {
                //如果是编辑，则需要从数据库从新读取用户数据
                var getUserResult = this.Get(user.ID);
                if (getUserResult.RCode != 0)
                {
                    this._logger.W("无法从数据获取用户原始数据，更新失败：" + getUserResult.ToString());
                    return new ResultValue<User>(-1, "无法从数据获取用户原始数据，保存失败");
                }

                getUserResult.Data.DisplayName = user.DisplayName;
                getUserResult.Data.ModifyDate = new Nullable<DateTime>(DateTime.Now);
                getUserResult.Data.ModifyUser = creator.ID;
                user = getUserResult.Data;
                this._userRps.Update(user);
            }

            return new ResultValue<User>(user);
        }

        /// <summary>
        /// 验证指定的登录名是否可用于该指定编号的注册
        /// </summary>
        /// <param name="loginName">需要验证的登录名</param>
        /// <param name="id">用户编号</param>
        /// <returns>是否可用</returns>
        public ResultValue<bool> LoginNameValidate(string loginName, string id)
        {
            this._logger.I("开始判断用户名是否已经存在了");
            var existsUser = this._userRps.Table.Where(x => x.LoginName == loginName && x.Status == -1).FirstOrDefault();
            if (existsUser != null && existsUser.ID != id)
            {
                return new ResultValue<bool>(false);
            }
            else
            {
                return new ResultValue<bool>(true);
            }
        }

        /// <summary>
        /// 删除指定的用户信息
        /// </summary>
        /// <param name="user">需要删除的用户对象</param>
        /// <param name="modify_user">操作用户</param>
        /// <returns>是否删除成功</returns>
        public ResultValue<bool> Delete(User user, User modify_user)
        {
            this._logger.I("开始删除用户");
            this._logger.D(JsonConvert.SerializeObject(user));

            if (user == null)
            {
                this._logger.W("由于用户对象不存在，删除失败");
                return new ResultValue<bool>(-1, "由于用户对象不存在，删除失败");
            }
            else if (string.IsNullOrEmpty(user.ID))
            {
                this._logger.W("该用户ID信息有误，删除失败");
                return new ResultValue<bool>(-1, "该用户ID信息有误，删除失败");
            }

            //根据编号获取用户对象
            var getUserResult = this.Get(user.ID);

            if (getUserResult.RCode != 0)
            {
                this._logger.W("根据用户ID获取用户失败，删除失败");
                return new ResultValue<bool>(-1, "获取用户原始信息失败，删除失败");
            }

            //开始封装对象
            user = getUserResult.Data;
            user.ModifyDate = new Nullable<DateTime>(DateTime.Now);
            user.ModifyUser = modify_user.ID;
            user.Status = -1;   //本系统中-1代表软删除

            this._logger.I("更新");
            this._userRps.Update(user);

            return new ResultValue<bool>(true);
        }

        /// <summary>
        /// 根据系统编号获取用户信息对象
        /// </summary>
        /// <param name="id">系统编号</param>
        /// <returns>用户信息对象</returns>
        public ResultValue<User> Get(string id)
        {
            this._logger.I("开始获取用户信息对象");

            if (string.IsNullOrEmpty(id))
            {
                this._logger.W("系统编号为空，无法获取用户信息");
                return new ResultValue<User>(-1, "系统编号为空，无法获取用户信息");
            }

            var data = this._userRps.Table.Where(x => x.ID == id && x.Status != -1).FirstOrDefault();

            return new ResultValue<User>(data);
        }

        /// <summary>
        /// 获取指定分组下的成员列表
        /// </summary>
        /// <param name="groupID">分组编号</param>
        /// <returns>成员信息集合</returns>
        public ResultValue<User[]> GetGroupMembers(string groupID)
        {
            this._logger.I("开始根据分组编号获取旗下成员");

            if (string.IsNullOrEmpty(groupID))
            {
                this._logger.W("分组编号为空，无法获取用户信息");
                return new ResultValue<User[]>(-1, "分组编号为空，无法获取用户信息");
            }

            var getGroupResult = this._groupBLL.Get(groupID);
            if (getGroupResult.RCode != 0 || getGroupResult.Data == null)
            {
                this._logger.W("无法从数据获取分组原始数据，获取成员失败：" + getGroupResult);
                return new ResultValue<User[]>(-1, "无法从数据获取分组原始数据，获取成员失败");
            }

            this._logger.W("开始数据层获取");
            var users = this._userRps.Table.Join(this._userGroupRps.Table, u => u.ID, ug => ug.UserID, (u, ug) => new { User = u, GroupID = ug.GroupID })
                .Where(x => x.GroupID == groupID && x.User.Status != -1).Select(x => x.User).ToArray();

            return new ResultValue<User[]>(users);
        }
    }
}
