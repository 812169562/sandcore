using Sand.Context;
using Sand.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sand.Log.Core
{
    /// <summary>
    /// /
    /// </summary>
    public class ExceptionlessLogContext : LogContext
    {
        /// <summary>
        /// 初始化日志上下文
        /// </summary>
        /// <param name="context">上下文</param>
        public ExceptionlessLogContext(IContext context) : base(context)
        {
        }
        /// <summary>
        /// 创建日志上下文信息
        /// </summary>
        protected override LogContextInfo CreateInfo()
        {
            return new LogContextInfo
            {
                TraceId = Guid.NewGuid().ToString(),
                Stopwatch = GetStopwatch(),
                Url = Web.Url,
                Browser = Web.Browser,
                Host = Web.Host
            };
        }
    }
}
