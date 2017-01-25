using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyD3.Business.Interface.DataModel
{
    [JsonObject]
    public class DictionaryDataStruct
    {
        /// <summary>
        /// 数据系统编号
        /// </summary>
        [JsonProperty]
        public string ID { get; set; }

        /// <summary>
        /// 顺序号
        /// </summary>
        [JsonProperty]
        public int OrderNo { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        [JsonProperty]
        public int Status { get; set; }

        /// <summary>
        /// 数据字典值
        /// </summary>
        [JsonProperty]
        public string Value { get; set; }

        /// <summary>
        /// 数据字典的名称
        /// </summary>
        [JsonProperty]
        public string Name { get; set; }
    }
}
