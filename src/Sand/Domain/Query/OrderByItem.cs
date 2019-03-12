using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sand.Domain.Query
{
    /// <summary>
    /// 排序项
    /// </summary>
    public class OrderByItem
    {
        /// <summary>
        /// 初始化排序项
        /// </summary>
        /// <param name="name">排序属性</param>
        /// <param name="direction">排序方向</param>
        public OrderByItem(string name, OrderDirection direction)
        {
            Name = name;
            Direction = direction;
        }

        /// <summary>
        /// 排序属性
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 排序方向
        /// </summary>
        public OrderDirection Direction { get; private set; }

        /// <summary>
        /// 创建排序字符串
        /// </summary>
        public string Generate()
        {
            if (Direction == OrderDirection.Asc)
                return Name;
            return string.Format("{0} desc", Name);
        }
    }
    /// <summary>
    /// 排序方向
    /// </summary>
    public enum OrderDirection
    {
        /// <summary>
        /// 升序
        /// </summary>
        [Description("升序")]
        Asc,
        /// <summary>
        /// 降序
        /// </summary>
        [Description("降序")]
        Desc
    }
}
