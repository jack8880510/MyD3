using MyD3.Entity;
using MyD3.Entity.DBEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyD3.Data.Interface
{
    public interface IPageRepository
    {
        /// <summary>
        /// 从数据库读取所有的页面信息
        /// </summary>
        /// <returns></returns>
        ResultValue<Page[]> All();
    }
}
