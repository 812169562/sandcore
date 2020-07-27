using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Sand.Events.Default
{
    /// <summary>
    /// 事件处理器上下文
    /// </summary>
    public class EventHandlerExecutionContext : IEventHandlerExecutionContext
    {
        private readonly IServiceCollection registry;
        private readonly Func<IServiceCollection, IServiceProvider> serviceProviderFactory;
        private readonly ConcurrentDictionary<Type, List<Type>> registrations = new ConcurrentDictionary<Type, List<Type>>();

        /// <summary>
        /// 注册事件
        /// </summary>
        /// <param name="registry">事件</param>
        /// <param name="serviceProviderFactory">事件处理器</param>
        public EventHandlerExecutionContext(IServiceCollection registry,
            Func<IServiceCollection, IServiceProvider> serviceProviderFactory = null)
        {
            this.registry = registry;
            this.serviceProviderFactory = serviceProviderFactory ?? (sc => registry.BuildServiceProvider());
        }

        /// <summary>
        /// 异步执行事件
        /// </summary>
        /// <param name="event">事件</param>
        /// <param name="cancellationToken">事件处理器</param>
        public async Task HandleEventAsync(IEvent @event, CancellationToken cancellationToken = default(CancellationToken))
        {
            var eventType = @event.GetType();
            if (this.registrations.TryGetValue(eventType, out List<Type> handlerTypes) &&
                handlerTypes?.Count > 0)
            {
                var serviceProvider = this.serviceProviderFactory(this.registry);
                using (var childScope = serviceProvider.CreateScope())
                {
                    foreach (var handlerType in handlerTypes)
                    {
                        var handler = (IEventHandler)childScope.ServiceProvider.GetService(handlerType);
                        if (await handler.CanHandle(@event))
                        {
                            await handler.HandleAsync(@event, cancellationToken);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <typeparam name="THandler"></typeparam>
        /// <returns></returns>
        public bool HandlerRegistered<TEvent, THandler>()
            where TEvent : IEvent
            where THandler : IEventHandler<TEvent>
            => this.HandlerRegistered(typeof(TEvent), typeof(THandler));
        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="handlerType"></param>
        /// <returns></returns>
        public bool HandlerRegistered(Type eventType, Type handlerType)
        {
            if (this.registrations.TryGetValue(eventType, out List<Type> handlerTypeList))
            {
                return handlerTypeList != null && handlerTypeList.Contains(handlerType);
            }

            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <typeparam name="THandler"></typeparam>
        public void RegisterHandler<TEvent, THandler>()
            where TEvent : IEvent
            where THandler : IEventHandler<TEvent>
            => this.RegisterHandler(typeof(TEvent), typeof(THandler));
        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="handlerType"></param>
        public void RegisterHandler(Type eventType, Type handlerType)
        {
            Utils.ConcurrentDictionarySafeRegister(eventType, handlerType, this.registrations);
            this.registry.AddTransient(handlerType);
        }
    }
}
