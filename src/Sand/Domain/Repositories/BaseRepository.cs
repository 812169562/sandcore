using Sand.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Sand.Context;
using Sand.Domain.Uow;
using Sand.DI;
using Autofac;

namespace Sand.Domain.Repositories
{
    /// <summary>
    /// 仓储基类
    /// </summary>
    /// <typeparam name="TEntity">实体</typeparam>
    /// <typeparam name="TPrimaryKey">实体主键类型</typeparam>
    public abstract class BaseRepository<TEntity, TPrimaryKey> : IRepository<TEntity, TPrimaryKey> where TEntity : class, IEntity<TPrimaryKey>
    {
        /// <summary>
        /// 工作单元
        /// </summary>
        protected IUnitOfWork Uow { get; set; }
        /// <summary>
        /// 基础仓储
        /// </summary>
        /// <param name="unitOfWork"></param>
        protected BaseRepository(IUnitOfWork unitOfWork)
        {
            Uow = unitOfWork;
            UserContext = Ioc.GetService<IUserContext>();
        }

        /// <summary>
        /// 用户上下文
        /// </summary>
        public virtual IUserContext UserContext { get; set; }

        /// <summary>
        /// 获取条数
        /// </summary>
        /// <param name="predicate">条件表达式</param>
        /// <returns>条数</returns>
        public abstract int Count(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 异步获取条数
        /// </summary>
        /// <param name="predicate">条件表达式</param>
        /// <returns>条数</returns>
        public virtual async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await Task.FromResult(Count(predicate));
        }

        /// <summary>
        /// 创建对象
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>创建的实体对象</returns>
        public abstract TEntity Create(TEntity entity);

        /// <summary>
        /// 异步创建对象
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>创建的实体对象</returns>
        public virtual async Task<TEntity> CreateAsync(TEntity entity)
        {
            return await Task.FromResult(Create(entity));
        }

        /// <summary>
        /// 创建对象集合
        /// </summary>
        /// <param name="entities">实体对象集合</param>
        /// <returns>创建对象集合</returns>
        public abstract IList<TEntity> CreateList(IList<TEntity> entities);

        /// <summary>
        /// 异步创建对象集合
        /// </summary>
        /// <param name="entities">实体对象集合</param>
        /// <returns>创建对象集合</returns>
        public virtual async Task<IList<TEntity>> CreateListAsync(IList<TEntity> entities)
        {
            return await Task.FromResult(CreateList(entities));
        }

        /// <summary>
        /// 创建对象集合
        /// </summary>
        /// <param name="entity">实体对象集合</param>
        /// <returns>实体主键</returns>
        public abstract TPrimaryKey CreateReturnId(TEntity entity);

        /// <summary>
        /// 异步创建对象集合
        /// </summary>
        /// <param name="entity">实体对象集合</param>
        /// <returns>实体主键</returns>
        public virtual async Task<TPrimaryKey> CreateReturnIdAsync(TEntity entity)
        {
            return await Task.FromResult(CreateReturnId(entity));
        }

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="entity">删除实体</param>
        public abstract void Delete(TEntity entity);

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="id">实体编号</param>
        public abstract void Delete(TPrimaryKey id);

        /// <summary>
        /// 异步删除实体
        /// </summary>
        /// <param name="entity">删除实体</param>
        public virtual Task DeleteAsync(TEntity entity)
        {
            Delete(entity);
            return Task.FromResult(0);
        }

        /// <summary>
        /// 异步删除实体
        /// </summary>
        /// <param name="id">实体编号</param>
        public Task DeleteAsync(TPrimaryKey id)
        {
            Delete(id);
            return Task.CompletedTask;
        }

        /// <summary>
        /// 删除实体集合
        /// </summary>
        /// <param name="ids">实体编号集合</param>
        public abstract void Delete(IList<TPrimaryKey> ids);

        /// <summary>
        /// 异步删除实体集合
        /// </summary>
        /// <param name="ids">实体编号</param>
        /// <returns></returns>
        public virtual Task DeleteAsync(IList<TPrimaryKey> ids)
        {
            Delete(ids);
            return Task.CompletedTask;
        }

