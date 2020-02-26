using System;
using System.Collections.Generic;
using System.Text;

namespace Sand.Attributes
{
    /// <summary>
    /// 编号特性
    /// </summary>
    public class CodeAttribute : Attribute
    {
        ///<summary>
        /// 编号
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 编号特性
        /// </summary>
        /// <param name="code">编号</param>
        /// <param name="name">名称</param>
        public CodeAttribute(string code, string name="")
        {
        }
    }
}
