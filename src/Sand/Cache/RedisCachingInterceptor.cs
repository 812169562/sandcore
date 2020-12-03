using AspectCore.DynamicProxy;
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
using Sand.Log;
using Sand.Log.Less;
using Newtonsoft.Json;
using Sand.Context;
using Microsoft.Extensions.DependencyInjection;
using Sand.Exceptions;
using AspectCore.DependencyInjection;
using AspectCore.Extensions.AspectScope;

namespace Sand.Cache
{
    /// <summary>
    /// redis缓存aop
    /// </summary>
    public class RedisCachingInterceptor : ScopeInterceptorAttribute
    {
        public override Scope Scope { get; set; } = Scope.Nested;

        /// <summary>
        /// Gets or sets the cache provider factory.
        /// </summary>
        /// <value>The cache provider.</value>
        [FromServiceContext]
        public IEasyCachingProviderFactory CacheProviderFactory { get; set; }

        /// <summary>
        /// Gets or sets the hybrid caching provider.
        /// </summary>
        /// <value>The hybrid caching provider.</value>
        [FromServiceContext]
        public IHybridCachingProvider HybridCachingProvider { get; set; }

        /// <summary>
        /// Gets or sets the key generator.
        /// </summary>
        /// <value>The key generator.</value>
        [FromServiceContext]
        public IEasyCachingKeyGenerator KeyGenerator { get; set; }

        /// <summary>
        /// Gets or sets the key generator.
        /// </summary>
        /// <value>The key generator.</value>
        [FromServiceContext]
        public ILog Log { get; set; }

        /// <summary>
        /// Get or set the options
        /// </summary>
        [FromServiceContext]
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
            var cacheKey = "";
            cacheKey = KeyGenerator.GetCacheKey(context.ServiceMethod, context.Parameters, attribute.CacheKeyPrefix);
            if (context.Parameters != null && context.Parameters.Length > 0)
            {
                var md5key = Encrypt.Md5By32(Json.ToJson(context.Parameters));
                cacheKey = cacheKey + ":" + md5key;
            }
            else
            {
                var md5key = Encrypt.Md5By32(Json.ToJson(context.ServiceMethod.Name));
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
                    //cacheValue = await _cacheProvider.GetAsync(cacheKey, returnType);
                    var val = await _cacheProvider.GetAsync<string>(cacheKey);
                    if (val.HasValue)
                    {
                        cacheValue = JsonConvert.DeserializeObject(val.Value, returnType);
                    }
                }
            }
            catch (Exception ex)
            {
                if (!attribute.IsHighAvailability)
                {
                    throw ex;
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
            try
            {
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
                            //var f = Newtonsoft.Json.JsonConvert.SerializeObject(returnValue);
                            await _cacheProvider.SetAsync(cacheKey, JsonConvert.SerializeObject(returnValue,new JsonSerializerSettings(){ NullValueHandling = NullValueHandling.Ignore,DateTimeZoneHandling = DateTimeZoneHandling.Local }), TimeSpan.FromSeconds(attribute.Expiration));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Submit();
            }
        }
    }
}
