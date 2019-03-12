using System;
using NLog;
using Microsoft.Extensions.Logging;
using Sand.Log.Abstractions;
using Sand.Log.Core;
using Sand.Context;
using Sand.Log.Provider;
using Sand.Log.Factory;
using Sand.DI;
using Autofac;

namespace Sand.Log
{
    /// <summary>
    /// 日志实现
    /// </summary>
    public class Log : LogBase<LogContent>
    {
        /// <summary>
        /// 类名
        /// </summary>
        private readonly string _class;

        /// <summary>
        /// 初始化日志操作
        /// </summary>
        /// <param name="providerFactory">日志提供程序工厂</param>
        /// <param name="context">日志上下文</param>
        /// <param name="format">日志格式器</param>
        /// <param name="session">用户会话</param>
        public Log(ILogProviderFactory providerFactory, ILogContext context, ILogFormat format, IUserContext session) : base(providerFactory.Create("", format), context, session)
        {
        }

        /// <summary>
        /// 初始化日志操作
        /// </summary>
        /// <param name="provider">日志提供程序</param>
        /// <param name="context">日志上下文</param>
        /// <param name="session">用户会话</param>
        /// <param name="class">类名</param>
        private Log(ILogProvider provider, ILogContext context, IUserContext session, string @class) : base(provider, context, session)
        {
            _class = @class;
        }

        /// <summary>
        /// 获取日志内容
        /// </summary>
        protected override LogContent GetContent()
        {
            return new LogContent { Class = _class };
        }

        /// <summary>
        /// 初始化
        /// </summary>
        protected override void Init(LogContent content)
        {
            base.Init(content);
            content.Tenant = Session.TenantId;
            content.Application = "";
            content.Operator = Session.LoginName;
            content.Role = "";
        }

        /// <summary>
        /// 获取日志操作实例
        /// </summary>
        public static ILog GetLog()
        {
            return GetLog(string.Empty);
        }

        /// <summary>
        /// 获取日志操作实例
        /// </summary>
        /// <param name="instance">实例</param>
        public static ILog GetLog(object instance)
        {
            if (instance == null)
                return GetLog();
            var className = instance.GetType().ToString();
            return GetLog(className, className);
        }

        /// <summary>
        /// 获取日志操作实例
        /// </summary>
        /// <param name="logName">日志名称</param>
        public static ILog GetLog(string logName)
        {
            return GetLog(logName, string.Empty);
        }

        /// <summary>
        /// 获取日志操作实例
        /// </summary>
        private static ILog GetLog(string logName, string @class)
        {
            var providerFactory = GetLogProviderFactory();
            var format = GetLogFormat();
            var context = GetLogContext();
            var session = GetSession();
            return new Log(providerFactory.Create(logName, format), context, session, @class);
        }

        /// <summary>
        /// 获取日志提供程序工厂
        /// </summary>
        private static ILogProviderFactory GetLogProviderFactory()
        {
            try
            {
                return DefaultIocConfig.Container.Resolve<ILogProviderFactory>();
            }
            catch
            {
                return new NLogProviderFactory();
            }
        }

        /// <summary>
        /// 获取日志格式器
        /// </summary>
        private static ILogFormat GetLogFormat()
        {
            try
            {
                return DefaultIocConfig.Container.Resolve<ILogFormat>();
            }
            catch
            {
                return ContentFormat.Instance;
            }
        }

        /// <summary>
        /// 获取日志上下文
        /// </summary>
        private static ILogContext GetLogContext()
        {
            try
            {
                return DefaultIocConfig.Container.Resolve<ILogContext>();
            }
            catch
            {
                return NullLogContext.Instance;
            }
        }

        /// <summary>
        /// 获取用户会话
        /// </summary>
        private static IUserContext GetSession()
        {
            try
            {
                return DefaultIocConfig.Container.Resolve<IUserContext>();
            }
            catch
            {
                return new TestUserContext();
            }
        }

        /// <summary>
        /// 空日志操作
        /// </summary>
        public static readonly ILog Null = NullLog.Instance;
    }
}
