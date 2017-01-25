using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyD3Group = MyD3.Entity.DBEntity.Group;

namespace MyD3.Business.Interface.DataModel.User
{
    /// <summary>
    /// 登录用户的分组信息
    /// </summary>
    public class LoginUserGroupInfo
    {
        /// <summary>
        /// 使用指定的分组初始化登录用户分组信息对象
        /// </summary>
        /// <param name="group">登录用户所在的分组信息</param>
        public LoginUserGroupInfo(MyD3Group group)
        {
            this.GroupData = group;
        }

        /// <summary>
        /// 分组数据信息
        /// </summary>
        [JsonProperty]
        public MyD3Group GroupData { get; set; }

        /// <summary>
        /// 授权信息
        /// </summary>
        [JsonProperty]
        public Authorization Authorization { get; set; }
    }
}
