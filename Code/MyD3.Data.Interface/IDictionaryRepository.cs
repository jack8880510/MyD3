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
    /// <summary>
    /// 数据字典数据仓库
    /// </summary>
    public interface IDictionaryRepository
    {
        /// <summary>
        /// 从数据库获取所有数据字典索引信息
        /// </summary>
        /// <returns>数据索引信息</returns>
        ResultValue<DictionaryIndex[]> AllIndex();

        /// <summary>
        /// 从数据库获取所有数据字典数据信息
        /// </summary>
        /// <returns>数据详情信息</returns>
        ResultValue<DictionaryData[]> AllData();

        /// <summary>
        /// 根据ID获取数据字典索引
        /// </summary>
        /// <param name="id">索引系统编号</param>
        /// <returns>字典索引信息</returns>
        ResultValue<DictionaryIndex> GetIndex(string id);

        /// <summary>
        /// 根据索引编号，获取索引下的全部数据
        /// </summary>
        /// <param name="indexID">索引编号</param>
        /// <returns>索引下的数据</returns>
        ResultValue<DictionaryData[]> GetData(string indexID);

        /// <summary>
        /// 更新字典索引信息
        /// </summary>
        /// <param name="model">索引对象</param>
        /// <returns>是否更新成功</returns>
        ResultValue<bool> UpdateIndexStatus(DictionaryIndex model);

        /// <summary>
        /// 保存完整字典的信息
        /// </summary>
        /// <param name="index">需要保存的字典索引</param>
        /// <param name="data">需要保存的字典数据对象集合</param>
        /// <returns>是否保存成功</returns>
        ResultValue<bool> SaveDictionary(DictionaryIndex index, DictionaryData[] data);
    }
}
