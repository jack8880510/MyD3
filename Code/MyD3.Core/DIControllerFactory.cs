using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace MyD3.Core
{
    /// <summary>
    /// 控制器自定义实例工厂
    /// 
    /// 使用Ninject实现DI
    /// </summary>
    public class DIControllerFactory : DefaultControllerFactory
    {
        /// <summary>
        /// 改写控制器实例化过程
        /// </summary>
        /// <param name="requestContext">请求上下文</param>
        /// <param name="controllerType">控制器类型</param>
        /// <returns>控制器实例</returns>
        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            //不要修改！此处实现控制器的DI，控制的构造函数中现在可以添加注入模块中绑定的类型了
            return controllerType == null ? null : InstanceFactory.CurrentFactory.Get<IController>(controllerType);
        }
    }
}
