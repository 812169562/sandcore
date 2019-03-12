using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sand.Events
{/// <summary>
/// 
/// </summary>
    public interface IEventStore : IDisposable
    {/// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEvent"></typeparam>
    /// <param name="event"></param>
    /// <returns></returns>
        Task SaveEventAsync<TEvent>(TEvent @event)
            where TEvent : IEvent;
    }
}
