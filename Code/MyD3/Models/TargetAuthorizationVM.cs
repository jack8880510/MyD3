using MyD3.Business.Interface.DataModel.User;
using MyD3.Entity.DBView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DemoSite.Models
{
    /// <summary>
    /// 目标授权视图模型
    /// </summary>
    public class TargetAuthorizationVM
    {
        /// <summary>
        /// 系统所有权限
        /// </summary>
        public ModulePermissionDetail[] AllPermissions { get; set; }

        /// <summary>
        /// 目标授权信息
        /// </summary>
        public Authorization TargetAuthorization { get; set; }
    }
}