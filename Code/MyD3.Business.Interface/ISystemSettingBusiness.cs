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
    public interface ISystemSettingBusiness
    {
        /// <summary>
        /// 获取全部的系统设置
        /// </summary>
        /// <returns>系统设置信息</returns>
        ResultValue<SystemSettingDetail[]> AllSystemSettingDetail();

        /// <summary>
        /// 保存系统设置
        /// </summary>
        /// <param name="args">需要保存的系统设置列表</param>
        /// <param name="creator">创建人</param>
        /// <returns>是否保存成功</returns>
        ResultValue<bool> Save(IDictionary<string, string> args, User creator);
    }
}
