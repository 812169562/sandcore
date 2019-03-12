using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Sand.Extensions
{
    /// <summary>
    /// 条件表达式满足
    /// </summary>
    public static class WhereIfExtension
    {
        /// <summary>
        /// 条件表达式
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="source">源查询对象</param>
        /// <param name="predicate">表达式</param>
        /// <param name="condition">满足条件成立</param>
        /// <returns></returns>
        public static IQueryable<T> WhereIf<T>(this IQueryable<T> source, Expression<Func<T, bool>> predicate,
            bool condition)
        {
            return condition ? source.Where(predicate) : source;
        }


        /// <summary>
        /// 条件表达式
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="source">源查询对象</param>
        /// <param name="predicate">表达式</param>
        /// <param name="condition">满足条件成立</param>
        /// <returns></returns>
        public static IEnumerable<T> WhereIf<T>(this IEnumerable<T> source, Func<T, bool> predicate, bool condition)
        {
            return condition ? source.Where(predicate) : source;
        }


        /// <summary>
        /// 条件表达式
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="source">源查询对象</param>
        /// <param name="predicate">表达式</param>
        /// <param name="condition">满足条件成立</param>
        /// <returns></returns>
        public static IQueryable<T> WhereIf<T>(this IQueryable<T> source, Expression<Func<T, int, bool>> predicate,
            bool condition)
        {
            return condition ? source.Where(predicate) : source;
        }


        /// <summary>
        /// 条件表达式
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="source">源查询对象</param>
        /// <param name="predicate">表达式</param>
        /// <param name="condition">满足条件成立</param>
        /// <returns></returns>
        public static IEnumerable<T> WhereIf<T>(this IEnumerable<T> source, Func<T, int, bool> predicate, bool condition)
        {
            return condition ? source.Where(predicate) : source;
        }

        /// <summary>
        /// 条件表达式
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="first">第一拼接条件</param>
        /// <param name="second">第二拼接条件</param>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> WhereIf<T>(this Expression<Func<T, bool>> first,
            Expression<Func<T, bool>> second, bool condition)
        {
            return condition ? first.Compose(second, Expression.And) : first;
        }

        ///// <summary>
        ///// 条件表达式
        ///// </summary>
        ///// <typeparam name="T">类型</typeparam>
        ///// <param name="source">源查询对象</param>
        ///// <param name="predicate">表达式</param>
        ///// <param name="condition">满足条件成立</param>
        ///// <returns></returns>
        //public static IQueryable<T> Filter<T>(this IQueryable<T> source, Expression<Func<T, bool>> predicate)
        //{
        //    return condition ? source.Where(predicate) : source;
        //}
    }
}
