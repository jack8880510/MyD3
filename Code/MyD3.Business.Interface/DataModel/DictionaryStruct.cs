using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyD3.Business.Interface.DataModel
{
    /// <summary>
    /// 数据字典结构
    /// </summary>
    [JsonObject]
    public class DictionaryStruct
    {
        /// <summary>
        /// 初始化数据字典结构
        /// </summary>
        public DictionaryStruct()
        {

        }

        /// <summary>
        /// 索引系统编号
        /// </summary>
        [JsonProperty]
        public string ID { get; set; }

        /// <summary>
        /// 索引
        /// </summary>
        [JsonProperty]
        public string Index { get; set; }

        /// <summary>
        /// 索引显示名
        /// </summary>
        [JsonProperty]
        public string Name { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        [JsonProperty]
        public int Status { get; set; }

        /// <summary>
        /// 数据字典数据集合
        /// </summary>
        [JsonProperty]
        public IList<DictionaryDataStruct> Data { get; set; }
    }
}
