using System;
using System.Collections.Generic;
using System.Text;

namespace Sand.Events
{/// <summary>
/// 订阅事件
/// </summary>
    public interface IEventSubscriber : IDisposable
    {/// <summary>
     /// 
     /// </summary>
     /// <typeparam name="TEvent"></typeparam>
     /// <typeparam name="TEventHandler"></typeparam>
        void Subscribe<TEvent, TEventHandler>()
            where TEvent : IEvent
            where TEventHandler : IEventHandler<TEvent>;
        /// <summary>
        /// 
        /// </summary>
        void SubscribeAll();
    }
}
