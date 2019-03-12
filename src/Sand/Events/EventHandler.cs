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
        public bool CanHandle(IEvent @event)
            => typeof(T).Equals(@event.GetType());

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
        public Task<bool> HandleAsync(IEvent @event, CancellationToken cancellationToken = default)
            => CanHandle(@event) ? HandleAsync((T)@event, cancellationToken) : Task.FromResult(false);
    }
}
