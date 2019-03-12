using System;
using System.Reflection;

namespace Sand.Finders
{
    /// <summary>
    /// 定义一个查找器
    /// </summary>
    /// <typeparam name="TItem">要查找的项类型</typeparam>
    public interface IFinder<out TItem>
    {
        /// <summary>
        /// 查找指定条件的项
        /// </summary>
        /// <param name="predicate">筛选条件</param>
        /// <param name="fromCache">是否来自缓存</param>
        /// <returns></returns>
        TItem[] Find(Func<TItem, bool> predicate, bool fromCache = false);

        /// <summary>
        /// 查找所有项
        /// </summary>
        /// <param name="fromCache">是否来自缓存</param>
        /// <returns></returns>
        TItem[] FindAll(bool fromCache = false);
    }

    /// <summary>
    /// 定义类型查找行为
    /// </summary>
    public interface ITypeFinder : IFinder<Type>
    {
    }

    /// <summary>
    /// 定义程序集查找器
    /// </summary>
    public interface IAssemblyFinder : IFinder<Assembly>
    { }

    /// <summary>
    /// 定义所有程序集查找器
    /// </summary>
    public interface IAllAssemblyFinder : IAssemblyFinder
    {
    }
}