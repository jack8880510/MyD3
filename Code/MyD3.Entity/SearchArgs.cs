using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyD3.Entity
{
    /// <summary>
    /// 搜索条件
    /// </summary>
    public class SearchArgs
    {
        public SearchArgs()
        {
            this.SearchCondition = new Dictionary<string, string>();
        }

        private int pageSize;

        [JsonProperty(PropertyName = "rowCount")]
        public int PageSize
        {
            get
            {
                if (this.pageSize == -1)
                {
                    return 999999999;
                }
                return this.pageSize;
            }
            set
            {
                this.pageSize = value;
            }
        }

        public int OrigPageSize
        {
            get
            {
                return this.pageSize;
            }
        }

        [JsonProperty(PropertyName = "current")]
        public int PageNumber { get; set; }

        public int PageIndex
        {
            get
            {
                return PageNumber <= 0 ? 0 : PageNumber - 1;
            }
        }

        public string SortName { get; set; }

        public bool SortDESC { get; set; }

        public IDictionary<string, string> SearchCondition { get; set; }

        public string ConditionString { get; set; }
    }
}
