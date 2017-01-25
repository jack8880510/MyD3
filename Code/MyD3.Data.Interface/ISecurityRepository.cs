using MyD3.Data.Interface.DataModel;
using MyD3.Entity;
using MyD3.Entity.DBEntity;
using MyD3.Entity.DBView;

namespace MyD3.Data.Interface
{
    /// <summary>
    /// 安全信息数据仓库
    /// </summary>
    public interface ISecurityRepository
    {
        /// <summary>
        /// 获取所有的模块权限
        /// </summary>
        /// <returns>模块权限信息表</returns>
        ResultValue<ModulePermissionDetail[]> GetAllModulePermission();

        /// <summary>
        /// 获取所有的页面受控元素
        /// </summary>
        /// <returns>页面受控元素列表</returns>
        ResultValue<PageElementDetail[]> GetAllPageElementDetail();

        /// <summary>
        /// 获取指定对象的模块授权
        /// </summary>
        /// <param name="targetID">对象编号</param>
        /// <param name="targetType">对象类型</param>
        /// <returns>模块授权信息</returns>
        ResultValue<ModulePermissionDetail[]> GetModuleAuthorization(string targetID, TargetType targetType);

        /// <summary>
        /// 获取指定对象的页面元素授权
        /// </summary>
        /// <param name="targetID">对象ID</param>
        /// <param name="tragetType">对象类型</param>
        /// <returns>页面授权信息</returns>
        ResultValue<PageElementDetail[]> GetPageElementAuthorization(string targetID, TargetType targetType);

        /// <summary>
        /// 重写目标授权数据
        /// </summary>
        /// <param name="targetID">目标编号</param>
        /// <param name="targetType">目标类型</param>
        /// <param name="newAuthorization">新的目标授权列表</param>
        /// <returns></returns>
        ResultValue<bool> RewriteTargetAuthorization(string targetID, TargetType targetType, TargetAuthonization[] newAuthorization);
    }
}
