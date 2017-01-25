using MyD3.Entity;
using MyD3.Entity.DBEntity;
using MyD3.Entity.DBView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyD3.Data.Interface
{
    public interface ISystemSettingRepository
    {
        /// <summary>
        /// 从数据库获取全部的系统设置数据
        /// </summary>
        /// <returns>系统设置信息集合</returns>
        ResultValue<SystemSettingDetail[]> All();

        /// <summary>
        /// 根据指定的名称获取配置信息
        /// </summary>
        /// <param name="name">配置名称</param>
        /// <returns>配置信息</returns>
        ResultValue<SystemSetting> Get(string name);

        /// <summary>
        /// 将系统配置保存到数据库中
        /// </summary>
        /// <param name="data">需要保存的数据</param>
        /// <returns>是否保存成功</returns>
        ResultValue<bool> Save(IList<SystemSetting> data);
    }
}
