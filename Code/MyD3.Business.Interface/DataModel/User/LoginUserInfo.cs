using System.Collections.Generic;
using MyD3User = MyD3.Entity.DBEntity.User;
using MyD3Group = MyD3.Entity.DBEntity.Group;
using Newtonsoft.Json;
using System.Linq;
using MyD3.Entity.DBEntity;

namespace MyD3.Business.Interface.DataModel.User
{
    /// <summary>
    /// 登录用户的信息
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class LoginUserInfo
    {
        /// <summary>
        /// 使用用户数据初始化登录用户信息
        /// </summary>
        /// <param name="user">登录用户的信息</param>
        public LoginUserInfo(MyD3User user)
        {
            this.UserData = user;
            this.Groups = new List<LoginUserGroupInfo>();
        }

        /// <summary>
        /// 使用用户与分组数据初始化登录用户信息
        /// </summary>
        /// <param name="user">登录用户的信息</param>
        /// <param name="groups">用户关联的分组</param>
        public LoginUserInfo(MyD3User user, MyD3Group[] groups)
            : this(user)
        {
            if (groups != null)
            {
                this.Groups = groups.Select(x => new LoginUserGroupInfo(x)).ToList();
            }
        }

        /// <summary>
        /// 用户数据
        /// </summary>
        [JsonProperty]
        public MyD3User UserData { get; set; }

        /// <summary>
        /// 用户所在分组的信息
        /// </summary>
        [JsonProperty]
        public IList<LoginUserGroupInfo> Groups { get; set; }

        /// <summary>
        /// 授权信息
        /// </summary>
        [JsonProperty]
        public Authorization Authorization { get; set; }
    }
}
