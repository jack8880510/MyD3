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
    /// 页面数据业务层
    /// </summary>
    public interface IPageBusiness
    {
        /// <summary>
        /// 获取所有的页面信息
        /// </summary>
        /// <returns></returns>
        ResultValue<Page[]> All();

        /// <summary>
        /// 根据页面路由信息获取页面信息对象
        /// </summary>
        /// <param name="area">区域</param>
        /// <param name="controller">控制器</param>
        /// <param name="action">动作</param>
        /// <returns>页面信息对象</returns>
        ResultValue<Page> Get(string area, string controller, string action);
    }
}
