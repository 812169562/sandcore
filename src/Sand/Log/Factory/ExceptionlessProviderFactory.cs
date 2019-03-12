using Sand.Log.Abstractions;
using Sand.Log.Provider;

namespace Sand.Log.Factory
{
    /// <summary>
    /// 
    /// </summary>
    public class ExceptionlessProviderFactory: ILogProviderFactory
    {
        /// <summary>
        /// 创建日志提供程序
        /// </summary>
        /// <param name="logName">日志名称</param>
        /// <param name="format">日志格式化器</param>
        public ILogProvider Create(string logName, ILogFormat format = null)
        {
            return new ExceptionlessProvider(logName);
        }
    }
}
