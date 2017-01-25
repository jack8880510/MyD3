using MyD3.Business.Interface.DataModel;
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
    /// 数据字典业务模型
    /// </summary>
    public interface IDictionaryBusiness
    {
        /// <summary>
        /// 根据索引编号获取数据字典结构化数据对象
        /// </summary>
        /// <param name="indexID">索引编号</param>
        /// <returns>结构化数据对象</returns>
        ResultValue<DictionaryStruct> GetStruct(string indexID);

        /// <summary>
        /// 根据搜索条件，搜索索引信息
        /// </summary>
        /// <param name="args">搜索条件</param>
        /// <returns>搜索结果</returns>
        ResultValue<SearchResult<DictionaryStruct>> Search(SearchArgs args);

        /// <summary>
        /// 禁用指定的索引
        /// </summary>
        /// <param name="id">需要禁用的索引ID</param>
        /// <param name="modify_user">修改人</param>
        /// <returns>是否禁用成功</returns>
        ResultValue<bool> Disable(string id, User modify_user);

        /// <summary>
        /// 启用指定的索引
        /// </summary>
        /// <param name="id">需要启用的索引ID</param>
        /// <param name="modify_user">修改人</param>
        /// <returns>是否启用成功</returns>
        ResultValue<bool> Enable(string id, User modify_user);

        /// <summary>
        /// 新增或修改字典数据
        /// </summary>
        /// <param name="data">需要处理的数据</param>
        /// <param name="creator">修改人</param>
        /// <returns>编辑后的数据</returns>
        ResultValue<DictionaryStruct> Save(DictionaryStruct data, User creator);
    }
}
