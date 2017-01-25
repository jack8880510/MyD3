using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyD3.Entity.DBEntity
{
    /// <summary>
    /// 数据字典数据
    /// </summary>
    public class DictionaryData : MyD3Entity
    {
        [JsonProperty]
        public string IndexID { get; set; }

        [JsonProperty]
        public string Name { get; set; }

        [JsonProperty]
        public string Value { get; set; }

        [JsonProperty]
        public bool IsDefault { get; set; }
    }
}
