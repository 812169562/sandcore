using AspectCore.DynamicProxy;
using AspectCore.Injector;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using EasyCaching.Core;
using EasyCaching.Core.Interceptor;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using EasyCaching.Core.Configurations;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Sand.Extensions;
using Sand.Helpers;

namespace Sand.Cache
{
    /// <summary>
    /// redis缓存aop
    /// </summary>
    public class RedisCachingInterceptor : AbstractInterceptor
    {
        /// <summary>
        /// Gets or sets the cache provider factory.
        /// </summary>
        /// <value>The cache provider.</value>
        [FromContainer]
        public IEasyCachingProviderFactory CacheProviderFactory { get; set; }

        /// <summary>
        /// Gets or sets the hybrid caching provider.
        /// </summary>
        /// <value>The hybrid caching provider.</value>
        [FromContainer]
        public IHybridCachingProvider HybridCachingProvider { get; set; }

        /// <summary>
        /// Gets or sets the key generator.
        /// </summary>
        /// <value>The key generator.</value>
        [FromContainer]
        public IEasyCachingKeyGenerator KeyGenerator { get; set; }

        /// <summary>
        /// Get or set the options
        /// </summary>
        [FromContainer]
        public IOptions<EasyCachingInterceptorOptions> Options { get; set; }
        /// <summary>
        /// The typeof task result method.
        /// </summary>
        private static readonly ConcurrentDictionary<Type, MethodInfo>
                    TypeofTaskResultMethod = new ConcurrentDictionary<Type, MethodInfo>();

        /// <summary>
        /// The typeof task result method.
        /// </summary>
        private static readonly ConcurrentDictionary<MethodInfo, object[]>
                    MethodAttributes = new ConcurrentDictionary<MethodInfo, object[]>();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public override async Task Invoke(AspectContext context, AspectDelegate next)
        {
            await ProcessRedisCaching(context, next);
        }
        private object[] GetMethodAttributes(MethodInfo mi)
        {
            return MethodAttributes.GetOrAdd(mi, mi.GetCustomAttributes(true));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        private async Task ProcessRedisCaching(AspectContext context, AspectDelegate next)
        {
            var attribute = GetMethodAttributes(context.ServiceMethod).FirstOrDefault(x => x.GetType() == typeof(RedisCachingAttribute)) as RedisCachingAttribute;
            if (attribute == null)
            {
                await context.Invoke(next);
            }
            var returnType = context.IsAsync()
                    ? context.ServiceMethod.ReturnType.GetGenericArguments().First()
                    : context.ServiceMethod.ReturnType;

            var cacheKey = KeyGenerator.GetCacheKey(context.ServiceMethod, context.Parameters, attribute.CacheKeyPrefix);
            if (context.Parameters != null && context.Parameters.Length > 0)
            {
                var md5key = Encrypt.Md5By16(Json.ToJson(context.Parameters));
                cacheKey = cacheKey + ":" + md5key;
            }
            object cacheValue = null;
            var isAvailable = true;
            try
            {
                if (attribute.IsHybridProvider)
                {
                    cacheValue = await HybridCachingProvider.GetAsync(cacheKey, returnType);
                }
                else
                {
                    var _cacheProvider = CacheProviderFactory.GetCachingProvider(attribute.CacheProviderName ?? Options.Value.CacheProviderName);
                    cacheValue = await _cacheProvider.GetAsync(cacheKey, returnType);
                }
            }
            catch (Exception ex)
            {
                if (!attribute.IsHighAvailability)
                {
                    throw;
                }
                else
                {
                    isAvailable = false;
                }
            }

            if (cacheValue != null)
            {
                if (context.IsAsync())
                {
                    //#1
                    //dynamic member = context.ServiceMethod.ReturnType.GetMember("Result")[0];
                    //dynamic temp = System.Convert.ChangeType(cacheValue.Value, member.PropertyType);
                    //context.ReturnValue = System.Convert.ChangeType(Task.FromResult(temp), context.ServiceMethod.ReturnType);
                    //#2                                               
                    context.ReturnValue =
                        TypeofTaskResultMethod.GetOrAdd(returnType, t => typeof(Task).GetMethods().First(p => p.Name == "FromResult" && p.ContainsGenericParameters).MakeGenericMethod(returnType)).Invoke(null, new object[] { cacheValue });
                }
                else
                {
                    //context.ReturnValue = System.Convert.ChangeType(cacheValue.Value, context.ServiceMethod.ReturnType);
                    context.ReturnValue = cacheValue;
                }
                return;
            }
            // Invoke the method if we don't have a cache hit
            await next(context);
            if (isAvailable)
            {
                // get the result
                var returnValue = context.IsAsync()
                    ? await context.UnwrapAsyncReturnValue()
                    : context.ReturnValue;

                // should we do something when method return null?
                // 1. cached a null value for a short time
                // 2. do nothing
                if (returnValue != null && cacheKey.IsNotWhiteSpaceEmpty())
                {
                    if (attribute.IsHybridProvider)
                    {
                        await HybridCachingProvider.SetAsync(cacheKey, returnValue, TimeSpan.FromSeconds(attribute.Expiration));
                    }
                    else
                    {
                        var _cacheProvider = CacheProviderFactory.GetCachingProvider(attribute.CacheProviderName ?? Options.Value.CacheProviderName);
                        await _cacheProvider.SetAsync(cacheKey, returnValue, TimeSpan.FromSeconds(attribute.Expiration));
                    }
                }
            }
        }
    }
}
