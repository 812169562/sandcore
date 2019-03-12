using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Sand.Extensions
{
    /// <summary>
    /// 过滤条件表达式
    /// </summary>
    public static class FilterExtension
    {
        /// <summary>
        /// 判断条件不成立（null 或者没有此种数据）
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="source">源查询对象</param>
        /// <param name="predicate">表达式</param>
        /// <returns></returns>
        public static bool None<T>(this IQueryable<T> source, Expression<Func<T, bool>> predicate = null)
        {
            if (source == null)
                return false;
            if (predicate == null)
                return !source.Any();
            return !source.Any(predicate);
        }

        /// <summary>
        /// 过滤表达式
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="source">源查询对象</param>
        /// <param name="predicate">表达式</param>
        /// <param name="condition">满足条件成立</param>
        /// <returns></returns>
        public static IQueryable<T> Filter<T>(this IQueryable<T> source, Expression<Func<T, bool>> predicate,
            object condition)
        {
            if (condition == null)
                return source;
            if (condition is string)
                if (string.IsNullOrEmpty(condition.ToString()))
                    return source;
            return source.Where(predicate);
        }
        /// <summary>
        /// 条件表达式
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="first">第一拼接条件</param>
        /// <param name="second">第二拼接条件</param>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> Filter<T>(this Expression<Func<T, bool>> first,
            Expression<Func<T, bool>> second, object condition)
        {
            if (condition == null)
                return first;
            if (condition is string)
                if (string.IsNullOrEmpty(condition.ToString()))
                    return first;
            return first.Compose(second, Expression.And);
        }

        /// <summary>
        /// 条件表达式
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="first">第一拼接条件</param>
        /// <param name="second">第二拼接条件</param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> Filter<T>(this Expression<Func<T, bool>> first,
            Expression<Func<T, bool>> second)
        {
            if (second == null)
                return first;
            var value = GetValue(second);
            if (value == null)
                return first;
            if (value is string && string.IsNullOrEmpty(value.ToString()))
                return first;
            return first.Compose(second, Expression.And);
        }

        /// <summary>
        /// 表达式右边的值
        /// </summary>
        /// <param name="expression">表达式</param>
        /// <returns></returns>
        public static object GetValue(Expression expression)
        {
            if (expression == null)
                return null;
            switch (expression.NodeType)
            {
                case ExpressionType.Lambda:
                    return GetValue(((LambdaExpression)expression).Body);
                case ExpressionType.Convert:
                    return GetValue(((UnaryExpression)expression).Operand);
                case ExpressionType.Equal:
                case ExpressionType.NotEqual:
                case ExpressionType.GreaterThan:
                case ExpressionType.LessThan:
                case ExpressionType.GreaterThanOrEqual:
                case ExpressionType.LessThanOrEqual:
                    return GetValue(((BinaryExpression)expression).Right);
                case ExpressionType.Call:
                    return GetValue(((MethodCallExpression)expression).Arguments.FirstOrDefault());
                case ExpressionType.MemberAccess:
                    return GetMemberValue((MemberExpression)expression);
                case ExpressionType.Constant:
                    return GetConstantExpressionValue(expression);
            }
            return null;
        }

        private static object GetMemberValue(MemberExpression expression)
        {
            if (expression == null)
                return null;
            var field = expression.Member as FieldInfo;
            if (field != null)
            {
                var constValue = GetConstantExpressionValue(expression.Expression);
                return field.GetValue(constValue);
            }
            var property = expression.Member as PropertyInfo;
            if (property == null)
                return null;
            var value = GetMemberValue(expression.Expression as MemberExpression);
            return property.GetValue(value, null);
        }

        private static object GetConstantExpressionValue(Expression expression)
        {
            var constantExpression = (ConstantExpression)expression;
            return constantExpression.Value;
        }
    }
}
