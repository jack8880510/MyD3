using MyD3.Common.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyD3.Common.Log;
using DS.Business.Tools.Request;
using System.Net;

namespace DS.Business.Tools
{
    public class BattleNetClient : BaseClient
    {
        /// <summary>
        /// 创建客户端对象
        /// </summary>
        /// <param name="logger"></param>
        public BattleNetClient(ILogger logger) : base(logger)
        {
        }

        /// <summary>
        /// 重写API调用，在调用前追加对AccessToken的有效性验证
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request"></param>
        /// <returns></returns>
        public override T InvokeAPI<T>(BaseRequest request)
        {
            try
            {
                return base.InvokeAPI<T>(request);
            }
            catch (WebException wexc1)
            {
                throw wexc1;
            }
        }

        protected override string CreateQueryString(BaseRequest req)
        {
            return RequestHelper.ToUrlParams(req);
        }
    }
}
