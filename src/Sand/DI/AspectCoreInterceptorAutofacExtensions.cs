using AspectCore.Configuration;
using AspectCore.Extensions.Autofac;
using Autofac;
using EasyCaching.Core;
using EasyCaching.Core.Configurations;
using EasyCaching.Core.Interceptor;
using EasyCaching.Interceptor.AspectCore;
using Microsoft.Extensions.Options;
using Sand.Cache;
using Sand.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Sand.DI
{
    /// <summary>
    /// Autofac注入
    /// </summary>
    public static class AspectCoreInterceptorAutofacExtensions
    {
        /// <summary>
        /// Add the AspectCore interceptor.
        /// </summary>
        public static void AddAspectCoreInterceptor(this ContainerBuilder builder, Action<EasyCachingInterceptorOptions> action)
        {
            builder.RegisterType<DefaultEasyCachingKeyGenerator>().As<IEasyCachingKeyGenerator>();

            builder.RegisterType<EasyCachingInterceptor>();
            var config = new EasyCachingInterceptorOptions();

            action(config);

            var options = Options.Create(config);

            builder.Register(x => options);

            builder.RegisterDynamicProxy(configure =>
            {
                bool all(MethodInfo x) => x.CustomAttributes.Any(data => typeof(EasyCachingAbleAttribute).GetTypeInfo().IsAssignableFrom(data.AttributeType));
                configure.Interceptors.AddTyped<EasyCachingInterceptor>(all);
            });
        }

        /// <summary>
        /// 添加redis缓存拦截器
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="action">如果配置多个，注意传入action=>CacheProviderName</param>
        public static void AddRedisAspectCoreInterceptor(this ContainerBuilder builder, Action<EasyCachingInterceptorOptions> action = null)
        {
            if (action == null)
            {
                action = new Action<EasyCachingInterceptorOptions>((EasyCachingInterceptorOptions) => { });
            }
            builder.RegisterType<DefaultEasyCachingKeyGenerator>().As<IEasyCachingKeyGenerator>();

            builder.RegisterType<RedisCachingInterceptor>();

            var config = new EasyCachingInterceptorOptions();

            if (config.CacheProviderName.IsEmpty())
            {
                config.CacheProviderName = EasyCachingConstValue.DefaultCSRedisName;
            }
            action(config);

            var options = Options.Create(config);

            builder.Register(x => options);

            builder.RegisterDynamicProxy(configure =>
            {
                bool allredis(MethodInfo x) => x.CustomAttributes.Any(data => typeof(RedisCachingAttribute).GetTypeInfo().IsAssignableFrom(data.AttributeType));

                configure.Interceptors.AddTyped<RedisCachingInterceptor>(allredis);
            });
        }
    }
}
