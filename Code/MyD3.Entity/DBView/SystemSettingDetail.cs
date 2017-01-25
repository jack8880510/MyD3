using MyD3.Entity.DBEntity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyD3.Entity.DBView
{
    /// <summary>
    /// 系统设置视图
    /// </summary>
    public class SystemSettingDetail : MyD3Entity
    {
        /// <summary>
        /// 名称
        /// </summary>
        [JsonProperty]
        public string Name { get; set; }

        /// <summary>
        /// 显示名
        /// </summary>
        [JsonProperty]
        public string DisplayName { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        [JsonProperty]
        public string Value { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [JsonProperty]
        public string Description { get; set; }

        /// <summary>
        /// 分组编号
        /// </summary>
        [JsonProperty]
        public string GroupID { get; set; }

        /// <summary>
        /// 分组名称
        /// </summary>
        [JsonProperty]
        public string GroupName { get; set; }

        /// <summary>
        /// 分组排序号
        /// </summary>
        [JsonProperty]
        public int GroupOrderNo { get; set; }

        /// <summary>
        /// 该设置值得类型
        /// 当前提供以下类型的设置项
        /// 1.TEXT提供一个文本框用于输入内容
        /// 2.BOOL提供一个CheckBox用于输入
        /// 3.DATE提供一个DatePick用于输入日期
        /// 4.TIME提供一个TimePick用于输入时间
        /// 5.DATETIME提供一个DateTime用于输入日期时间
        /// 6.OPTION提供一个ComboBox下拉菜单用于给用户选择
        /// </summary>
        [JsonProperty]
        public string ValueType { get; set; }

        /// <summary>
        /// 如果value_type为OPTION，则表示选项的类型
        /// 0.来自后面OptionText与OptionValue的选项组合
        /// 1.来自与后面OptionValue字段匹配的数据字典中的选项
        /// </summary>
        [JsonProperty]
        public int OptionType { get; set; }

        /// <summary>
        /// 如果Option_type为0时表示与Option_Text对应的数据值（使用英文逗号分隔），如果为1时代表数据字典中的索引
        /// </summary>
        [JsonProperty]
        public string OptionValue { get; set; }

        /// <summary>
        /// 如果option_type为0时表示与option_value对应的文本值，使用英文逗号分隔
        /// </summary>
        [JsonProperty]
        public string OptionText { get; set; }

        /// <summary>
        /// 该配置是否为必填的
        /// </summary>
        [JsonProperty]
        public bool Required { get; set; }

        /// <summary>
        /// 当TEXT时表示字符串最少数量，
        /// 当OPTION时表示最少选择数量，
        /// 当DATE时表示日期最早选择的日期，YYYY-MM-dd
        /// 当TIME时表示时间最早选择的时间：HH:mm:ss
        /// 当DATETIME时表示日期和时间允许的最小值：YYYY-MM-dd HH:mm:ss
        /// 当BOOL时不起作用
        /// 本值为0或者null时则不起作用不做验证
        /// </summary>
        [JsonProperty]
        public string MinLength { get; set; }

        /// <summary>
        /// 对应MinLength的最大值校验
        /// </summary>
        [JsonProperty]
        public string MaxLength { get; set; }

        /// <summary>
        /// 正则表达式验证规则
        /// </summary>
        [JsonProperty]
        public string TextReg { get; set; }
    }
}
