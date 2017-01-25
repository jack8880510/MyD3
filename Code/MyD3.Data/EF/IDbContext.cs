using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Common;
using System.Data.Entity.Core.Objects;
using MyD3.Data.Entities;

namespace MyD3.Data.EF
{
    public interface IDbContext
    {
        /// <summary>
        /// 开始批处理操作，优化性能
        /// </summary>
        void BeginBatchProcess();

        /// <summary>
        /// 结束批处理操作
        /// </summary>
        void EndBatchProcess();

        /// <summary>
        /// 相应对象变更
        /// </summary>
        void DetectChanges();

        /// <summary>
        /// 获取连接对象
        /// </summary>
        DbConnection Connection { get; }

        /// <summary>
        /// 开启或关闭延迟加载
        /// </summary>
        bool LazyLoadingEnabled { get; set; }

        /// <summary>
        /// 从数据读取器中读取数据并转换成对象实体
        /// </summary>
        /// <typeparam name="TEntity">对象类型</typeparam>
        /// <param name="reader">读取器</param>
        /// <param name="entitySetName">结果集名称</param>
        /// <returns>结果集</returns>
        IList<TEntity> Translate<TEntity>(DbDataReader reader, string entitySetName);

        /// <summary>
        /// Get DbSet
        /// </summary>
        /// <typeparam name="TEntity">Entity type</typeparam>
        /// <returns>DbSet</returns>
        IDbSet<TEntity> Set<TEntity>() where TEntity : DbEntity;

        /// <summary>
        /// Save changes
        /// </summary>
        /// <returns></returns>
        int SaveChanges();

        /// <summary>
        /// Execute stores procedure and load a list of entities at the end
        /// </summary>
        /// <typeparam name="TEntity">Entity type</typeparam>
        /// <param name="commandText">Command text</param>
        /// <param name="parameters">Parameters</param>
        /// <returns>Entities</returns>
        IList<TEntity> ExecuteStoredProcedureList<TEntity>(string commandText, params object[] parameters)
            where TEntity : DbEntity, new();        

        /// <summary>
        /// Creates a raw SQL query that will return elements of the given generic type.  The type can be any type that has properties that match the names of the columns returned from the query, or can be a simple primitive type. The type does not have to be an entity type. The results of this query are never tracked by the context even if the type of object returned is an entity type.
        /// </summary>
        /// <typeparam name="TElement">The type of object returned by the query.</typeparam>
        /// <param name="sql">The SQL query string.</param>
        /// <param name="parameters">The parameters to apply to the SQL query string.</param>
        /// <returns>Result</returns>
        IEnumerable<TElement> SqlQuery<TElement>(string sql, params object[] parameters);

        /// <summary>
        /// Executes the given DDL/DML command against the database.
        /// </summary>
        /// <param name="sql">The command string</param>
        /// <param name="doNotEnsureTransaction">false - the transaction creation is not ensured; true - the transaction creation is ensured.</param>
        /// <param name="timeout">Timeout value, in seconds. A null value indicates that the default value of the underlying provider will be used</param>
        /// <param name="parameters">The parameters to apply to the command string.</param>
        /// <returns>The result returned by the database after executing the command.</returns>
        int ExecuteSqlCommand(string sql, bool doNotEnsureTransaction = false, int? timeout = null, params object[] parameters);
    }
}
