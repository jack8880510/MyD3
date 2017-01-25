using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyD3.Entity.DBEntity
{
    /// <summary>
    /// 数据字典索引实体
    /// </summary>
    public class DictionaryIndex : MyD3Entity
    {
        /// <summary>
        /// 索引显示名称
        /// </summary>
        [JsonProperty]
        public string Name { get; set; }

        /// <summary>
        /// 索引
        /// </summary>
        [JsonProperty]
        public string Index { get; set; }
    }
}
