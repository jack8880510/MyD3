using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyD3.Common.Cache
{
    /// <summary>
    /// 缓存管理类
    /// PS：缓存管理器的实例无法利用DI获取配置实例，这样会导致StackOverflow异常
    /// </summary>
    public interface ICacheManager
    {
        /// <summary>
        /// 获取缓存中的数据
        /// </summary>
        /// <param name="key">缓存中的key</param>
        /// <returns>对应数据</returns>
        string Get(string key);

        /// <summary>
        /// 获取缓存中的数据
        /// </summary>
        /// <param name="key">缓存中的key</param>
        /// <returns>对应数据</returns>
        T Get<T>(string key);

        /// <summary>
        /// 从指定的列表中获取数据
        /// </summary>
        /// <typeparam name="T">数据的类型</typeparam>
        /// <param name="list">列表名称</param>
        /// <param name="key">数据的key</param>
        /// <returns>数据信息</returns>
        T GetFromList<T>(string list, string key);

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <typeparam name="T">列表数据类型</typeparam>
        /// <param name="list">列表名称</param>
        /// <returns>数据列表</returns>
        IList<T> GetList<T>(string list);

        /// <summary>
        /// 向缓存中写入数据
        /// </summary>
        /// <param name="key">数据的Key</param>
        /// <param name="value">数据对象</param>
        bool Set(string key, string value);

        /// <summary>
        /// 向缓存中写入数据
        /// </summary>
        /// <param name="key">数据的Key</param>
        /// <param name="value">数据对象</param>
        /// <param name="timeOut">超时时间，单位分钟</param>
        bool Set<T>(string key, T value, int timeOut);

        /// <summary>
        /// 向缓存中写入数据
        /// </summary>
        /// <param name="key">数据的Key</param>
        /// <param name="value">数据对象</param>
        bool Set<T>(string key, T value);

        /// <summary>
        /// 向缓存中写入数据
        /// </summary>
        /// <param name="key">数据的Key</param>
        /// <param name="value">数据对象</param>
        /// <param name="timeOut">超时时间，单位分钟</param>
        bool Set(string key, string value, int timeOut);

        /// <summary>
        /// 将数据添加到指定的缓存列表
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="list">缓存列表名称</param>
        /// <param name="key">数据的key</param>
        /// <param name="value">数据内容</param>
        /// <returns>是否添加成功</returns>
        bool AddToList<T>(string list, string key, T value);

        /// <summary>
        /// 设置数据库的超时时间
        /// </summary>
        /// <param name="key">数据的Key</param>
        /// <param name="timeOut">超时时间，单位分钟</param>
        bool KeyExpire(string key, int timeOut);

        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="key">需要移除的数据</param>
        bool Remove(string key);

        /// <summary>
        /// 从指定列表中移除缓存
        /// </summary>
        /// <param name="list">列表名称</param>
        /// <param name="key">数据Key</param>
        /// <returns>是否移除成功</returns>
        bool RemoveFromList<T>(string list, string key);

        /// <summary>
        /// 移除全部缓存
        /// </summary>
        /// <returns>是否移除成功</returns>
        bool RemoveAll();

        /// <summary>
        /// 验证指定的Key在缓存中是否存在
        /// </summary>
        /// <param name="key">需要验证的Key</param>
        /// <returns>Key是否存在</returns>
        bool Exists(string key);

        /// <summary>
        /// 判断指定列表中，是否包含该缓存数据
        /// </summary>
        /// <param name="list">列表名称</param>
        /// <param name="key">数据的Key</param>
        /// <returns>Key是否存在</returns>
        bool ExistsFromList<T>(string list, string key);

        /// <summary>
        /// 判断指定列表是否存在
        /// </summary>
        /// <param name="list">列表名称</param>
        /// <returns>List是否存在</returns>
        bool ExistsList<T>(string list);
    }
}
