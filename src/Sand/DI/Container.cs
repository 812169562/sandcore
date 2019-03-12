using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Autofac;
using Autofac.Core;

namespace Sand.DI
{
    /// <summary>
    /// 初始化Autofac对象容器
    /// </summary>
    public class Container
    {
        /// <summary>
        /// 初始化Autofac对象容器
        /// </summary>
        /// <param name="modules">配置模块</param>
        public Container(params IModule[] modules) : this(null, modules)
        {
        }

        /// <summary>
        /// 初始化Autofac对象容器
        /// </summary>
        /// <param name="action">在注册模块前执行的操作</param>
        /// <param name="modules">配置模块</param>
        public Container(Action<ContainerBuilder> action, params IModule[] modules)
        {
            var builder = CreateBuilder(action, modules);
            _container = builder.Build();
        }

        /// <summary>
        /// 容器
        /// </summary>
        private static IContainer _container;

        /// <summary>
        /// 创建容器生成器
        /// </summary>
        /// <param name="modules">配置模块</param>
        public static ContainerBuilder CreateBuilder(params IModule[] modules)
        {
            return CreateBuilder(null, modules);
        }

        /// <summary>
        /// 创建容器生成器
        /// </summary>
        /// <param name="action">在注册模块前执行的操作</param>
        /// <param name="modules">配置模块</param>
        public static ContainerBuilder CreateBuilder(Action<ContainerBuilder> action, params IModule[] modules)
        {
            var builder = new ContainerBuilder();
            if (action != null)
                action(builder);
            foreach (var module in modules)
                builder.RegisterModule(module);
            return builder;
        }

        /// <summary>
        /// 创建对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        public static T Create<T>()
        {
            return _container.Resolve<T>();
        }

        /// <summary>
        /// 创建对象
        /// </summary>
        /// <param name="type">对象类型</param>
        public static object Create(Type type)
        {
            return _container.Resolve(type);
        }

        /// <summary>
        /// 初始化容器
        /// </summary>
        /// <param name="action">在注册模块前执行的操作</param>
        /// <param name="modules">依赖配置</param>
        public static void Init(Action<ContainerBuilder> action, params IModule[] modules)
        {
            var builder = CreateBuilder(action, modules);
            _container = builder.Build();
        }
        /// <summary>
        /// 是否已经注册
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns></returns>
        public static bool IsRegistered(Type type)
        {
            return _container.IsRegistered(type);
        }
        /// <summary>
        /// 解析接口
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static object Resolve<T>()
        {
            return _container.Resolve<T>();
        }
    }
}
