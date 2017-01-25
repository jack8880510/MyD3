using Ninject;
using System;
using MyD3.Business;
using MyD3.Business.Interface;
using MyD3.BusinessLayer;
using MyD3.Common.Cache;
using MyD3.Common.Config;
using MyD3.Common.Log;
using MyD3.Data;
using MyD3.Data.Interface;
using Ninject.Activation;
using System.Collections.Generic;
using System.Linq;
using MyD3.Data.EF;
using DS.Business.Tools;

namespace MyD3.Core
{
    /// <summary>
    /// Ninject实例工厂，基于Ninject DI框架
    /// </summary>
    class NinjectInstanceFactory : InstanceFactory
    {
        /// <summary>
        /// 从DI关系中获取指定类型的对象实例
        /// </summary>
        /// <typeparam name="T">需要获取的类型</typeparam>
        /// <returns>类型的实例</returns>
        public override T Get<T>()
        {
            return this.ninjectKernel.Get<T>();
        }

        /// <summary>
        /// 从DI关系中获取指定类型的对象实例
        /// </summary>
        /// <typeparam name="T">返回的类型</typeparam>
        /// <param name="type">需要获取的类型</param>
        /// <returns>类型的实例</returns>
        public override T Get<T>(Type type)
        {
            return (T)this.ninjectKernel.Get(type);
        }

        private IKernel ninjectKernel;

        /// <summary>
        /// 初始化实例工厂
        /// </summary>
        public NinjectInstanceFactory()
        {
            //初始化DI框架
            ninjectKernel = new StandardKernel();

            this.AddBindings();
        }

        /// <summary>
        /// 新增DI类型绑定
        /// </summary>
        private void AddBindings()
        {
            // 业务层绑定
            this.ninjectKernel.Bind<IUserBusiness>().To<UserBusiness>().InThreadScope();
            this.ninjectKernel.Bind<IGroupBusiness>().To<GroupBusiness>().InThreadScope();
            this.ninjectKernel.Bind<ISecurityBusiness>().To<SecurityBusiness>().InThreadScope();
            this.ninjectKernel.Bind<IDictionaryBusiness>().To<DictionaryBusiness>().InThreadScope();
            this.ninjectKernel.Bind<ISystemSettingBusiness>().To<SystemSettingBusiness>().InThreadScope();
            this.ninjectKernel.Bind<IPageBusiness>().To<PageBusiness>().InThreadScope();

            //业务层工具绑定
            this.ninjectKernel.Bind<BattleNetOAuthClient>().ToSelf();

            //数据层绑定
            this.ninjectKernel.Bind<IDbContext>().To<MyD3DbContext>().InThreadScope()
                .WithConstructorArgument("nameOrConnectionString", "main");
            this.ninjectKernel.Bind(typeof(IRepository<>)).To(typeof(EfRepository<>)).InThreadScope();

            //基础库绑定
            this.ninjectKernel.Bind<ILogger>().To<Log4Logger>().InTransientScope();
            this.ninjectKernel.Bind<ICacheManager>().To<RedisCacheManager>().InThreadScope();
            //注意：配置适配器建议为单利模式，因为其中的版本号需要全局维护，用于刷新缓存使用
            //如果使用非单利模式，请使用cacheManager进行维护（推荐）或在全局使用static变量维护配置版本
            this.ninjectKernel.Bind<IMultipleConfigAdapter>().To<DoNetConfigAdapter>().InSingletonScope();
            //此处绑定系统设置业务层为一个适配器，使用了非单利模式，因为业务层使用Cache来维护配置版本号
            this.ninjectKernel.Bind<IMultipleConfigAdapter>().To<SystemSettingBusiness>().InThreadScope();
            //初始化配置管理器时将所有的适配器注入到构造中
            this.ninjectKernel.Bind<IConfigManager>().To<MultipleConfigManager>().InSingletonScope()
                .WithConstructorArgument("adapters", x => x.Kernel.GetAll<IMultipleConfigAdapter>().ToArray());
        }
    }
}
