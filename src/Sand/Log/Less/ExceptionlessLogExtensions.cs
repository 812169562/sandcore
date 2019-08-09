﻿using Exceptionless;
using Exceptionless.Extensions;
using Exceptionless.NLog;
using Sand.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sand.Log.Less
{
    /// <summary>
    /// Exceptionless扩展方法
    /// </summary>
    public static class ExceptionlessLogExtensions
    {
        /// <summary>
        /// 提交到中心端
        /// </summary>
        /// <param name="exception">异常</param>
        /// <param name="tag">tag</param>
        public static void Submit(this Exception exception, string tag = null)
        {
            if (exception == null)
                return;
            exception.ToExceptionless().AddTags(tag ?? exception.GetType().ToString()).Submit();
        }
        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="client">客户端</param>
        /// <param name="log">日志内容</param>
        public static void Log(this ExceptionlessClient client, ExceptionlessLog log)
        {
            var eventBuilder = client.CreateLog(log.Message).AddTags(log.Tag);
            if (log.Data != null)
            {
                foreach (var item in log.Data)
                {
                    eventBuilder.AddObject(item);
                }
            }
            eventBuilder.SetProperty(log.PropertyName, log.Property);
            eventBuilder.SetVersion(Uuid.Next());
            eventBuilder.SetUserIdentity(log.UserId);
            eventBuilder.Submit();
        }
    }
}