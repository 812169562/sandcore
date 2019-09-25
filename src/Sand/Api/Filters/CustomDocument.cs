using System;
using System.Collections.Generic;
using System.Text;

namespace Sand.Api.Filters
{
    /// <summary>
    /// 配置控制器显示
    /// </summary>
    public class CustomDocument
    {
        /// <summary>
        /// 加载的x'm'l文件
        /// </summary>
        public string XmlName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public CustomDocument()
        {
            XmlName = "Sand.Api.xml";
        }
    }
}
