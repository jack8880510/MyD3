using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyD3.Common.Config;

namespace DS.Business.Tools.Request.D3
{
    public class ProfileRequest : BattleNetRequest
    {
        public ProfileRequest(IConfigManager config, BattleNetLocation location, string battleTag) : base(config, location)
        {
            this.BattleTag = battleTag;
            this.Langurage = "zh_CN";
        }

        /// <summary>
        /// 表示战网昵称
        /// </summary>
        public string BattleTag { get; set; }

        protected override string APIAddress
        {
            get
            {
                return "/d3/profile/" + BattleTag + "/";
            }
        }
    }
}
