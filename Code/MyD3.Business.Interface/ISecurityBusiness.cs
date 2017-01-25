using MyD3.Business.Interface.DataModel.User;
using MyD3.Entity;
using MyD3.Entity.DBEntity;
using MyD3.Entity.DBView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyD3.Business.Interface
{
    /// <summary>
    /// 安全业务类
    /// </summary>
    public interface ISecurityBusiness
    {
        /// <summary>
        /// 载入分组授权
        /// </summary>
        /// <param name="groupID">需要获取授权的分组ID</param>
        /// <returns>分组授权信息</returns>
        ResultValue<Authorization> LoadGroupAuthorization(string groupID);

        /// <summary>
        /// 载入全部的模块权限明细
        /// </summary>
        /// <returns>模块权限明细</returns>
        ResultValue<ModulePermissionDetail[]> LoadAllModulePermission();

        /// <summary>
        /// 载入用户个人授权
        /// </summary>
        /// <param name="userID">需要获取授权信息的用户ID</param>
        /// <returns>用户个人授权信息</returns>
        ResultValue<Authorization> LoadUserAuthorizations(string userID);

        /// <summary>
        /// 整合授权信息
        /// </summary>
        /// <param name="authorization">需要整合的授权列表</param>
        /// <returns>整合后的授权信息对象</returns>
        ResultValue<Authorization> MergeLoginUserAuthorization(IList<Authorization> authorization);

        /// <summary>
        /// 分组授权
        /// </summary>
        /// <param name="groupID">需要授权的分组ID</param>
        /// <param name="permissions">权限编号列表</param>
        /// <param name="creator">操作人</param>
        /// <returns>授权是否成功</returns>
        ResultValue<bool> GroupAuthoriz(string groupID, string[] permissions, User creator);

        /// <summary>
        /// 用户授权
        /// </summary>
        /// <param name="userID">需要授权的用户ID</param>
        /// <param name="permissions">权限编号列表</param>
        /// <param name="creator">操作人</param>
        /// <returns>授权是否成功</returns>
        ResultValue<bool> UserAuthoriz(string userID, string[] permissions, User creator);
    }
}
