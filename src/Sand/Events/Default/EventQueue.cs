using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Sand.Events.Default
{
    /// <summary>
    /// 事件队列
    /// </summary>
    internal sealed class EventQueue
    {
        /// <summary>
        /// 事件队列发布
        /// </summary>
        public event System.EventHandler<EventProcessedEventArgs> EventPushed;

        public EventQueue() { }

        /// <summary>
        /// 发布事件
        /// </summary>
        /// <param name="event"></param>
        public void Push(IEvent @event)
        {
            OnMessagePushed(new EventProcessedEventArgs(@event));
        }
        private void OnMessagePushed(EventProcessedEventArgs e) => this.EventPushed?.Invoke(this, e);
    }
}
