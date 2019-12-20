using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Sand.Domain.Entities;
using Sand.Expressions;
using Sand.Lambdas;

namespace Sand.Domain.Query
{
    /// <summary>
    /// 查询接口
    /// </summary>
    public interface IQuery
    {
        /// <summary>
        /// 启始页
        /// </summary>
        int PageIndex { get; set; }

        /// <summary>
        /// 每页数量
        /// </summary>
        int PageSize { get; set; }

        /// <summary>
        /// 查询对象(用对dapper构建参数时用)
        /// </summary>
        object Param { get; set; }
        /// <summary>
        ///获取前多少条
        /// </summary>
        int? Top { get; set; }
    }

    /// <summary>
    /// 查询接口
    /// </summary>
    /// <typeparam name="TEntity">实体</typeparam>
    /// <typeparam name="TKey">主键</typeparam>
    public interface IQuery<TEntity, TKey> : IQuery where TEntity : class, IEntity
    {
        /// <summary>
        /// 查询数据
        /// </summary>
        string QueryData { get; set; }
        /// <summary>
        /// 是否开启跟踪
        /// </summary>
        bool IsTracking { get; set; }

        /// <summary>
        /// 获取排序
        /// </summary>
        string GetOrderBy();

        /// <summary>
        /// 获取分页
        /// </summary>
        IPager GetPager();

        /// <summary>
        /// 获取查询条件
        /// </summary>
        Expression<Func<TEntity, bool>> GetPredicate();

        /// <summary>
        /// 添加排序
        /// </summary>
        /// <typeparam name="TProperty">属性类型</typeparam>
        /// <param name="expression">属性表达式</param>
        /// <param name="desc">是否降序</param>
        IQuery<TEntity, TKey> OrderBy<TProperty>(Expression<Func<TEntity, TProperty>> expression, bool desc = false);

        /// <summary>
        /// 添加排序
        /// </summary>
        /// <param name="propertyName">排序属性</param>
        /// <param name="desc">是否降序</param>
        IQuery<TEntity, TKey> OrderBy(string propertyName, bool desc = false);

        /// <summary>
        /// 添加查询条件
        /// </summary>
        /// <param name="predicate">查询条件</param>
        IQuery<TEntity, TKey> Where(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 选择性添加查询条件
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <param name="condition">该值为true则添加条件，否则忽略</param>
        IQuery<TEntity, TKey> WhereIf(Expression<Func<TEntity, bool>> predicate, bool condition);
        /// <summary>
        /// 与连接
        /// </summary>
        /// <param name="query">查询对象</param>
        IQuery<TEntity, TKey> And(IQuery<TEntity, TKey> query);

        /// <summary>
        /// 与连接
        /// </summary>
        /// <param name="predicate">查询条件</param>
        IQuery<TEntity, TKey> And(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 或连接
        /// </summary>
        /// <param name="query">查询对象</param>
        IQuery<TEntity, TKey> Or(IQuery<TEntity, TKey> query);

        /// <summary>
        /// 或连接
        /// </summary>
        /// <param name="predicate">查询条件</param>
        IQuery<TEntity, TKey> Or(Expression<Func<TEntity, bool>> predicate);
    }

    /// <summary>
    /// 查询对象
    /// </summary>
    /// <typeparam name="TEntity">实体</typeparam>
    /// <typeparam name="TKey">实体Key</typeparam>
    [Serializable]
    public class BaseQuery<TEntity, TKey> : IQuery<TEntity, TKey> where TEntity : class, IEntity
    {
        /// <summary>
        /// 排序生成器
        /// </summary>
        private OrderByBuilder _orderBuilder { get; set; }
        /// <summary>
        /// 查询条件
        /// </summary>
        private Expression<Func<TEntity, bool>> _predicate;
        /// <summary>
        /// 查询数据
        /// </summary>
        public string QueryData { get; set; }
        /// <summary>
        /// 启始页
        /// </summary>
        public int PageIndex { get; set; }
        /// <summary>
        /// 每页数量
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// 跟踪号
        /// </summary>
        public bool IsTracking { get; set; }
        /// <summary>
        /// 参数构造器
        /// </summary>
        public object Param { get; set; }
        /// <summary>
        /// 前多少条数据
        /// </summary>
        public int? Top { get; set; }

        /// <summary>
        /// 初始化
        /// </summary>
        public BaseQuery()
        {
            PageIndex = 1;
            PageSize = 15;
        }

        /// <summary>
        /// 获取排序
        /// </summary>
        /// <returns></returns>
        public string GetOrderBy()
        {
            _orderBuilder = _orderBuilder ?? new OrderByBuilder();
            var order = _orderBuilder.Generate();
            if (string.IsNullOrWhiteSpace(order))
                return "CreateTime Desc";
            return order;
        }

        /// <summary>
        /// 获取分页
        /// </summary>
        /// <returns></returns>
        public IPager GetPager()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取表达式
        /// </summary>
        /// <returns></returns>
        public Expression<Func<TEntity, bool>> GetPredicate()
        {
            return _predicate;
        }

        /// <summary>
        /// 排序
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="expression"></param>
        /// <param name="desc"></param>
        /// <returns></returns>
        public IQuery<TEntity, TKey> OrderBy<TProperty>(Expression<Func<TEntity, TProperty>> expression, bool desc = false)
        {
            return OrderBy(Lambda.GetName(expression), desc);
        }
        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="desc"></param>
        /// <returns></returns>
        public IQuery<TEntity, TKey> OrderBy(string propertyName, bool desc = false)
        {
            _orderBuilder = _orderBuilder ?? new OrderByBuilder();
            _orderBuilder.Add(propertyName, desc);
            return this;
        }
        /// <summary>
        /// where
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IQuery<TEntity, TKey> Where(Expression<Func<TEntity, bool>> predicate)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// where
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public IQuery<TEntity, TKey> WhereIf(Expression<Func<TEntity, bool>> predicate, bool condition)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// And
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IQuery<TEntity, TKey> And(IQuery<TEntity, TKey> query)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// And
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IQuery<TEntity, TKey> And(Expression<Func<TEntity, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Or
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IQuery<TEntity, TKey> Or(IQuery<TEntity, TKey> query)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Or
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IQuery<TEntity, TKey> Or(Expression<Func<TEntity, bool>> predicate)
        {
            throw new NotImplementedException();
        }


    }

    /// <summary>
    /// 查询对象
    /// </summary>
    /// <typeparam name="TEntity">实体</typeparam>
    [Serializable]
    public class BaseQuery<TEntity> : BaseQuery<TEntity, string> where TEntity : class, IEntity
    {
    }
}
