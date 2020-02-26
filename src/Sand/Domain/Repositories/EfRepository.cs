using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sand.Dependency;
using Sand.Domain.Entities;
using Sand.Domain.Uow;
using Sand.Exceptions;
using Sand.Lambdas.Dynamics;
using Sand.EntityFramework.UpdatePlus.Extensions;
using Sand.EntityFramework.UpdatePlus;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Sand.Helpers;
using Sand.Context;
using Sand.DI;

namespace Sand.Domain.Repositories
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TPrimaryKey"></typeparam>
    public class EfRepository<TEntity, TPrimaryKey> : BaseRepository<TEntity, TPrimaryKey> where TEntity : class, IEntity<TPrimaryKey>, new()
    {
        /// <summary>
        /// 实体集
        /// </summary>
        public override DbSet<TEntity> Table { get { return WriteUow.Set<TEntity>(); } }
        /// <summary>
        /// 读取数据集合实体集
        /// </summary>
        public override DbSet<TEntity> ReadTable { get { if (DbMode == Utils.Enums.DbMode.Single) return Table; return ReadUow.Set<TEntity>(); } }
        /// <summary>
        /// 工作单元
        /// </summary>
        protected new IWriteUnitOfWork WriteUow { get; set; }
        /// <summary>
        /// 读数据工作单元
        /// </summary>
        protected new IReadUnitOfWork ReadUow { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public EfRepository()
        {
            WriteUow = Ioc.GetService<IWriteUnitOfWork>();
            ReadUow = DbMode == Utils.Enums.DbMode.Single ? null : Ioc.GetService<IReadUnitOfWork>();
        }
        /// <summary>
        /// 附加
        /// </summary>
        /// <param name="entity"></param>
        protected virtual EntityEntry<TEntity> AttachIfNot(TEntity entity)
        {
            return Table.Attach(entity);
        }
        /// <summary>
        /// 创建数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override TEntity Create(TEntity entity)
        {
            entity.Init();
            entity.SetCreateUser(UserContext);
            entity.Validation();
            Table.Add(entity);
            return entity;
        }
        /// <summary>
        /// 创建数据集合
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public override IList<TEntity> CreateList(IList<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                entity.Init();
                entity.SetCreateUser(UserContext);
                entity.Validation();
            }
            Table.AddRange(entities);
            return entities;
        }

        /// <summary>
        /// 异步创建对象
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>创建的实体对象</returns>
        public async override Task<TEntity> CreateAsync(TEntity entity)
        {
            entity.Init();
            entity.SetCreateUser(UserContext);
            entity.Validation();
            await Table.AddAsync(entity);
            return await Task.FromResult(entity);
        }

        /// <summary>
        /// 异步创建对象集合
        /// </summary>
        /// <param name="entities">实体对象集合</param>
        /// <returns>创建对象集合</returns>
        public async override Task<IList<TEntity>> CreateListAsync(IList<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                entity.Init();
                entity.SetCreateUser(UserContext);
                entity.Validation();
            }
            await Table.AddRangeAsync(entities);
            return entities;
        }

        /// <summary>
        /// 创建数据并且返回编号
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override TPrimaryKey CreateReturnId(TEntity entity)
        {
            return Create(entity).Id;
        }

        /// <summary>
        /// 获取未跟踪查询对象
        /// </summary>
        public IQueryable<TEntity> FindAsNoTracking()
        {
            return ReadTable.AsNoTracking();
        }

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <returns></returns>
        public override IQueryable<TEntity> Retrieve()
        {
            return ReadTable;
        }
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public override IQueryable<TEntity> Retrieve(Expression<Func<TEntity, bool>> predicate)
        {
            return ReadTable.Where(predicate);
        }
        /// <summary>
        /// 根据编号查询数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override TEntity RetrieveById(TPrimaryKey id)
        {
            return Table.Find(id);
        }
        /// <summary>
        /// 根据编号查询数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public override IList<TEntity> RetrieveByIds(IList<TPrimaryKey> ids)
        {
            return ReadTable.Where(t => ids.Contains(t.Id)).ToList();
        }

        #region 更新
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override TEntity Update(TEntity entity)
        {
            if (entity == null)
                throw new Warning("操作错误:保存的对象已经不存在");
            WriteUow.Entry(entity).State = EntityState.Detached;
            var old = this.RetrieveById(entity.Id);
            if (old.Version != entity.Version)
            {
                if (old.Version == null || entity.Version == null)
                    throw new Warning("当前操作数据不是最新数据,请重新刷新页面再操作！");
                if (old.Version != entity.Version)
                    throw new Warning("当前操作数据不是最新数据,请重新刷新页面再操作！");
            }
            var oldEntry = WriteUow.Entry(old);
            oldEntry.State = EntityState.Detached;
            oldEntry.CurrentValues[nameof(entity.Version)] = entity.Version;
            oldEntry = Table.Attach(old);
            entity.SetUpdateUser(UserContext);
            entity.CreateId = old.CreateId;
            entity.CreateName = old.CreateName;
            entity.CreateTime = old.CreateTime;
            entity.Validation();
            oldEntry.CurrentValues.SetValues(entity);
            return entity;
        }

        /// <summary>
        /// 更新数据(添加更新人信息)
        /// </summary>
        /// <param name="entity">更新实体</param>
        /// <param name="context">用户信息</param>
        /// <returns></returns>
        public override async Task<TEntity> Update(TEntity entity, IUserContext context)
        {
            if (entity == null)
                throw new Warning("操作错误:保存的对象已经不存在");
            WriteUow.Entry(entity).State = EntityState.Detached;
            var old = await this.RetrieveByIdAsync(entity.Id);
            if (old.Version != entity.Version)
            {
                if (old.Version == null || entity.Version == null)
                    throw new Warning("当前操作数据不是最新数据,请重新刷新页面再操作！");
                if (old.Version != entity.Version)
                    throw new Warning("当前操作数据不是最新数据,请重新刷新页面再操作！");
            }
            var oldEntry = WriteUow.Entry(old);
            oldEntry.State = EntityState.Detached;
            oldEntry.CurrentValues[nameof(entity.Version)] = entity.Version;
            oldEntry = Table.Attach(old);
            entity.SetUpdateUser(context);
            entity.CreateId = old.CreateId;
            entity.CreateName = old.CreateName;
            entity.CreateTime = old.CreateTime;
            entity.Validation();
            oldEntry.CurrentValues.SetValues(entity);
            return entity;
        }

        /// <summary>
        /// 部分跟新实体（根据编号）
        /// </summary>
        /// <param name="id"></param>
        /// <param name="update">更新实体</param>
        /// <returns>更新成功条数</returns>
        public override async Task<int> UpdateAsync(TPrimaryKey id, Expression<Func<TEntity, TEntity>> update)
        {
            return await Table.Where(t => t.Id.Equals(id)).UpdateAsync(update);
        }
        /// <summary>
        /// 部分跟新实体（根据条件部分更新,必须写表达式）
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="updateFactory">更新实体</param>
        /// <returns>更新成功条数</returns>
        public override async Task<int> UpdateAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TEntity>> updateFactory)
        {
            return await Table.Where(predicate).UpdateAsync(updateFactory, GetUpdateFactory());
        }
        private Expression<Func<TEntity, TEntity>> GetUpdateFactory()
        {
            return (TEntity) =>
                 new TEntity
                 {
                     LastUpdateId = UserContext.LoginKey,
                     LastUpdateName = UserContext.LoginName,
                     LastUpdateTime = DateTime.Now,
                     Version = Uuid.Next(SequentialGuidType.SequentialAsString)
                 };
        }

        /// <summary>
        /// 部分跟新实体（根据编号）
        /// </summary>
        /// <param name="id"></param>
        /// <param name="update">更新实体</param>
        /// <returns>更新成功条数</returns>
        public override int Update(TPrimaryKey id, Expression<Func<TEntity, TEntity>> update)
        {
            return Table.Where(t => t.Id.Equals(id)).Update(update);
        }
        /// <summary>
        /// 部分跟新实体（根据条件部分更新,必须写表达式）
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="update">更新实体</param>
        /// <returns>更新成功条数</returns>
        public override int Update(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TEntity>> update)
        {
            return Table.Where(predicate).Update(update);
        }

        /// <summary>
        /// 部分跟新实体（根据条件部分更新,必须赋值Id）
        /// </summary>
        /// <param name="entities">更新实体</param>
        /// <returns>更新成功条数</returns>
        public override int Update(IList<TEntity> entities)
        {
            if (entities.Any(t => EqualityComparer<TPrimaryKey>.Default.Equals(t.BatchUpdateId, default)))
                throw new Warning("必须使用唯一识别号进行更新");
            foreach (var item in entities)
            {
                Update(item);
            }
            return entities.Count();
        }

        /// <summary>
        /// 部分跟新实体（根据条件部分更新,必须赋值Id）
        /// </summary>
        /// <param name="entities">更新实体</param>
        /// <returns>更新成功条数</returns>
        public override async Task<int> UpdateAsync(IList<TEntity> entities)
        {
            if (entities.Any(t => EqualityComparer<TPrimaryKey>.Default.Equals(t.BatchUpdateId, default)))
                throw new Warning("必须使用唯一识别号进行更新");
            foreach (var item in entities)
            {
                await UpdateAsync(item);
            }
            return await Task.FromResult(entities.Count());
        }

        #endregion
        /// <summary>
        /// 根据编号删除数据
        /// </summary>
        /// <param name="id"></param>
        public override void Delete(TPrimaryKey id)
        {
            var entity = Table.Local.FirstOrDefault(ent => EqualityComparer<TPrimaryKey>.Default.Equals(ent.Id, id));
            if (entity == null)
            {
                entity = RetrieveById(id);
                if (entity == null)
                {
                    throw new Warning("当前操作数据不是最新数据,请重新刷新页面再操作！");
                }
            }
            Delete(entity);
        }
        /// <summary>
        /// 查询数据条数
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public override int Count(Expression<Func<TEntity, bool>> predicate)
        {
            return ReadTable.Where(predicate).Count();
        }
        /// <summary>
        /// 查询数据条数
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public override Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return ReadTable.Where(predicate).CountAsync();
        }
        /// <summary>
        /// 删除指定数据
        /// </summary>
        /// <param name="entity"></param>
        public override void Delete(TEntity entity)
        {
            entity.VersionInit();
            entity.SetUpdateUser(UserContext);
            if (entity is ISoftDelete)
            {
                ((ISoftDelete)entity).IsDeleted = true;
            }
            else
            {
                Table.Remove(entity);
            }
        }
        /// <summary>
        /// 根据编号删除数据
        /// </summary>
        /// <param name="ids"></param>

        public override void Delete(IList<TPrimaryKey> ids)
        {
            var entities = this.Table.Where(t => ids.Contains(t.Id));
            foreach (var entity in entities)
            {
                entity.VersionInit();
                entity.SetUpdateUser(UserContext);
                if (entity is ISoftDelete)
                {
                    ((ISoftDelete)entity).IsDeleted = true;
                }
                else
                {
                    Table.Remove(entity);
                }
            }
        }
        /// <summary>
        /// 查询所有数据
        /// </summary>
        /// <returns></returns>
        public override IList<TEntity> RetrieveAll()
        {
            return ReadTable.Where(t => true).ToList();
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class EfRepository<TEntity> : EfRepository<TEntity, string> where TEntity : class, IEntity<string>, new()
    {
        /// <summary>
        /// 
        /// </summary>
        public EfRepository()
        {
        }
    }
}
