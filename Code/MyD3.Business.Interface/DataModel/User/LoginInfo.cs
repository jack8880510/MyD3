using MyD3User = MyD3.Entity.DBEntity.User;
using MyD3Group = MyD3.Entity.DBEntity.Group;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace MyD3.Business.Interface.DataModel.User
{
    /// <summary>
    /// 登录信息类
    /// </summary>
    /// <typeparam name="T">登录信息中的用户类型</typeparam>
    public class LoginInfo
    {
        /// <summary>
        /// 使用指定的用户初始化登录信息
        /// </summary>
        /// <param name="user">登录的用户</param>
        public LoginInfo(MyD3User user)
        {
            this.User = new LoginUserInfo(user);
        }

        /// <summary>
        /// 使用指定的用户和分组信息初始化登录信息
        /// </summary>
        /// <param name="user">登录的用户</param>
        /// <param name="groups">登录用户所在的分组列表</param>
        public LoginInfo(MyD3User user, MyD3Group[] groups)
        {
            this.User = new LoginUserInfo(user, groups);
        }

        /// <summary>
        /// 用户信息
        /// </summary>
        [JsonProperty]
        public LoginUserInfo User { get; set; }

        /// <summary>
        /// 用户及分组整合后的授权信息
        /// </summary>
        [JsonProperty]
        public Authorization AllAuthorization { get; set; }
    }
}
