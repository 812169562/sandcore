using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sand.Result
{
    /// <summary>
    /// 分页
    /// </summary>
    /// <typeparam name="T">分页类型</typeparam>
    public class Paged<T>
    {
        /// <summary>
        /// 初始化
        /// </summary>
        public Paged()
        {
            Result = new List<T>();
        }
        /// <summary>
        /// 查询参数
        /// </summary>
        public object Param { get; set; }
        /// <summary>
        /// 起始页
        /// </summary>
        public int PageIndex { get; set; }
        /// <summary>
        /// 每页大小
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// 总条数
        /// </summary>
        public int TotalCount { get; set; }
        /// <summary>
        /// 总页数
        /// </summary>
        public int PageCount { get; set; }
        /// <summary>
        /// 分页结果
        /// </summary>
        public List<T> Result { get; set; }
    }
}
