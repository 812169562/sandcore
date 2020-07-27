using System.Threading;
using System.Threading.Tasks;

namespace Sand.Events
{
    /// <summary>
    /// 事件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class EventHandler<T> : IEventHandler<T>
        where T : IEvent
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="event"></param>
        /// <returns></returns>
        public async Task<bool> CanHandle(IEvent @event)
        => await Task.FromResult(typeof(T).Equals(@event.GetType()));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="event"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public abstract Task<bool> HandleAsync(T @event, CancellationToken cancellationToken = default);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="event"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> HandleAsync(IEvent @event, CancellationToken cancellationToken = default) => await CanHandle(@event) ? await HandleAsync((T)@event, cancellationToken) : await Task.FromResult(false);
    }
}
