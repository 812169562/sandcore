using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Sand.DI;
using Sand.Events.Default;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sand.Events
{
    /// <summary>
    /// 事件扩展方法
    /// </summary>
    public static class EventExtension
    {
        /// <summary>
        /// 添加事件组件
        /// </summary>
        /// <param name="services"></param>
        public static void AddDefaultEventBus(this IServiceCollection services)
        {
            var eventHandlerExecutionContext = new EventHandlerExecutionContext(services, sc => sc.BuildServiceProvider());
            services.AddSingleton<IEventHandlerExecutionContext>(eventHandlerExecutionContext);
            services.AddSingleton<IEventBus, PassThroughEventBus>();
        }

        /// <summary>
        /// 添加事件组件
        /// </summary>
        /// <param name="app"></param>
        public static void UseDefaultEventBus(this IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();
            eventBus.SubscribeAll();
        }
    }
}
