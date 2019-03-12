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
        /// 添加查询条件,如果参数值为空，则忽略该条件，注意：一次仅能添加一个条件
        /// </summary>
        /// <param name="predicate">查询条件</param>
        IQuery<TEntity, TKey> Filter(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 添加查询条件
        /// </summary>
        /// <param name="propertyName">属性名</param>
        /// <param name="value">值</param>
        /// <param name="operator">运算符</param>
        IQuery<TEntity, TKey> Filter(string propertyName, object value, Operator @operator = Operator.Equal);

        /// <summary>
        /// 添加查询条件
        /// </summary>
        /// <param name="criteria">规约对象,由于规约对象可能返回多个条件组合，所以由规约对象本身进行空值判断</param>
        IQuery<TEntity, TKey> Filter(TEntity criteria);

        /// <summary>
        /// 添加整数范围过滤条件
        /// </summary>
        /// <typeparam name="TProperty">属性类型</typeparam>
        /// <param name="propertyExpression">属性表达式，范例：t => t.Age</param>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        IQuery<TEntity, TKey> FilterInt<TProperty>(Expression<Func<TEntity, TProperty>> propertyExpression, int? min,
            int? max);

        /// <summary>
        /// 添加double范围过滤条件
        /// </summary>
        /// <typeparam name="TProperty">属性类型</typeparam>
        /// <param name="propertyExpression">属性表达式，范例：t => t.Age</param>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        IQuery<TEntity, TKey> FilterDouble<TProperty>(Expression<Func<TEntity, TProperty>> propertyExpression,
            double? min, double? max);

        /// <summary>
        /// 添加decimal范围过滤条件
        /// </summary>
        /// <typeparam name="TProperty">属性类型</typeparam>
        /// <param name="propertyExpression">属性表达式，范例：t => t.Age</param>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        IQuery<TEntity, TKey> FilterDecimal<TProperty>(Expression<Func<TEntity, TProperty>> propertyExpression,
            decimal? min, decimal? max);

        /// <summary>
        /// 添加日期范围过滤条件 - 不包含时间
        /// </summary>
        /// <typeparam name="TProperty">属性类型</typeparam>
        /// <param name="propertyExpression">属性表达式，范例：t => t.Age</param>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        IQuery<TEntity, TKey> FilterDate<TProperty>(Expression<Func<TEntity, TProperty>> propertyExpression,
            DateTime? min, DateTime? max);

        /// <summary>
        /// 添加日期范围过滤条件 - 包含时间
        /// </summary>
        /// <typeparam name="TProperty">属性类型</typeparam>
        /// <param name="propertyExpression">属性表达式，范例：t => t.Age</param>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        IQuery<TEntity, TKey> FilterDateTime<TProperty>(Expression<Func<TEntity, TProperty>> propertyExpression,
            DateTime? min, DateTime? max);

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
    public class BaseQuery<TEntity, TKey> : IQuery<TEntity, TKey> where TEntity : class, IEntity
    {
        /// <summary>
        /// 排序生成器
        /// </summary>
        private OrderByBuilder _orderBuilder { get; set; }

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
            throw new NotImplementedException();
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
        /// Filter
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IQuery<TEntity, TKey> Filter(Expression<Func<TEntity, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Filter
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        /// <param name="operator"></param>
        /// <returns></returns>
        public IQuery<TEntity, TKey> Filter(string propertyName, object value, Operator @operator = Operator.Equal)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Filter
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public IQuery<TEntity, TKey> Filter(TEntity criteria)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Filter
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="propertyExpression"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public IQuery<TEntity, TKey> FilterInt<TProperty>(Expression<Func<TEntity, TProperty>> propertyExpression, int? min, int? max)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="propertyExpression"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public IQuery<TEntity, TKey> FilterDouble<TProperty>(Expression<Func<TEntity, TProperty>> propertyExpression, double? min, double? max)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// FilterDecimal
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="propertyExpression"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public IQuery<TEntity, TKey> FilterDecimal<TProperty>(Expression<Func<TEntity, TProperty>> propertyExpression, decimal? min, decimal? max)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// FilterDate
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="propertyExpression"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public IQuery<TEntity, TKey> FilterDate<TProperty>(Expression<Func<TEntity, TProperty>> propertyExpression, DateTime? min, DateTime? max)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// FilterDateTime
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="propertyExpression"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public IQuery<TEntity, TKey> FilterDateTime<TProperty>(Expression<Func<TEntity, TProperty>> propertyExpression, DateTime? min, DateTime? max)
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

    public class BaseQuery<TEntity> : BaseQuery<TEntity, string> where TEntity : class, IEntity
    {
    }
}
