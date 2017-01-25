using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyD3.Common.Config;

namespace DS.Business.Tools.Request
{
    /// <summary>
    /// 获取用户账号信息请求
    /// </summary>
    public class AccountUserRequest : BattleNetOAuthRequest
    {
        public AccountUserRequest(IConfigManager config, BattleNetLocation location) : base(config, location)
        {
        }

        protected override string APIAddress
        {
            get
            {
                return "/account/user";
            }
        }
    }
}
