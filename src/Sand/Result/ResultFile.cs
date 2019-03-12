using System;
using System.Collections.Generic;
using System.Text;

namespace Sand.Result
{
    /// <summary>
    /// 文件返回类型
    /// </summary>
    public class ResultFile
    {
        /// <summary>
        /// 标识唯一号
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// 文件名称
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 文件大小
        /// </summary>
        public string FileSize { get; set; }
        /// <summary>
        /// 文件的索引
        /// </summary>
        public string Index { get; set; }
        /// <summary>
        /// 文件路径包含File名称
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// 扩展名称
        /// </summary>
        public string Extension { get; set; }
    }
}
