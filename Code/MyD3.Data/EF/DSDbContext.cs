using MyD3.Data.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using System.Data.Entity.Infrastructure;
using System.Data;
using System.Data.Entity.Core.Objects;

namespace MyD3.Data.EF
{
    public class MyD3DbContext : DbContext, IDbContext
    {
        /// <summary>
        /// 初始化数据上下文
        /// </summary>
        /// <param name="nameOrConnectionString">链接字符串或链接字符串配置的名称</param>
        public MyD3DbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
            this.LazyLoadingEnabled = true;
        }

        /// <summary>
        /// 重写模型创建过程，用于构造模型映射关系
        /// </summary>
        /// <param name="modelBuilder">模型构建器</param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //设定默认数据库架构
            modelBuilder.HasDefaultSchema("public");

            //从当前执行项目中搜索所有包含EntityTypeConfiguration的类型
            var typesToRegister = Assembly.GetExecutingAssembly().GetTypes()
            .Where(type => !String.IsNullOrEmpty(type.Namespace))
            .Where(type => type.BaseType != null && type.BaseType.IsGenericType && type.BaseType.GetGenericTypeDefinition() == typeof(EntityTypeConfiguration<>));

            //将这些类型添加到模型构建器中
            foreach (var type in typesToRegister)
            {
                dynamic configurationInstance = Activator.CreateInstance(type);
                modelBuilder.Configurations.Add(configurationInstance);
            }

            //配置加载完毕后，使用
            base.OnModelCreating(modelBuilder);
        }

        /// <summary>
        /// 附加一个实体到上下文
        /// 如果实体已经在本地上下文中存在，则不从数据库中加载，否则从数据库中加载
        /// </summary>
        /// <typeparam name="TEntity">TEntity</typeparam>
        /// <param name="entity">Entity</param>
        /// <returns>Attached entity</returns>
        protected virtual TEntity AttachEntityToContext<TEntity>(TEntity entity) where TEntity : DbEntity, new()
        {
            //little hack here until Entity Framework really supports stored procedures
            //otherwise, navigation properties of loaded entities are not loaded until an entity is attached to the context
            var alreadyAttached = Set<TEntity>().Local.FirstOrDefault(x => x.ID == entity.ID);
            if (alreadyAttached == null)
            {
                //attach new entity
                Set<TEntity>().Attach(entity);
                return entity;
            }
            else
            {
                //entity is already loaded.
                return alreadyAttached;
            }
        }

        public DbConnection Connection
        {
            get { return this.Database.Connection; }
        }

        public bool LazyLoadingEnabled
        {
            get
            {
                return ((IObjectContextAdapter)this).ObjectContext.ContextOptions.LazyLoadingEnabled;
            }
            set
            {
                ((IObjectContextAdapter)this).ObjectContext.ContextOptions.LazyLoadingEnabled = value;
            }
        }

        public void BeginBatchProcess()
        {
            this.Configuration.AutoDetectChangesEnabled = false;
        }

        public void EndBatchProcess()
        {
            this.Configuration.AutoDetectChangesEnabled = true;
        }

        public void DetectChanges()
        {
            if (!this.Configuration.AutoDetectChangesEnabled)
            {
                this.ChangeTracker.DetectChanges();
            }
        }

        public IList<TEntity> Translate<TEntity>(DbDataReader reader, string entitySetName)
        {
            return ((IObjectContextAdapter)this).ObjectContext.Translate<TEntity>(reader, entitySetName, MergeOption.AppendOnly).ToList();
        }

        public new IDbSet<TEntity> Set<TEntity>() where TEntity : DbEntity
        {
            return base.Set<TEntity>();
        }

        public IList<TEntity> ExecuteStoredProcedureList<TEntity>(string commandText, params object[] parameters) where TEntity : DbEntity, new()
        {
            //HACK: Entity Framework Code First doesn't support doesn't support output parameters
            //That's why we have to manually create command and execute it.
            //just wait until EF Code First starts support them
            //
            //More info: http://weblogs.asp.net/dwahlin/archive/2011/09/23/using-entity-framework-code-first-with-stored-procedures-that-have-output-parameters.aspx

            bool hasOutputParameters = false;
            if (parameters != null)
            {
                foreach (var p in parameters)
                {
                    var outputP = p as DbParameter;
                    if (outputP == null)
                        continue;

                    if (outputP.Direction == ParameterDirection.Output)
                        hasOutputParameters = true;
                }
            }



            var context = ((IObjectContextAdapter)(this)).ObjectContext;

            if (hasOutputParameters)
            {
                //no output parameters
                var result = this.Database.SqlQuery<TEntity>(commandText, parameters).ToList();
                for (int i = 0; i < result.Count; i++)
                    result[i] = AttachEntityToContext(result[i]);

                return result;

                //var result = context.ExecuteStoreQuery<TEntity>(commandText, parameters).ToList();
                //foreach (var entity in result)
                //    Set<TEntity>().Attach(entity);
                //return result;
            }
            else
            {

                //var connection = context.Connection;
                var connection = this.Database.Connection;
                //Don't close the connection after command execution


                //open the connection for use
                if (connection.State == ConnectionState.Closed)
                    connection.Open();
                //create a command object
                using (var cmd = connection.CreateCommand())
                {
                    //command to execute
                    cmd.CommandText = commandText;
                    cmd.CommandType = CommandType.StoredProcedure;

                    // move parameters to command object
                    if (parameters != null)
                        foreach (var p in parameters)
                            cmd.Parameters.Add(p);

                    //database call
                    var reader = cmd.ExecuteReader();
                    //return reader.DataReaderToObjectList<TEntity>();
                    var result = context.Translate<TEntity>(reader).ToList();
                    for (int i = 0; i < result.Count; i++)
                        result[i] = AttachEntityToContext(result[i]);
                    //close up the reader, we're done saving results
                    reader.Close();
                    return result;
                }

            }
        }

        public IEnumerable<TElement> SqlQuery<TElement>(string sql, params object[] parameters)
        {
            return this.Database.SqlQuery<TElement>(sql, parameters);
        }

        public int ExecuteSqlCommand(string sql, bool doNotEnsureTransaction = false, int? timeout = default(int?), params object[] parameters)
        {
            int? previousTimeout = null;
            if (timeout.HasValue)
            {
                //store previous timeout
                previousTimeout = ((IObjectContextAdapter)this).ObjectContext.CommandTimeout;
                ((IObjectContextAdapter)this).ObjectContext.CommandTimeout = timeout;
            }

            var transactionalBehavior = doNotEnsureTransaction
                ? TransactionalBehavior.DoNotEnsureTransaction
                : TransactionalBehavior.EnsureTransaction;
            var result = this.Database.ExecuteSqlCommand(transactionalBehavior, sql, parameters);

            if (timeout.HasValue)
            {
                //Set previous timeout back
                ((IObjectContextAdapter)this).ObjectContext.CommandTimeout = previousTimeout;
            }

            //return result
            return result;
        }
    }
}
