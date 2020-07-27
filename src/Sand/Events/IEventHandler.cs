using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sand.Events
{
    /// <summary>
    /// 
    /// </summary>
    public interface IEventHandler
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="event"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> HandleAsync(IEvent @event, CancellationToken cancellationToken = default);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="event"></param>
        /// <returns></returns>
        Task<bool> CanHandle(IEvent @event);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IEventHandler<in T> : IEventHandler
        where T : IEvent
    {/// <summary>
    /// 
    /// </summary>
    /// <param name="event"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
        Task<bool> HandleAsync(T @event, CancellationToken cancellationToken = default);
    }
}
