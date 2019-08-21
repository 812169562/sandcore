using System;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Collections.Generic;
using MongoDB.Driver;
using Sand.Domain.Entities;
using Sand.Dependency;

namespace Sand.Mongo
{
    /// <summary>
    /// mongo based repository interface
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IMongoRepository<T> : IDependency where T : IMongoEntity
    {
        #region MongoSpecific

        /// <summary>
        /// mongo collection
        /// </summary>
        IMongoCollection<T> Collection { get; }

        /// <summary>
        /// filter for collection
        /// </summary>
        FilterDefinitionBuilder<T> Filter { get; }

        /// <summary>
        /// projector for collection
        /// </summary>
        ProjectionDefinitionBuilder<T> Project { get; }

        /// <summary>
        /// updater for collection
        /// </summary>
        UpdateDefinitionBuilder<T> Updater { get; }

        #endregion MongoSpecific

        #region CRUD

        #region Delete

        /// <summary>
        /// 根据编号删除
        /// </summary>
        /// <param name="id">编号</param>
        bool Delete(string id);


        /// <summary>
        /// 删除一条实体数据
        /// </summary>
        /// <param name="entity">实体</param>
        bool Delete(T entity);

        /// <summary>
        /// 根据过滤条件删除数据
        /// </summary>
        /// <param name="filter">条件表达式</param>
        bool Delete(Expression<Func<T, bool>> filter);

        /// <summary>
        /// 删除所有数据
        /// </summary>
        bool DeleteAll();

        /// <summary>
        /// 根据编号删除
        /// </summary>
        /// <param name="id">编号</param>
        Task<bool> DeleteAsync(string id);

        /// <summary>
        /// 删除一条实体数据
        /// </summary>
        /// <param name="entity">实体</param>
        Task<bool> DeleteAsync(T entity);

        /// <summary>
        /// 根据过滤条件删除数据
        /// </summary>
        /// <param name="filter">expression filter</param>
        Task<bool> DeleteAsync(Expression<Func<T, bool>> filter);

        /// <summary>
        /// 删除所有数据
        /// </summary>
        Task<bool> DeleteAllAsync();
        #endregion Delete

        #region Find

        /// <summary>
        /// 根据实体查询
        /// </summary>
        /// <param name="filter">filter definition</param>
        /// <returns>collection of entity</returns>
        IEnumerable<T> Find(FilterDefinition<T> filter);

        /// <summary>
        /// 根据实体表达式查询
        /// </summary>
        /// <param name="filter">expression filter</param>
        /// <returns>collection of entity</returns>
        IEnumerable<T> Find(Expression<Func<T, bool>> filter);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="filter">expression filter</param>
        /// <param name="pageIndex">page index, based on 0</param>
        /// <param name="size">number of items in page</param>
        /// <returns>collection of entity</returns>
        IEnumerable<T> Find(Expression<Func<T, bool>> filter, int pageIndex, int size);

        /// <summary>
        /// 分页查询
        /// default ordering is descending
        /// </summary>
        /// <param name="filter">条件表达式</param>
        /// <param name="order">排序表达式</param>
        /// <param name="pageIndex">page index, based on 0</param>
        /// <param name="size">number of items in page</param>
        /// <returns>collection of entity</returns>
        IEnumerable<T> Find(Expression<Func<T, bool>> filter, Expression<Func<T, object>> order, int pageIndex, int size);

        /// <summary>
        /// find entities with paging and ordering in direction
        /// </summary>
        /// <param name="filter">条件表达式</param>
        /// <param name="order">排序表达式</param>
        /// <param name="pageIndex">page index, based on 0</param>
        /// <param name="size">number of items in page</param>
        /// <param name="isDescending">ordering direction</param>
        /// <returns>collection of entity</returns>
        IEnumerable<T> Find(Expression<Func<T, bool>> filter, Expression<Func<T, object>> order, int pageIndex, int size, bool isDescending);

        #endregion Find

        #region FindAll

        /// <summary>
        /// 查询所有数据
        /// </summary>
        /// <returns>collection of entity</returns>
        IEnumerable<T> FindAll();

        /// <summary>
        /// 查询所有数据
        /// </summary>
        /// <param name="pageIndex">page index, based on 0</param>
        /// <param name="size">number of items in page</param>
        /// <returns>collection of entity</returns>
        IEnumerable<T> FindAll(int pageIndex, int size);

        /// <summary>
        /// 查询所有数据
        /// default ordering is descending
        /// </summary>
        /// <param name="order">ordering parameters</param>
        /// <param name="pageIndex">page index, based on 0</param>
        /// <param name="size">number of items in page</param>
        /// <returns>collection of entity</returns>
        IEnumerable<T> FindAll(Expression<Func<T, object>> order, int pageIndex, int size);

        /// <summary>
        /// fetch all items in collection with paging and ordering in direction
        /// </summary>
        /// <param name="order">排序表达式</param>
        /// <param name="pageIndex">page index, based on 0</param>
        /// <param name="size">number of items in page</param>
        /// <param name="isDescending">ordering direction</param>
        /// <returns>collection of entity</returns>
        IEnumerable<T> FindAll(Expression<Func<T, object>> order, int pageIndex, int size, bool isDescending);

        #endregion FindAll

        #region First

        /// <summary>
        /// 查询第一条数据
        /// </summary>
        /// <returns>entity of <typeparamref name="T"/></returns>
        T First();

        /// <summary>
        /// 根据条件查询第一条数据
        /// </summary>
        /// <param name="filter">filter definition</param>
        /// <returns>entity of <typeparamref name="T"/></returns>
        T First(FilterDefinition<T> filter);

        /// <summary>
        /// 根据条件查询第一条数据
        /// </summary>
        /// <param name="filter">expression filter</param>
        /// <returns>entity of <typeparamref name="T"/></returns>
        T First(Expression<Func<T, bool>> filter);

        /// <summary>
        /// 按照某种条件进行查询第一条数据
        /// </summary>
        /// <param name="filter">expression filter</param>
        /// <param name="order">排序表达式</param>
        /// <returns>entity of <typeparamref name="T"/></returns>
        T First(Expression<Func<T, bool>> filter, Expression<Func<T, object>> order);

        /// <summary>
        /// 按照某种条件进行查询第一条数据
        /// </summary>
        /// <param name="filter">expression filter</param>
        /// <param name="order">排序表达式</param>
        /// <param name="isDescending">ordering direction</param>
        /// <returns>entity of <typeparamref name="T"/></returns>
        T First(Expression<Func<T, bool>> filter, Expression<Func<T, object>> order, bool isDescending);

        #endregion First

        #region Get

        /// <summary>
        /// 根据主键获取数据
        /// </summary>
        /// <param name="id">id value</param>
        /// <returns>entity of <typeparamref name="T"/></returns>
        T Get(string id);

        #endregion Get

        #region Insert

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity">实体</param>
        void Insert(T entity);

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity">实体</param>
        Task InsertAsync(T entity);

        /// <summary>
        /// 批量插入数据集
        /// </summary>
        /// <param name="entities">实体数据集</param>
        void Insert(IEnumerable<T> entities);

        /// <summary>
        /// 批量插入数据集
        /// </summary>
        /// <param name="entities">实体数据集</param>
        Task InsertAsync(IEnumerable<T> entities);

        #endregion Insert

        #region Last

        /// <summary>
        /// 获取最后一条数据
        /// </summary>
        /// <returns>entity of <typeparamref name="T"/></returns>
        T Last();

        /// <summary>
        /// 根据条件获取最后一条数据
        /// </summary>
        /// <param name="filter">filter definition</param>
        /// <returns>entity of <typeparamref name="T"/></returns>
        T Last(FilterDefinition<T> filter);

        /// <summary>
        /// 根据条件获取最后一条数据
        /// </summary>
        /// <param name="filter">条件表达式</param>
        /// <returns>entity of <typeparamref name="T"/></returns>
        T Last(Expression<Func<T, bool>> filter);

        /// <summary>
        /// 根据条件获取最后一条数据
        /// </summary>
        /// <param name="filter">条件表达式</param>
        /// <param name="order">排序表达式</param>
        /// <returns>entity of <typeparamref name="T"/></returns>
        T Last(Expression<Func<T, bool>> filter, Expression<Func<T, object>> order);

        /// <summary>
        /// 根据条件获取最后一条数据
        /// </summary>
        /// <param name="filter">条件表达式</param>
        /// <param name="order">排序表达式</param>
        /// <param name="isDescending">ordering direction</param>
        /// <returns>entity of <typeparamref name="T"/></returns>
        T Last(Expression<Func<T, bool>> filter, Expression<Func<T, object>> order, bool isDescending);

        #endregion Last

        #region Replace

        /// <summary>
        /// 替换已经存在的数据
        /// </summary>
        /// <param name="entity">实体</param>
        bool Replace(T entity);

        /// <summary>
        /// 替换已经存在的数据
        /// </summary>
        /// <param name="entity">实体</param>
        Task<bool> ReplaceAsync(T entity);

        /// <summary>
        /// 替换已经存在的数据集
        /// </summary>
        /// <param name="entities">数据集</param>
        void Replace(IEnumerable<T> entities);

        #endregion Replace

        #region Update

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="id">编号</param>
        /// <param name="updates">updated field(s)</param>
        /// <returns>true if successful, otherwise false</returns>
        bool Update(string id, params UpdateDefinition<T>[] updates);

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="updates">updated field(s)</param>
        /// <returns>true if successful, otherwise false</returns>
        bool Update(T entity, params UpdateDefinition<T>[] updates);

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="filter">更新条件</param>
        /// <param name="updates">updated field(s)</param>
        /// <returns>true if successful, otherwise false</returns>
        bool Update(FilterDefinition<T> filter, params UpdateDefinition<T>[] updates);

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="filter">更新条件</param>
        /// <param name="updates">updated field(s)</param>
        /// <returns>true if successful, otherwise false</returns>
        bool Update(Expression<Func<T, bool>> filter, params UpdateDefinition<T>[] updates);

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <typeparam name="TField">field type</typeparam>
        /// <param name="entity">实体</param>
        /// <param name="field">表达式值</param>
        /// <param name="value">值</param>
        /// <returns>true if successful, otherwise false</returns>
        bool Update<TField>(T entity, Expression<Func<T, TField>> field, TField value);

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <typeparam name="TField">field type</typeparam>
        /// <param name="filter">filter</param>
        /// <param name="field">field</param>
        /// <param name="value">new value</param>
        /// <returns>true if successful, otherwise false</returns>
        bool Update<TField>(FilterDefinition<T> filter, Expression<Func<T, TField>> field, TField value);

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <typeparam name="TField">field type</typeparam>
        /// <param name="entity">entity</param>
        /// <param name="field">field</param>
        /// <param name="value">new value</param>
        Task<bool> UpdateAsync<TField>(T entity, Expression<Func<T, TField>> field, TField value);

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="updates">updated field(s)</param>
        Task<bool> UpdateAsync(string id, params UpdateDefinition<T>[] updates);

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="entity">entity</param>
        /// <param name="updates">updated field(s)</param>
        Task<bool> UpdateAsync(T entity, params UpdateDefinition<T>[] updates);

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <typeparam name="TField">field type</typeparam>
        /// <param name="filter">filter</param>
        /// <param name="field">field</param>
        /// <param name="value">new value</param>
        Task<bool> UpdateAsync<TField>(FilterDefinition<T> filter, Expression<Func<T, TField>> field, TField value);

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="filter">collection filter</param>
        /// <param name="updates">updated field(s)</param>
        Task<bool> UpdateAsync(FilterDefinition<T> filter, params UpdateDefinition<T>[] updates);

        /// <summary>
        ///更新数据
        /// </summary>
        /// <param name="filter">collection filter</param>
        /// <param name="updates">updated field(s)</param>
        Task<bool> UpdateAsync(Expression<Func<T, bool>> filter, params UpdateDefinition<T>[] updates);
        #endregion Update

        #endregion CRUD

        #region Utils
        /// <summary>
        /// 查看是否存在
        /// </summary>
        /// <param name="filter">expression filter</param>
        /// <returns>true if exists, otherwise false</returns>
        bool Any(Expression<Func<T, bool>> filter);

        #region EstimatedCount
        /// <summary>
        /// 获取条数
        /// </summary>
        /// <param name="options">count options</param>
        /// <returns>number of documents</returns>
        long EstimatedCount(EstimatedDocumentCountOptions options);

        /// <summary>
        /// 获取条数
        /// </summary>
        /// <param name="options">count options</param>
        /// <returns>number of documents</returns>
        Task<long> EstimatedCountAsync(EstimatedDocumentCountOptions options);

        /// <summary>
        /// 获取总条数
        /// </summary>
        /// <returns>number of documents</returns>
        long EstimatedCount();

        /// <summary>
        /// 获取总条数
        /// </summary>
        /// <returns>number of documents</returns>
        Task<long> EstimatedCountAsync();
        #endregion EstimatedCount

        #endregion Utils
    }
}