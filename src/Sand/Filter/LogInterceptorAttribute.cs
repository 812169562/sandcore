using System;
using System.Threading.Tasks;
using Sand.Dependency;
using NLog;
using AspectCore.DynamicProxy;
using AspectCore.Injector;
using Sand.Context;
using Microsoft.Extensions.Logging;

namespace Sand.Filter
{
    /// <summary>
    /// 日志aop
    /// </summary>
    public class LogInterceptorAttribute : AbstractInterceptorAttribute
    {
        /// <summary>
        /// 用户上下文
        /// </summary>
        [FromContainer]
        public IUserContext UserContext { get; set; }
        /// <summary>
        /// 日志
        /// </summary>
        private ILog _log { get; set; }
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public async override Task Invoke(AspectContext context, AspectDelegate next)
        {
            var beforeTime = DateTime.UtcNow;
            TimeSpan beforeTs = new TimeSpan(beforeTime.Ticks);
            try
            {
                _log = Log.Log.GetLog("AopDebugLog");
                _log.Debug("before：");
                await next(context);
            }
            catch (Exception ex)
            {
                _log.Error("error：" + beforeTime + "*" + context.ImplementationMethod.Name + "*" + context.ImplementationMethod.Name + "*" + ex.Message);
            }
            finally
            {
                var afterTime = DateTime.UtcNow;
                TimeSpan afterTs = new TimeSpan(afterTime.Ticks);
                _log.Debug("用时：" + (afterTs - beforeTs).Milliseconds + "毫秒");
                _log.Debug("after：" + afterTime + "*" + context.ImplementationMethod.Name + "*" + context.ImplementationMethod.Name);
            }
        }
    }
}
