using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyD3.Common.Network
{
    public abstract class BaseRequest
    {
        /// <summary>
        /// 获取一个值，表示API的地址信息
        /// </summary>
        protected abstract string APIAddress { get; }

        /// <summary>
        /// 获取一个值，表示API的基本地址信息
        /// </summary>
        protected abstract string BaseUrl { get; }

        /// <summary>
        /// 请求的地址
        /// </summary>
        public string Url
        {
            get
            {   //DZDPBaseURL
                string result = this.BaseUrl;

                if (result.LastIndexOf("/") == result.Length - 1)
                {
                    result = result.Substring(0, result.Length - 1);
                }

                return result + this.APIAddress;
            }
        }

        /// <summary>
        /// 表单提交方式
        /// </summary>
        public abstract FormMethods FormMethod
        {
            get;
        }
    }
}
