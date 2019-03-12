using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sand.Extensions;

namespace Sand.Domain.Query
{
    /// <summary>
    /// 排序生成器
    /// </summary>
    public class OrderByBuilder
    {
        /// <summary>
        /// 初始化排序生成器
        /// </summary>
        public OrderByBuilder()
        {
            Items = new List<OrderByItem>();
        }

        /// <summary>
        /// 排序项
        /// </summary>
        private List<OrderByItem> Items { get; set; }

        /// <summary>
        /// 生成排序字符串
        /// </summary>
        public string Generate()
        {
            return Items.Select(t => t.Generate()).ToList().Join();
        }

        /// <summary>
        /// 添加排序
        /// </summary>
        /// <param name="name">排序属性</param>
        /// <param name="desc">是否降序</param>
        public void Add(string name, bool desc = false)
        {
            if (desc == false)
            {
                Add(name, OrderDirection.Asc);
                return;
            }
            Add(name, OrderDirection.Desc);
        }

        /// <summary>
        /// 添加排序
        /// </summary>
        /// <param name="name">排序属性</param>
        /// <param name="direction">排序方向</param>
        public void Add(string name, OrderDirection direction)
        {
            if (name.IsEmpty())
                return;
            Items.Add(new OrderByItem(name, direction));
        }
    }
}
