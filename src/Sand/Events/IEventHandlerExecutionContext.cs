using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sand.Events
{
    /// <summary>
    /// 事件处理器上下文接口
    /// </summary>
    public interface IEventHandlerExecutionContext
    {
        /// <summary>
        /// 注册事件
        /// </summary>
        /// <typeparam name="TEvent">事件</typeparam>
        /// <typeparam name="THandler">事件处理器</typeparam>
        void RegisterHandler<TEvent, THandler>()
            where TEvent : IEvent
            where THandler : IEventHandler<TEvent>;

        /// <summary>
        /// 注册事件
        /// </summary>
        /// <param name="eventType">事件</param>
        /// <param name="handlerType">事件处理器</param>
        void RegisterHandler(Type eventType, Type handlerType);

        /// <summary>
        /// 注册事件
        /// </summary>
        /// <typeparam name="TEvent">事件</typeparam>
        /// <typeparam name="THandler">事件处理器</typeparam>
        bool HandlerRegistered<TEvent, THandler>()
            where TEvent : IEvent
            where THandler : IEventHandler<TEvent>;
        /// <summary>
        /// 注册事件
        /// </summary>
        /// <param name="eventType">事件类型</param>
        /// <param name="handlerType">事件处理器类型</param>
        bool HandlerRegistered(Type eventType, Type handlerType);
        /// <summary>
        /// 注册事件
        /// </summary>
        /// <param name="event">事件类型</param>
        /// <param name="cancellationToken">事件处理器类型</param>
        Task HandleEventAsync(IEvent @event, CancellationToken cancellationToken = default);
    }
}
