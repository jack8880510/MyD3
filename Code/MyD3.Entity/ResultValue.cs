namespace MyD3.Entity
{
    /// <summary>
    /// 用于在View层 Business层 Data层 以及 JS页面间传递数据使用数据载体
    /// </summary>
    /// <typeparam name="T">需要传递的数据类型</typeparam>
    public class ResultValue<T>
    {
        /// <summary>
        /// 初始化返回数据
        /// </summary>
        public ResultValue() { }

        /// <summary>
        /// 使用指定的数据初始化返回数据载体
        /// </summary>
        /// <param name="code">本次操作的返回编号</param>
        public ResultValue(int code)
        {
            this.RCode = code;
        }

        /// <summary>
        /// 使用指定的数据初始化返回数据载体
        /// </summary>
        /// <param name="data">本次操作返回的数据内容</param>
        public ResultValue(T data)
        {
            this.Data = data;
        }

        /// <summary>
        /// 使用指定的数据初始化返回数据载体
        /// </summary>
        /// <param name="code">本次操作的返回编号</param>
        /// <param name="msg">本次操作返回的消息</param>
        public ResultValue(int code, string msg) : this(code)
        {
            this.RMsg = msg;
        }

        /// <summary>
        /// 使用指定的数据初始化返回数据载体
        /// </summary>
        /// <param name="code">本次操作的返回编号</param>
        /// <param name="msg">本次操作返回的消息</param>
        /// <param name="data">本次操作返回的数据内容</param>
        public ResultValue(int code, string msg, T data) : this(code, msg)
        {
            this.Data = data;
        }

        /// <summary>
        /// 获取或设置一个值，表示本次方法调用返回的编号
        /// </summary>
        public int RCode { get; set; }

        /// <summary>
        /// 获取或设置一个值，表示本次方法调用返回的消息信息
        /// </summary>
        public string RMsg { get; set; }

        /// <summary>
        /// 获取或设置一个值，表示本次方法调用返回的数据
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// 重写对象描述方法
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string format = "RCode:{0} RMsg: {1}";

            return string.Format(format, this.RCode, this.RMsg);
        }
    }
}
