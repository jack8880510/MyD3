using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyD3.Common.Log
{
    /// <summary>
    /// 日志处理接口
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// 记录异常日志 级别Dug-4 最高
        /// </summary>
        /// <param name="e">异常信息对象</param>
        void E(Exception e);

        /// <summary>
        /// 记录异常日志 级别Dug-4 最高
        /// </summary>
        /// <param name="msg">日志消息</param>
        /// <param name="e">异常信息对象</param>
        void E(string msg, Exception e);

        /// <summary>
        /// 记录警告日志 级别Dug-3 高于Info 低于Exception
        /// </summary>
        /// <param name="msg">日志消息</param>
        void W(string msg);

        /// <summary>
        /// 记录信息日志 级别Dug-2 高于Deg 低于Warring
        /// </summary>
        /// <param name="msg">日志消息</param>
        void I(string msg);

        /// <summary>
        /// 记录调试日志，级别Dug-1 最低
        /// </summary>
        /// <param name="msg">日志消息</param>
        void D(string msg);
    }
}
