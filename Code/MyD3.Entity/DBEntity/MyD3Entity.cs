using MyD3.Data.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyD3.Entity.DBEntity
{
    /// <summary>
    /// 公用实体类
    /// </summary>
    public class MyD3Entity : DbEntity
    {
        /// <summary>
        /// 排序号
        /// </summary>
        [JsonProperty]
        public int OrderNo { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        [JsonProperty]
        public int Status { get; set; }

        /// <summary>
        /// 数据创建时间
        /// </summary>
        [JsonProperty]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 数据创建者ID
        /// </summary>
        [JsonProperty]
        public string CreatorID { get; set; }

        /// <summary>
        /// 最后创建时间
        /// </summary>
        [JsonProperty]
        public DateTime? ModifyDate { get; set; }

        /// <summary>
        /// 最后创建者ID
        /// </summary>
        [JsonProperty]
        public string ModifyUser { get; set; }
    }
}
