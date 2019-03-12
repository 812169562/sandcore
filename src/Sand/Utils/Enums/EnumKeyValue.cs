using System;
using System.Collections.Generic;
using System.Text;

namespace Sand.Utils.Enums
{
    /// <summary>
    /// 枚举集合用户绑定
    /// </summary>
    public class EnumKeyValue
    {
        /// <summary>
        /// 绑定值
        /// </summary>
        public int Value { get; set; }

        /// <summary>
        /// 描述值
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 显示值
        /// </summary>
        public string DisplayName { get; set; }
        /// <summary>
        /// 显示值Label
        /// </summary>
        public string Label { get { return this.DisplayName; } }
    }
}
