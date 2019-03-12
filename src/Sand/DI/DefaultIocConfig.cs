using Autofac;
using Microsoft.Extensions.DependencyModel;
using Sand.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;

namespace Sand.DI
{
    /// <summary>
    /// autofac注入模块（扫描程序集）
    /// </summary>
    public class DefaultIocConfig : IocConfig
    {
        /// <summary>
        /// 加载IOC
        /// </summary>
        /// <param name="builder"></param>
        protected override void Load(ContainerBuilder builder)
        {
            var assemblies = new List<Assembly>();
            var dependencyContext = DependencyContext.Default;
            var libs = dependencyContext.CompileLibraries.Where(lib => !lib.Serviceable && lib.Type != "package");
            foreach (var lib in libs)
            {
                var assembly = AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(lib.Name));
                assemblies.Add(assembly);
            }
            var typeBase = typeof(IDependency);
            builder.RegisterAssemblyTypes(assemblies.ToArray())
                .Where(t => typeBase.IsAssignableFrom(t) && t != typeBase && !t.GetTypeInfo().IsAbstract)
                .AsImplementedInterfaces().InstancePerLifetimeScope();

            var typeBaseSingleton = typeof(IDependencySingleton);
            builder.RegisterAssemblyTypes(assemblies.ToArray())
                .Where(t => typeBaseSingleton.IsAssignableFrom(t) && t != typeBase && !t.GetTypeInfo().IsAbstract)
                .AsImplementedInterfaces().SingleInstance();
        }
        /// <summary>
        /// 容器构建
        /// </summary>
        public static ContainerBuilder ContainerBuilder { get; set; }
        /// <summary>
        /// 容器
        /// </summary>
        public static Autofac.IContainer Container { get; set; }
        /// <summary>
        /// 构造方法
        /// </summary>
        static DefaultIocConfig()
        {
            ContainerBuilder = new ContainerBuilder();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public static class Ioc
    {
        /// <summary>
        /// 服务定位
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <returns></returns>
        public static TService GetService<TService>()
        {
            return DefaultIocConfig.Container.Resolve<TService>();
        }
    }
}
