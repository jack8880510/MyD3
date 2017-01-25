using MyD3.Common.Config;
using MyD3.Common.Log;
using Newtonsoft.Json;
using ServiceStack.Redis;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Configuration;

namespace MyD3.Common.Cache
{
    /// <summary>
    /// Redis缓存管理器
    /// </summary>
    public class RedisCacheManager : ICacheManager, IDisposable
    {
        /// <summary>
        /// 日志处理对象
        /// </summary>
        private readonly ILogger _logger;
        private RedisClient redisClient;

        /// <summary>
        /// 初始化Redis缓存类
        /// </summary>
        /// <param name="logger">日志处理对象</param>
        public RedisCacheManager(ILogger logger)
        {
            this._logger = logger;
        }

        /// <summary>
        /// 打开Redis数据库连接
        /// </summary>
        private void RedisOpen()
        {
            this._logger.I("开始Redis缓存客户端实例化");

            if (this.redisClient != null)
            {
                this._logger.I("Redis缓存客户端已创建，跳过创建步骤");
                return;
            }

            this._logger.I("开始获取Redis缓存数据库地址");
            var address = ConfigurationManager.AppSettings["redisAddress"];

            if (string.IsNullOrEmpty(address))
            {
                this._logger.I("由于Redis缓存数据库地址获取失败，创建失败了：" + address);
                throw new ArgumentException("Redis数据库的链接地址不能为空");
            }
            this._logger.D(address);

            int port = -1;

            this._logger.I("开始获取Redis缓存数据库端口");
            if (!int.TryParse(ConfigurationManager.AppSettings["redisPort"], out port))
            {
                this._logger.I("由于Redis缓存数据库端口获取失败，创建失败了：" + port);
                throw new ArgumentException("Redis数据库的端口不能为空并且必须为一个有效的数字");
            }
            this._logger.D(port.ToString());

            long db = 0;

            this._logger.I("开始获取Redis缓存数据库索引");
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["redisDbId"]) && !long.TryParse(ConfigurationManager.AppSettings["redisDbId"], out db))
            {
                this._logger.I("由于Redis缓存数据库索引获取失败，创建失败了：" + db);
                throw new ArgumentException("Redis数据库ID必须是一个有效的数字");
            }
            else if (db < 0)
            {
                this._logger.I("由于Redis缓存数据库索引获取失败，创建失败了：" + db);
                throw new ArgumentException("Redis数据库ID必须是一个有效的数字");
            }
            this._logger.D(db.ToString());

