using Microsoft.Extensions.Logging;
using Sand.Attributes;
using Sand.Finders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sand.Events.Default
{
    /// <summary>
    /// 内存事件总线
    /// </summary>
    public sealed class PassThroughEventBus : IEventBus
    {
        private readonly EventQueue eventQueue = new EventQueue();
        private readonly ILogger logger;
        private readonly IAllAssemblyFinder _allAssemblyFinder;
        private readonly IEventHandlerExecutionContext context;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="allAssemblyFinder"></param>
        /// <param name="logger"></param>
        public PassThroughEventBus(IEventHandlerExecutionContext context, IAllAssemblyFinder allAssemblyFinder,
            ILogger<PassThroughEventBus> logger)
        {
            this.context = context;
            this.logger = logger;
            this._allAssemblyFinder = allAssemblyFinder;
            logger.LogInformation($"PassThroughEventBus构造函数调用完成。Hash Code：{this.GetHashCode()}.");

            eventQueue.EventPushed += EventQueue_EventPushed;
        }

        private async void EventQueue_EventPushed(object sender, EventProcessedEventArgs e)
            => await this.context.HandleEventAsync(e.Event);

        /// <summary>
        /// 发布事件
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <param name="event"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
            where TEvent : IEvent
                => Task.Factory.StartNew(() => eventQueue.Push(@event));

        /// <summary>
        /// 订阅事件
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <typeparam name="TEventHandler"></typeparam>
        public void Subscribe<TEvent, TEventHandler>()
            where TEvent : IEvent
            where TEventHandler : IEventHandler<TEvent>
        {
            if (!this.context.HandlerRegistered<TEvent, TEventHandler>())
            {
                this.context.RegisterHandler<TEvent, TEventHandler>();
            }
        }

        /// <summary>
        /// 订阅事件
        /// </summary>
        public void SubscribeAll()
        {
            var typeEvent = typeof(EventAttribute);
            var typeHandler = typeof(EventHandlerAttribute);
            var assembly = _allAssemblyFinder.FindAll();
            var eventTypes = assembly.Where(t => t.DefinedTypes.Any(p => p.CustomAttributes.Any(m => m.AttributeType == typeEvent)));
            var handlerTypes = assembly.Where(t => t.DefinedTypes.Any(p => p.CustomAttributes.Any(m => m.AttributeType == typeHandler)));
            var handlers = new List<TypeInfo>();
            var events = new List<TypeInfo>();
            foreach (var item in eventTypes)
            {
                events.AddRange(item.DefinedTypes.Where(t => t.CustomAttributes.Any(p => p.AttributeType == typeEvent)));
            }
            foreach (var item in eventTypes)
            {
                handlers.AddRange(item.DefinedTypes.Where(t => t.CustomAttributes.Any(p => p.AttributeType == typeHandler)));
            }
            foreach (var e in events)
            {
                foreach (var handler in handlers)
                {
                    if (e==null||events==null)
                    {
                        continue;
                    }
                    if (handler.ImplementedInterfaces.Any(t => t.ToString() == $"Sand.Events.IEventHandler`1[{e.ToString()}]"))
                    {
                        if (!this.context.HandlerRegistered(e, handler))
                        {
                            this.context.RegisterHandler(e, handler);
                        }
                    }
                }
            }
        }
        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls
        void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    this.eventQueue.EventPushed -= EventQueue_EventPushed;
                    logger.LogInformation($"PassThroughEventBus已经被Dispose。Hash Code:{this.GetHashCode()}.");
                }

                disposedValue = true;
            }
        }
        /// <summary>
        /// 释放
        /// </summary>
        public void Dispose() => Dispose(true);

        #endregion
    }
}
