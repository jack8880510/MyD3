using MyD3.Entity;
using MyD3.Entity.DBEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyD3.Data.Interface
{
    /// <summary>
    /// 分组数据仓库接口
    /// </summary>
    public interface IGroupRepository
    {
        /// <summary>
        /// 根据分组名称获取分组数据
        /// </summary>
        /// <param name="name">分组名</param>
        /// <returns>分组对象</returns>
        ResultValue<Group> GetByName(string name);

        /// <summary>
        /// 获取分组信息
        /// </summary>
        /// <param name="id">主键编号</param>
        /// <returns>分组信息对象</returns>
        ResultValue<Group> Get(string id);

        /// <summary>
        /// 获取用户关联的分组
        /// </summary>
        /// <param name="userID">用户编号</param>
        /// <returns>用户关联的分组信息集合</returns>
        ResultValue<Group[]> GetUserGroup(string userID);

        /// <summary>
        /// 新增分组信息
        /// </summary>
        /// <param name="model">需要新增的分组对象</param>
        /// <returns>新增后的分组对象</returns>
        ResultValue<Group> Create(Group model);

        /// <summary>
        /// 更新分组信息
        /// </summary>
        /// <param name="model">需要更新的分组对象</param>
        /// <returns>更新后的分组对象</returns>
        ResultValue<bool> Update(Group model);

        /// <summary>
        /// 根据搜索条件，搜索分组信息
        /// </summary>
        /// <param name="args">搜索条件</param>
        /// <returns>搜索结果</returns>
        ResultValue<SearchResult<Group>> Search(SearchArgs args);

        /// <summary>
        /// 修改分组成员数据
        /// </summary>
        /// <param name="groupID">分组编号</param>
        /// <param name="members">成员信息</param>
        /// <returns>是否处理成功</returns>
        ResultValue<bool> SetGroupMember(string groupID, UserGroup[] members);
    }
}
