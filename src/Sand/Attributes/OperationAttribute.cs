using System;
using System.Collections.Generic;
using System.Text;

namespace Sand.Attributes
{
    /// <summary>
    /// 操作类型特性
    /// </summary>
    public class OperationAttribute : Attribute
    {
        /// <summary>
        /// 操作类型
        /// </summary>
        public OperationType Type { get; set; }
        /// <summary>
        /// 操作类型
        /// </summary>
        /// <param name="type">操作类型</param>
        public OperationAttribute(OperationType type = OperationType.Comm)
        {
            Type = type;
        }
    }
    /// <summary>
    /// 操作类型
    /// </summary>
    public enum OperationType
    {
        /// <summary>
        /// 无任何特殊操作
        /// </summary>
        None,
        /// <summary>
        /// 常用操作
        /// </summary>
        Comm,
        /// <summary>
        /// 树形目录操作
        /// </summary>
        Catalog,
    }
}
