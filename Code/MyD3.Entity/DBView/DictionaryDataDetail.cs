using MyD3.Data.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyD3.Entity.DBView
{
    /// <summary>
    /// 数据字典明细视图
    /// </summary>
    public class DictionaryDataDetail : DbEntity
    {
        /// <summary>
        /// 索引
        /// </summary>
        [JsonProperty]
        public string Index { get; set; }

        /// <summary>
        /// 索引名称
        /// </summary>
        [JsonProperty]
        public string IndexName { get; set; }

        /// <summary>
        /// 数据值
        /// </summary>
        [JsonProperty]
        public string DataValue { get; set; }

        /// <summary>
        /// 数据名称
        /// </summary>
        [JsonProperty]
        public string DataName { get; set; }

        /// <summary>
        /// 是否为默认
        /// </summary>
        [JsonProperty]
        public bool IsDefault { get; set; }
    }
}
