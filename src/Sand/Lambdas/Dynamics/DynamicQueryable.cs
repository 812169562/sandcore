using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Sand.Lambdas.Dynamics {
    /// <summary>
    /// /
    /// </summary>
    public static class DynamicQueryable {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="predicate"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static IQueryable<T> WhereDynamic<T>( this IQueryable<T> source, string predicate, params object[] values ) {
            return (IQueryable<T>)WhereDynamic( (IQueryable)source, predicate, values );
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="predicate"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static IQueryable WhereDynamic( this IQueryable source, string predicate, params object[] values ) {
            if ( source == null ) throw new ArgumentNullException( "source" );
            if ( predicate == null ) throw new ArgumentNullException( "predicate" );
            LambdaExpression lambda = DynamicExpression.ParseLambda( source.ElementType, typeof( bool ), predicate, values );
            return source.Provider.CreateQuery(
                Expression.Call(
                    typeof( Queryable ), "Where",
                    new Type[] { source.ElementType },
                    source.Expression, Expression.Quote( lambda ) ) );
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="selector"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static IQueryable Select( this IQueryable source, string selector, params object[] values ) {
            if ( source == null ) throw new ArgumentNullException( "source" );
            if ( selector == null ) throw new ArgumentNullException( "selector" );
            LambdaExpression lambda = DynamicExpression.ParseLambda( source.ElementType, null, selector, values );
            return source.Provider.CreateQuery(
                Expression.Call(
                    typeof( Queryable ), "Select",
                    new Type[] { source.ElementType, lambda.Body.Type },
                    source.Expression, Expression.Quote( lambda ) ) );
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="ordering"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static IQueryable<T> OrderByDynamic<T>( this IQueryable<T> source, string ordering, params object[] values ) {
            return (IQueryable<T>)OrderByDynamic( (IQueryable)source, ordering, values );
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="ordering"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static IQueryable OrderByDynamic( this IQueryable source, string ordering, params object[] values ) {
            if ( source == null ) throw new ArgumentNullException( "source" );
            if ( ordering == null ) throw new ArgumentNullException( "ordering" );
            ParameterExpression[] parameters = new ParameterExpression[] {
                Expression.Parameter(source.ElementType, "") };
            ExpressionParser parser = new ExpressionParser( parameters, ordering, values );
            IEnumerable<DynamicOrdering> orderings = parser.ParseOrdering();
            Expression queryExpr = source.Expression;
            string methodAsc = "OrderBy";
            string methodDesc = "OrderByDescending";
            foreach ( DynamicOrdering o in orderings ) {
                queryExpr = Expression.Call(
                    typeof( Queryable ), o.Ascending ? methodAsc : methodDesc,
                    new Type[] { source.ElementType, o.Selector.Type },
                    queryExpr, Expression.Quote( Expression.Lambda( o.Selector, parameters ) ) );
                methodAsc = "ThenBy";
                methodDesc = "ThenByDescending";
            }
            return source.Provider.CreateQuery( queryExpr );
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static IQueryable<T> Take<T>( this IQueryable<T> source, int? count ) {
            if (count==null)
                return source;
            if ( source == null ) throw new ArgumentNullException( "source" );
            return (IQueryable<T>)source.Provider.CreateQuery(
                Expression.Call(
                    typeof( Queryable ), "Take",
                    new Type[] { source.ElementType },
                    source.Expression, Expression.Constant( count ) ) );
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static IQueryable<T> Skip<T>( this IQueryable<T> source, int? count ) {
            if (count == null)
                return source;
            if ( source == null ) throw new ArgumentNullException( "source" );
            return (IQueryable<T>) source.Provider.CreateQuery(
                Expression.Call(
                    typeof( Queryable ), "Skip",
                    new Type[] { source.ElementType },
                    source.Expression, Expression.Constant( count ) ) );
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="keySelector"></param>
        /// <param name="elementSelector"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static IQueryable GroupBy( this IQueryable source, string keySelector, string elementSelector, params object[] values ) {
            if ( source == null ) throw new ArgumentNullException( "source" );
            if ( keySelector == null ) throw new ArgumentNullException( "keySelector" );
            if ( elementSelector == null ) throw new ArgumentNullException( "elementSelector" );
            LambdaExpression keyLambda = DynamicExpression.ParseLambda( source.ElementType, null, keySelector, values );
            LambdaExpression elementLambda = DynamicExpression.ParseLambda( source.ElementType, null, elementSelector, values );
            return source.Provider.CreateQuery(
                Expression.Call(
                    typeof( Queryable ), "GroupBy",
                    new Type[] { source.ElementType, keyLambda.Body.Type, elementLambda.Body.Type },
                    source.Expression, Expression.Quote( keyLambda ), Expression.Quote( elementLambda ) ) );
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool Any( this IQueryable source ) {
            if ( source == null ) throw new ArgumentNullException( "source" );
            return (bool)source.Provider.Execute(
                Expression.Call(
                    typeof( Queryable ), "Any",
                    new Type[] { source.ElementType }, source.Expression ) );
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static int Count( this IQueryable source ) {
            if ( source == null ) throw new ArgumentNullException( "source" );
            return (int)source.Provider.Execute(
                Expression.Call(
                    typeof( Queryable ), "Count",
                    new Type[] { source.ElementType }, source.Expression ) );
        }
    }
}
