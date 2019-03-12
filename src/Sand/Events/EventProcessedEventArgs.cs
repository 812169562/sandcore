using System;
using System.Collections.Generic;
using System.Text;

namespace Sand.Events
{
    /// <summary>
    /// 
    /// </summary>
    public class EventProcessedEventArgs : EventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="event"></param>
        public EventProcessedEventArgs(IEvent @event)
        {
            this.Event = @event;
        }
        /// <summary>
        /// 
        /// </summary>
        public IEvent Event { get; }

    }
}
