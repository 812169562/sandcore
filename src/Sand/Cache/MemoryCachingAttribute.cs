using EasyCaching.Core.Interceptor;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sand.Cache
{
    /// <summary>
    /// 内存缓存
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = true)]
    public class MemoryCachingAttribute : EasyCachingInterceptorAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="expiration">缓存时间(默认30s)</param>
        public MemoryCachingAttribute(int expiration = 30)
        {
            Expiration = expiration;
        }
        /// <summary>
        /// 缓存时间(默认30s)
        /// </summary>
        /// <value>缓存时间</value>
        public int Expiration { get; set; } = 30;
    }
}