            try
            {
                this._logger.I("开始实例化");
                this.redisClient = new RedisClient(address, port);
                this._logger.I("开始设置DB");
                this.redisClient.ChangeDb(db);
            }
            catch (Exception ex)
            {
                throw new Exception("RedisClient尝试连接时出现异常，请检查服务端是否开启或客户端配置是否正确", ex);
            }
        }

        /// <summary>
        /// 释放Redis资源
        /// </summary>
        public void Dispose()
        {
            this._logger.I("开始释放资源");
            if (this.redisClient != null)
            {
                //如果Client被创建则销毁
                this.redisClient.Dispose();
                this._logger.I("Redis客户端释放完成");
            }
        }

        /// <summary>
        /// 向Redis写入数据
        /// </summary>
        /// <param name="key">数据的Key</param>
        /// <param name="value">数据内容</param>
        public bool Set(string key, string value)
        {
            //打开数据库连接
            this.RedisOpen();

            //添加到列表
            this._logger.I("开始设置缓存数据");
            this._logger.D(key + "@" + value);
            return this.redisClient.Set(key, value);
        }

        /// <summary>
        /// 向Redis写入数据
        /// </summary>
        /// <param name="key">数据的Key</param>
        /// <param name="value">数据内容</param>
        /// <param name="timeOut">超时时间，单位分钟</param>
        public bool Set(string key, string value, int timeOut)
        {
            //打开数据库连接
            this.RedisOpen();

            //添加到列表
            this._logger.I("开始设置缓存数据");
            this._logger.D(key + "(" + timeOut + "m)@" + value);
            return this.redisClient.Set(key, value, new TimeSpan(0, timeOut, 0));
        }

        /// <summary>
        /// 向Redis写入数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">数据的Key</param>
        /// <param name="value">数据内容</param>
        /// <param name="timeOut">超时时间，单位分钟</param>
        /// <returns></returns>
        public bool Set<T>(string key, T value, int timeOut)
        {
            //打开数据库连接
            this.RedisOpen();

            //添加到列表
            this._logger.I("开始设置缓存数据");
            this._logger.D(key + "(" + timeOut + "m)@" + JsonConvert.SerializeObject(value));
            return this.redisClient.Set<T>(key, value, new TimeSpan(0, timeOut, 0));
        }

        /// <summary>
        /// 向Redis写入数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">数据的Key</param>
        /// <param name="value">数据内容</param>
        /// <returns></returns>
        public bool Set<T>(string key, T value)
        {
            //打开数据库连接
            this.RedisOpen();

            //添加到列表
            this._logger.I("开始设置缓存数据");
            this._logger.D(key + "@" + JsonConvert.SerializeObject(value));
            return this.redisClient.Set<T>(key, value);
        }

        /// <summary>
        /// 将数据添加到指定的缓存列表
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="list">缓存列表名称</param>
        /// <param name="key">数据的key</param>
        /// <param name="value">数据内容</param>
        /// <returns>是否添加成功</returns>
        public bool AddToList<T>(string list, string key, T value)
        {
            //打开数据库连接
            this.RedisOpen();

            //获取TypedClient
            this._logger.I("开始获取TypedClient");
            var typedClient = this.redisClient.As<T>();

            //获取当前列表
            this._logger.I("开始获取HashTable：" + list);
            var data = typedClient.GetHash<string>(list);

            //加入到列表
            this._logger.I("开始设置缓存数据");
            this._logger.D(key + "@" + JsonConvert.SerializeObject(value));
            return typedClient.SetEntryInHash<string>(data, key, value);
        }

        /// <summary>
        /// 设置数据库的超时时间
        /// </summary>
        /// <param name="key">数据的Key</param>
        /// <param name="timeOut">超时时间，单位分钟</param>
        public bool KeyExpire(string key, int timeOut)
        {
            //打开数据库连接
            this.RedisOpen();

            //设置有效期
            this._logger.I("开始设置缓存数据有效期");
            this._logger.D(key + "(" + timeOut + ")");
            return this.redisClient.Expire(key, timeOut * 60);
        }

        /// <summary>
        /// 移除Redis数据
        /// </summary>
        /// <param name="key">数据的Key</param>
        bool ICacheManager.Remove(string key)
        {
            //打开数据库连接
            this.RedisOpen();

            this._logger.I("开始移除缓存数据");
            this._logger.D(key);
            return this.redisClient.Remove(key);
        }

        /// <summary>
        /// 从指定列表中移除缓存
        /// </summary>
        /// <param name="list">列表名称</param>
        /// <param name="key">数据Key</param>
        /// <returns>是否移除成功</returns>
        public bool RemoveFromList<T>(string list, string key)
        {
            //打开数据库连接
            this.RedisOpen();

            //获取TypedClient
            this._logger.I("开始获取TypedClient");
            var typedClient = this.redisClient.As<T>();

            //获取当前列表
            this._logger.I("开始获取HashTable：" + list);
            var data = typedClient.GetHash<string>(list);

            //从列表移除
            this._logger.I("开始从列表移除");
            this._logger.D(key);
            return typedClient.RemoveEntryFromHash<string>(data, key);
        }

        /// <summary>
        /// 获取Redis数据
        /// </summary>
        /// <param name="key">数据的Key</param>
        public string Get(string key)
        {
            //打开数据库连接
            this.RedisOpen();

            this._logger.I("开始获取缓存数据");
            this._logger.D(key);
            return this.redisClient.Get<string>(key);
        }

        /// <summary>
        /// 获取Redis数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">数据的Key</param>
        /// <returns></returns>
        public T Get<T>(string key)
        {
            //打开数据库连接
            this.RedisOpen();

            this._logger.I("开始获取缓存数据");
            this._logger.D(key + "#" + typeof(T).Name);
            return this.redisClient.Get<T>(key);
        }

        /// <summary>
        /// 从指定的列表中获取数据
        /// </summary>
        /// <typeparam name="T">数据的类型</typeparam>
        /// <param name="list">列表名称</param>
        /// <param name="key">数据的key</param>
        /// <returns>数据信息</returns>
        public T GetFromList<T>(string list, string key)
        {
            //打开数据库连接
            this.RedisOpen();

            //获取TypedClient
            this._logger.I("开始获取TypedClient");
            var typedClient = this.redisClient.As<T>();

            //获取Hash表
            this._logger.I("开始获取Hash：" + list);
            var hashTable = typedClient.GetHash<string>(list);

            //加入到列表
            this._logger.I("开始获取缓存数据");
            this._logger.D(key);

            T result = default(T);
            if (hashTable.TryGetValue(key, out result))
            {
                this._logger.I("数据获取成功");
            }
            else
            {
                this._logger.I("数据获取失败，返回类型默认值");
            }
            return result;
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <typeparam name="T">列表数据类型</typeparam>
        /// <param name="list">列表名称</param>
        /// <returns>数据列表</returns>
        public IList<T> GetList<T>(string list)
        {
            //打开数据库连接
            this.RedisOpen();

            //获取TypedClient
            this._logger.I("开始获取TypedClient");
            var typedClient = this.redisClient.As<T>();

            this._logger.I("开始获取Hash并返回所有的Value");
            return typedClient.GetHash<string>(list).Values.ToList();
        }

        /// <summary>
        /// 移除全部缓存
        /// </summary>
        /// <returns>是否移除成功</returns>
        public bool RemoveAll()
        {
            //打开数据库连接
            this.RedisOpen();

            //获取数据库中全部的Key
            var keys = this.redisClient.ScanAllKeys().ToList();
            this._logger.I("获取所有的数据Key开始删除，总数：" + keys.Count);

            //逐一删除
            this._logger.I("开始移除");
            keys.ForEach(x => this.redisClient.Remove(x));
            this._logger.I("移除成功");
            return true;
        }

        /// <summary>
        /// 验证指定的Key在缓存中是否存在
        /// </summary>
        /// <param name="key">需要验证的Key</param>
        /// <returns>Key是否存在</returns>
        public bool Exists(string key)
        {
            this.RedisOpen();

            this._logger.I("开始判断指定的缓存是否存在：" + key);
            return this.redisClient.Exists(key) > 0;
        }

        /// <summary>
        /// 判断指定列表中，是否包含该缓存数据
        /// </summary>
        /// <param name="list">列表名称</param>
        /// <param name="key">数据的Key</param>
        /// <returns>Key是否存在</returns>
        public bool ExistsFromList<T>(string list, string key)
        {
            //打开数据库连接
            this.RedisOpen();

            //获取TypedClient
            this._logger.I("开始获取TypedClient");
            var typedClient = this.redisClient.As<T>();

            this._logger.I("开始获取Hash列表");
            var hashTable = typedClient.GetHash<string>(list);

            this._logger.I("判断列表中是否包含该元素");
            return hashTable.Keys.Any(x => x == key);
        }

        /// <summary>
        /// 判断指定列表是否存在
        /// </summary>
        /// <param name="list">列表名称</param>
        /// <returns>List是否存在</returns>
        public bool ExistsList<T>(string list)
        {
            //打开数据库连接
            this.RedisOpen();

            //获取TypedClient
            this._logger.I("开始获取TypedClient");
            var typedClient = this.redisClient.As<T>();

            this._logger.I("开始获取HashTable并判断是否为空，返回结果");
            return typedClient.GetHash<string>(list) != null;
        }
    }
}
