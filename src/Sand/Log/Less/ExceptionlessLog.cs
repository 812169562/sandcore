using System;
using System.Collections.Generic;
using System.Text;

namespace Sand.Log.Less
{
    /// <summary>
    /// 日志异常信息
    /// </summary>
    public class ExceptionlessLog
    {
        /// <summary>
        /// 用户编号
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 消息内容
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 消息平台
        /// </summary>
        public string Tag { get; set; }
        /// <summary>
        /// 属性
        /// </summary>
        public string Property { get; set; }
        /// <summary>
        /// 属性名称
        /// </summary>
        public string PropertyName { get; set; }
        /// <summary>
        /// 日志数据
        /// </summary>
        public dynamic Data { get; set; }
    }
}
