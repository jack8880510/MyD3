using log4net;
using log4net.Config;
using System;
using System.Diagnostics;
using System.Reflection;

namespace MyD3.Common.Log
{
    /// <summary>
    /// 日志处理类（Log4net版本）
    /// </summary>
    public class Log4Logger : ILogger
    {
        public const string LOG_MSG_FORMAT = "{0}\t{1}";

        /// <summary>
        /// log4net核心组件
        /// </summary>
        private ILog log;

        /// <summary>
        /// 初始化Log4net组件
        /// </summary>
        public Log4Logger()
        {
            //初始化配置
            XmlConfigurator.Configure();
        }

        /// <summary>
        /// 记录调试日志，级别Dug-1 最低
        /// </summary>
        /// <param name="msg">日志消息</param>
        public void D(string msg)
        {
            //在堆栈中获取调用当前日志方法的方法
            var invokeMethod = new StackTrace(true).GetFrame(1).GetMethod();

            //如果尚未初始化日志引擎
            if (this.log == null)
            {
                //则使用调用者的类型进行初始化
                this.log = LogManager.GetLogger(invokeMethod.DeclaringType);
            }

            //写入日志
            this.log.Debug(string.Format(LOG_MSG_FORMAT, invokeMethod.Name, msg));
        }

        /// <summary>
        /// 记录异常日志 级别Dug-4 最高
        /// </summary>
        /// <param name="e">异常信息对象</param>
        public void E(Exception e)
        {
            //在堆栈中获取调用当前日志方法的方法
            var invokeMethod = new StackTrace(true).GetFrame(1).GetMethod();

            //如果尚未初始化日志引擎
            if (this.log == null)
            {
                //则使用调用者的类型进行初始化
                this.log = LogManager.GetLogger(invokeMethod.DeclaringType);
            }

            //写入日志
            this.log.Error(string.Format(LOG_MSG_FORMAT, invokeMethod.Name, e.StackTrace.ToString()), e);
        }

        /// <summary>
        /// 记录异常日志 级别Dug-4 最高
        /// </summary>
        /// <param name="msg">日志消息</param>
        /// <param name="e">异常信息对象</param>
        public void E(string msg, Exception e)
        {
            //在堆栈中获取调用当前日志方法的方法
            var invokeMethod = new StackTrace(true).GetFrame(1).GetMethod();

            //如果尚未初始化日志引擎
            if (this.log == null)
            {
                //则使用调用者的类型进行初始化
                this.log = LogManager.GetLogger(invokeMethod.DeclaringType);
            }

            //写入日志
            this.log.Error(string.Format(LOG_MSG_FORMAT, invokeMethod.Name, msg), e);
        }

        /// <summary>
        /// 记录信息日志 级别Dug-2 高于Deg 低于Warring
        /// </summary>
        /// <param name="msg">日志消息</param>
        public void I(string msg)
        {
            //在堆栈中获取调用当前日志方法的方法
            var invokeMethod = new StackTrace(true).GetFrame(1).GetMethod();

            //如果尚未初始化日志引擎
            if (this.log == null)
            {
                //则使用调用者的类型进行初始化
                this.log = LogManager.GetLogger(invokeMethod.DeclaringType);
            }

            //写入日志
            this.log.Info(string.Format(LOG_MSG_FORMAT, invokeMethod.Name, msg));
        }

        /// <summary>
        /// 记录警告日志 级别Dug-3 高于Info 低于Exception
        /// </summary>
        /// <param name="msg">日志消息</param>
        public void W(string msg)
        {
            //在堆栈中获取调用当前日志方法的方法
            var invokeMethod = new StackTrace(true).GetFrame(1).GetMethod();

            //如果尚未初始化日志引擎
            if (this.log == null)
            {
                //则使用调用者的类型进行初始化
                this.log = LogManager.GetLogger(invokeMethod.DeclaringType);
            }

            //写入日志
            this.log.Warn(string.Format(LOG_MSG_FORMAT, invokeMethod.Name, msg));
        }
    }
}
