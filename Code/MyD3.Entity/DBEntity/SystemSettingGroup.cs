using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyD3.Entity.DBEntity
{
    public class SystemSettingGroup : MyD3Entity
    {
        /// <summary>
        /// 名称
        /// </summary>
        [JsonProperty]
        public string Name { get; set; }
    }
}
