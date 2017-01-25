using MyD3.Business.Interface.DataModel.User;
using MyD3.Entity;
using MyD3.Entity.DBEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyD3.Business.Interface
{
    /// <summary>
    /// 分组业务模型接口
    /// </summary>
    public interface IGroupBusiness
    {
        /// <summary>
        /// 获取分组信息
        /// </summary>
        /// <param name="id">分组编号</param>
        /// <returns>分组信息</returns>
        ResultValue<Group> Get(string id);

        /// <summary>
        /// 获取用户关联的分组
        /// </summary>
        /// <param name="userID">用户编号</param>
        /// <returns>分组信息集合</returns>
        ResultValue<Group[]> GetUserGroup(string userID);

        /// <summary>
        /// 保存分组信息，如果分组存在则是编辑，否则是新增
        /// </summary>
        /// <param name="group">需要保存的分组信息</param>
        /// <param name="creator">创建人</param>
        /// <returns>分组信息保存结果</returns>
        ResultValue<Group> Save(Group group, User creator);

        /// <summary>
        /// 根据搜索条件，搜索分组信息
        /// </summary>
        /// <param name="args">搜索条件</param>
        /// <returns>搜索结果</returns>
        ResultValue<SearchResult<Group>> Search(SearchArgs args);

        /// <summary>
        /// 删除指定的分组信息
        /// </summary>
        /// <param name="group">需要删除的分组对象</param>
        /// <param name="modify_user">创建人</param>
        /// <returns>是否删除成功</returns>
        ResultValue<bool> Delete(Group group, User modify_user);

        /// <summary>
        /// 设置分组成员
        /// </summary>
        /// <param name="groupID">分组编号</param>
        /// <param name="memberIDList">成员ID列表</param>
        /// <param name="creator">操作员</param>
        /// <returns>是否设置成功</returns>
        ResultValue<bool> SetGroupMember(string groupID, string[] memberIDList, User creator);
    }
}
