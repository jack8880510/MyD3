using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyD3.Entity.DBEntity
{
    /// <summary>
    /// 分组
    /// </summary>
    public class Group : MyD3Entity
    {
        /// <summary>
        /// 分组名称
        /// </summary>
        [JsonProperty]
        public virtual string Name { get; set; }
    }
}
