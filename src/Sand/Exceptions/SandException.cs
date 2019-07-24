using System;
using System.Collections.Generic;
using System.Linq;

namespace Sand.Exceptions
{
    /// <summary>
    /// 系统异常
    /// </summary>
    public class SandException : Exception
    {
        /// <summary>
        /// 警告编号
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 多条警告信息
        /// </summary>
        public List<string> MutiMessage { get; set; }
        /// <summary>
        /// 警告信息
        /// </summary>
        public string Messages { get; set; }
        /// <summary>
        /// 返回数据
        /// </summary>
        public new object Data { get; set; }
        /// <summary>
        /// SandException
        /// </summary>
        public SandException()
        {
        }
        /// <summary>
        /// SandException
        /// </summary>
        /// <param name="message"></param>
        public SandException(string message) : base(message)
        {
        }
        /// <summary>
        /// SandException
        /// </summary>
        /// <param name="message"></param>
        public SandException(List<string> message)
        {
        }
    }

    /// <summary>
    /// 警告信息
    /// </summary>
    public class Warning : SandException
    {
        /// <summary>
        /// Warning
        /// </summary>
        public Warning()
        {
            Code = "W";
        }
        /// <summary>
        /// Warning
        /// </summary>
        /// <param name="message"></param>
        /// <param name="code"></param>
        /// <param name="ex"></param>
        public Warning(string message, string code = "W", Exception ex = null) : base(message)
        {
            if (!code.Contains("W")) code = "W" + code;
            Code = code;
            Messages = message;
        }
        /// <summary>
        /// Warning
        /// </summary>
        /// <param name="message"></param>
        public Warning(List<string> message)
        {
            MutiMessage = message;
            Messages = MutiMessage.Aggregate((item, current) => item + "," + current);
            if (!message.Any()) Code = string.Empty;
        }
    }


    /// <summary>
    /// 警告信息
    /// </summary>
    public class Transform : SandException
    {
        /// <summary>
        /// Warning
        /// </summary>
        public Transform()
        {
            Code = "T";
        }
        /// <summary>
        /// 警告，页面做跳转或者其他方法
        /// </summary>
        /// <param name="message">错误消息</param>
        /// <param name="data">返回数据</param>
        /// <param name="code">错误编号</param>
        /// <param name="ex">异常信息</param>
        public Transform(string message, object data = null, string code = "T", Exception ex = null) : base(message)
        {
            if (!code.Contains("T")) code = "T" + code;
            Code = code;
            Messages = message;
            Data = data;
        }
        /// <summary>
        /// Warning
        /// </summary>
        /// <param name="message"></param>
        public Transform(List<string> message)
        {
            MutiMessage = message;
            Messages = MutiMessage.Aggregate((item, current) => item + "," + current);
            if (!message.Any()) Code = string.Empty;
        }
    }

    /// <summary>
    /// 错误信息
    /// </summary>
    public class Error : SandException
    {
        /// <summary>
        /// Error日志
        /// </summary>
        public Error()
        {
            Code = "E";
        }
        /// <summary>
        /// Error日志
        /// </summary>
        /// <param name="message"></param>
        /// <param name="code"></param>
        public Error(string message, string code = "E") : base(message)
        {
            if (!code.Contains("E")) code = "E" + code;
            Code = code;
            Messages = message;
        }

        /// <summary>
        /// Error日志
        /// </summary>
        /// <param name="message"></param>
        public Error(List<string> message)
        {
            MutiMessage = message;
            Messages = MutiMessage.Aggregate((item, current) => item + "," + current);
            if (!message.Any())
                Code = string.Empty;
        }

    }
    /// <summary>
    /// 普通信息
    /// </summary>
    public class Info : SandException
    {
        /// <summary>
        /// Info日志
        /// </summary>
        public Info()
        {
            Code = "I";
        }
        /// <summary>
        /// Info日志
        /// </summary>
        /// <param name="message"></param>
        /// <param name="code"></param>
        public Info(string message, string code = "I") : base(message)
        {
            if (!code.Contains("I")) code = "I" + code;
            Code = code;
            Messages = message;
        }
        /// <summary>
        /// Info日志
        /// </summary>
        /// <param name="message"></param>
        public Info(List<string> message)
        {
            MutiMessage = message;
            Messages = MutiMessage.Aggregate((item, current) => item + "," + current);
            if (!message.Any())
                Code = string.Empty;
        }
    }
}
