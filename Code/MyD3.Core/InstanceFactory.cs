using System;

namespace MyD3.Core
{
    /// <summary>
    /// 实例上下文
    /// </summary>
    public abstract class InstanceFactory
    {
        private static InstanceFactory _factory;

        /// <summary>
        /// 获取实例工厂对象
        /// </summary>
        public static InstanceFactory CurrentFactory
        {
            get
            {
                if (_factory == null)
                {
                    _factory = new NinjectInstanceFactory();
                }
                return _factory;
            }
        }

        /// <summary>
        /// 从DI关系中获取指定类型的对象实例
        /// </summary>
        /// <typeparam name="T">返回的类型</typeparam>
        /// <param name="type">需要获取的类型</param>
        /// <returns>类型的实例</returns>
        public abstract T Get<T>(Type type);

        /// <summary>
        /// 从DI关系中获取指定类型的对象实例
        /// </summary>
        /// <typeparam name="T">需要获取的类型</typeparam>
        /// <returns>类型的实例</returns>
        public abstract T Get<T>();
    }
}
