using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sand.Events
{
    /// <summary>
    /// 事件总线
    /// </summary>
    public interface IEventBus : IEventPublisher, IEventSubscriber
    {
        
    }
}
