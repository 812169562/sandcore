using System;
using System.Collections.Generic;
using System.Text;

namespace Sand.Log.Abstractions
{
    /// <summary>
    /// 日志格式化接口
    /// </summary>
    public interface ILogFormat
    {
        /// <summary>
        /// 格式化
        /// </summary>
        /// <param name="content">日志内容</param>
        string Format(ILogContent content);
    }
}
