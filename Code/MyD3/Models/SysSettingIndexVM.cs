using MyD3.Entity.DBEntity;
using MyD3.Entity.DBView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DemoSite.Models
{
    public class SysSettingIndexVM
    {
        /// <summary>
        /// 系统设置分组信息
        /// </summary>
        public SysSettingIndexGroupVM[] SettingGroups { get; set; }
    }

    public class SysSettingIndexGroupVM
    {
        public string GroupName { get; set; }

        public string GroupID { get; set; }

        public int OrderNo { get; set; }

        /// <summary>
        /// 系统设置
        /// </summary>
        public SystemSettingDetail[] Settings { get; set; }
    }
}