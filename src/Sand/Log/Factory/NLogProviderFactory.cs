using Sand.Log.Abstractions;
using Sand.Log.Provider;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sand.Log.Factory
{  /// <summary>
   /// NLog日志提供程序工厂
   /// </summary>
    public class NLogProviderFactory : ILogProviderFactory
    {
        /// <summary>
        /// 创建日志提供程序
        /// </summary>
        /// <param name="logName">日志名称</param>
        /// <param name="format">日志格式化器</param>
        public ILogProvider Create(string logName, ILogFormat format = null)
        {
            return new NLogProvider(logName, format);
        }
    }
}