        /// <summary>
        /// 获取实体查询对象
        /// </summary>
        /// <returns>查询对象</returns>
        public abstract IQueryable<TEntity> Retrieve();

        /// <summary>
        /// 查询对象
        /// </summary>
        /// <param name="predicate">条件表达式</param>
        /// <returns>实体对象</returns>
        public abstract IQueryable<TEntity> Retrieve(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 获取所有集合
        /// </summary>
        /// <returns>获取所有集合</returns>
        public abstract IList<TEntity> RetrieveAll();

        /// <summary>
        /// 异步获取所有集合
        /// </summary>
        /// <returns>获取所有集合</returns>
        public virtual async Task<IList<TEntity>> RetrieveAllAsync()
        {
            return await Task.FromResult(RetrieveAll());
        }

        /// <summary>
        /// 异步获取查询对象
        /// </summary>
        /// <param name="predicate">条件表达式</param>
        /// <returns>实体对象</returns>
        public virtual async Task<IQueryable<TEntity>> RetrieveAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await Task.FromResult(Retrieve(predicate));
        }

        /// <summary>
        /// 根据编号查询实体
        /// </summary>
        /// <param name="id">实体编号</param>
        /// <returns>创建后的实体集</returns>
        public abstract TEntity RetrieveById(TPrimaryKey id);

        /// <summary>
        /// 异步根据编号查询实体
        /// </summary>
        /// <param name="id">实体编号</param>
        /// <returns>创建后的实体集</returns>
        public virtual async Task<TEntity> RetrieveByIdAsync(TPrimaryKey id)
        {
            return await Task.FromResult(RetrieveById(id));
        }

        /// <summary>
        /// 异步根据编号集合查询实体
        /// </summary>
        /// <param name="ids">实体编号</param>
        /// <returns>创建后的实体集</returns>
        public abstract IList<TEntity> RetrieveByIds(IList<TPrimaryKey> ids);

        /// <summary>
        /// 异步根据编号集合查询实体
        /// </summary>
        /// <param name="ids">实体编号</param>
        /// <returns>创建后的实体集</returns>
        public virtual async Task<IList<TEntity>> RetrieveByIdsAsync(IList<TPrimaryKey> ids)
        {
            return await Task.FromResult(RetrieveByIds(ids));
        }

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="entity">更新实体</param>
        /// <returns>更新后实体</returns>
        public abstract TEntity Update(TEntity entity);

        /// <summary>
        /// 异步更新实体
        /// </summary>
        /// <param name="entity">更新实体</param>
        /// <returns>更新后实体</returns>
        public virtual async Task<TEntity> UpdateAsync(TEntity entity)
        {
            return await Task.FromResult(Update(entity));
        }

        /// <summary>
        /// 异步更新实体
        /// </summary>
        /// <param name="id">实体编号</param>
        /// <param name="update">更新动作</param>
        /// <returns></returns>
        public abstract Task<int> UpdateAsync(TPrimaryKey id, Expression<Func<TEntity, TEntity>> update);
        /// <summary>
        /// 部分跟新实体（根据条件部分更新,必须写表达式）
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="update">更新实体</param>
        /// <returns></returns>
        public abstract Task<int> UpdateAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TEntity>> update);
        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="id">实体编号</param>
        /// <param name="update">更新动作</param>
        /// <returns></returns>
        public abstract int Update(TPrimaryKey id, Expression<Func<TEntity, TEntity>> update);
        /// <summary>
        /// 部分跟新实体（根据条件部分更新,必须写表达式）
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="update">更新实体</param>
        /// <returns></returns>
        public abstract int Update(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TEntity>> update);
        /// <summary>
        /// 批量部分跟新实体（根据条件部分更新,必须赋值Id）
        /// </summary>
        /// <param name="entities">更新实体</param>
        /// <returns>更新成功条数</returns>
        public abstract int Update(IList<TEntity> entities);

        /// <summary>
        /// 批量部分跟新实体（根据条件部分更新,必须赋值Id）
        /// </summary>
        /// <param name="entities">更新实体</param>
        /// <returns>更新成功条数</returns>
        public abstract Task<int> UpdateAsync(IList<TEntity> entities);
    }
}
