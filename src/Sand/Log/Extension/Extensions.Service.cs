using System;
using Exceptionless;
using Microsoft.Extensions.DependencyInjection;
using Sand.Log.Abstractions;
using Sand.Log.Core;
using Sand.Log.Factory;
using Autofac;

namespace Sand.Log.Extensions
{
    /// <summary>
    /// 日志扩展
    /// </summary>
    public static partial class Extensions {
        /// <summary>
        /// 注册NLog日志操作
        /// </summary>
        /// <param name="services">服务集合</param>
        public static void AddNLog( this IServiceCollection services ) {
            services.AddScoped<ILogProviderFactory, NLogProviderFactory>();
            services.AddSingleton<ILogFormat, ContentFormat>();
            services.AddScoped<ILogContext, LogContext>();
            services.AddScoped<ILog, Log>();
        }

        /// <summary>
        /// 注册NLog日志操作
        /// </summary>
        /// <param name="services">服务集合</param>
        public static void AddNLog(this ContainerBuilder services)
        {
            services.RegisterType<NLogProviderFactory>().As<ILogProviderFactory>().AsImplementedInterfaces().InstancePerLifetimeScope();
            services.RegisterType<ContentFormat>().As<ILogFormat>().SingleInstance();
            services.RegisterType<LogContext>().As<ILogContext>().AsImplementedInterfaces().InstancePerLifetimeScope();
            services.RegisterType<Log>().As<ILog>().AsImplementedInterfaces().InstancePerLifetimeScope();
        }

        /// <summary>
        /// 注册Exceptionless日志操作
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <param name="configAction">配置操作</param>
        public static void AddExceptionless( this IServiceCollection services, Action<ExceptionlessConfiguration> configAction ) {
            services.AddScoped<ILogProviderFactory, ExceptionlessProviderFactory>();
            services.AddSingleton( typeof( ILogFormat ), t => NullLogFormat.Instance );
            services.AddScoped<ILogContext, ExceptionlessLogContext>();
            services.AddScoped<ILog, Log>();
            configAction?.Invoke( ExceptionlessClient.Default.Configuration );
        }
    }
}
