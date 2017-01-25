using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyD3.Entity
{
    /// <summary>
    /// 搜索结果
    /// </summary>
    /// <typeparam name="T">结果数据类型</typeparam>
    public class SearchResult<T> : SearchArgs
    {
        /// <summary>
        /// 初始化搜索结果信息
        /// </summary>
        /// <param name="args">搜索条件</param>
        /// <param name="data">搜索结果数据</param>
        /// <param name="totalRecordCount">符合条件的总记录数</param>
        public SearchResult(SearchArgs args, T[] data, int totalRecordCount)
        {
            this.ConditionString = args.ConditionString;
            this.Data = data;
            this.PageNumber = args.PageNumber;
            this.PageSize = args.PageSize;
            this.SearchCondition = args.SearchCondition;
            this.SortDESC = args.SortDESC;
            this.SortName = args.SortName;
            this.TotalRecordCount = totalRecordCount;
        }

        /// <summary>
        /// 搜索结果中的数据
        /// </summary>
        [JsonProperty(PropertyName = "rows")]
        public T[] Data { get; set; }

        /// <summary>
        /// 总记录数
        /// </summary>
        [JsonProperty(PropertyName = "total")]
        public int TotalRecordCount { get; set; }
    }
}
