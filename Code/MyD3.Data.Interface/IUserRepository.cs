using MyD3.Entity;
using MyD3.Entity.DBEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyD3.Data.Interface
{
    public interface IUserRepository
    {
        /// <summary>
        /// 根据登录名称获取用户数据
        /// </summary>
        /// <param name="loginName">登录名</param>
        /// <returns>用户对象</returns>
        ResultValue<User> GetByLoginName(string loginName);

        /// <summary>
        /// 根据主键获取用户对象
        /// </summary>
        /// <param name="id">用户系统编号</param>
        /// <returns>用户对象</returns>
        ResultValue<User> Get(string id);

        /// <summary>
        /// 新增用户信息
        /// </summary>
        /// <param name="model">需要新增的用户对象</param>
        /// <returns>新增后的用户对象</returns>
        ResultValue<User> Create(User model);

        /// <summary>
        /// 更新用户信息
        /// </summary>
        /// <param name="model">需要更新的用户对象</param>
        /// <returns>更新后的用户对象</returns>
        ResultValue<bool> Update(User model);

        /// <summary>
        /// 根据搜索条件，搜索用户信息
        /// </summary>
        /// <param name="args">搜索条件</param>
        /// <returns>搜索结果</returns>
        ResultValue<SearchResult<User>> Search(SearchArgs args);

        /// <summary>
        /// 获取指定分组下的成员信息
        /// </summary>
        /// <param name="groupID">分组ID</param>
        /// <returns>分组成员信息</returns>
        ResultValue<User[]> GetGroupMembers(string groupID);
    }
}
