using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sand.Context;
using Sand.Domain.Entities;
using Sand.Domain.Query;
using Sand.Domain.Repositories;
using Sand.Domain.Uow;
using Sand.Exceptions;
using Sand.Extensions;
using Sand.Helpers;
using Sand.Result;
using Sand.Maps;
using Sand.Lambdas.Dynamics;
using Microsoft.EntityFrameworkCore;
using Sand.Filter;
using Sand.DI;
using Autofac;
using Microsoft.Extensions.Configuration;

namespace Sand.Service
{
    /// <summary>
    /// 服务基类
    /// </summary>
    /// <typeparam name="TDto">传输对象</typeparam>
    /// <typeparam name="TQuery">查询对象</typeparam>
    /// <typeparam name="TEntity">实体对象</typeparam>
    /// <typeparam name="TPrimaryKey">提示主键</typeparam>
    public class BaseService<TDto, TQuery, TEntity, TPrimaryKey> : IService<TDto, TQuery, TEntity, TPrimaryKey> where TDto : BaseDto<TPrimaryKey>, new() where TQuery : IQuery<TEntity, TPrimaryKey> where TEntity : class, IEntity<TPrimaryKey>, new()
    {
        /// <summary>
        /// 用户信息
        /// </summary>
        public IUserContext UserContext { get; set; }
        /// <summary>
        /// 日志信息
        /// </summary>
        public ILogger Logger { get; set; }
        /// <summary>
        /// 工作单元
        /// </summary>
        protected readonly IUnitOfWork Uow;
        /// <summary>
        /// 仓储
        /// </summary>
        protected readonly IRepository<TEntity, TPrimaryKey> Repository;

        /// <summary>
        /// 配置文件
        /// </summary>
        protected readonly IConfiguration Configuration;

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="uow">工作单元</param>
        /// <param name="repository">仓储</param>
        public BaseService(IUnitOfWork uow, IRepository<TEntity, TPrimaryKey> repository)
        {
            Uow = uow;
            Repository = repository;
            UserContext = DefaultIocConfig.Container.Resolve<IUserContext>();
        }

        /// <summary>
        /// 转为Dto
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>dto</returns>
        protected virtual TDto ToDto(TEntity entity)
        {
            entity.CheckNull("数据不能为空");
            var dto = new TDto();
            return entity.MapTo(dto);
        }
        /// <summary>
        /// 转为实体
        /// </summary>
        /// <param name="dto">dto</param>
        /// <returns>实体</returns>
        protected virtual TEntity ToEntity(TDto dto)
        {
            dto.CheckNull("数据不能为空");
            var entity = new TEntity();
            return dto.MapTo(entity);
        }

        /// <summary>
        /// 创建条件表达式
        /// </summary>
        /// <param name="query">查询对象</param>
        /// <returns></returns>
        protected virtual Expression<Func<TEntity, bool>> CreateQuery(TQuery query)
        {
            return Extensions.Extensions.True<TEntity>();
        }

        /// <summary>
        /// 创建条件表达式
        /// </summary>
        /// <param name="query">查询对象</param>
        /// <returns></returns>
        protected virtual void CreateOrder(TQuery query)
        {
            query.OrderBy(t => t.CreateTime);
        }

        /// <summary>
        /// 创建条件表达式
        /// </summary>
        /// <param name="query">查询对象</param>
        /// <returns></returns>
        private string CreateOrderBy(TQuery query)
        {
            CreateOrder(query);
            return query.GetOrderBy();
        }
        /// <summary>
        /// 创建实体对象
        /// </summary>
        /// <param name="dto">数据传输对象</param>
        /// <returns></returns>
        public TDto Create(TDto dto)
        {
            var result = Repository.Create(ToEntity(dto));
            return ToDto(result);
        }

        /// <summary>
        /// 创建实体对象
        /// </summary>
        /// <param name="dto">数据传输对象</param>
        public virtual async Task<TDto> CreateAsync(TDto dto)
        {
            var entity = ToEntity(dto);
            var result = await Repository.CreateAsync(entity);
            return ToDto(result);
        }

        /// <summary>
        /// 更新或者插入
        /// </summary>
        /// <param name="dto">数据传输对象</param>
        /// <returns></returns>

        public virtual TDto CreateOrUpdate(TDto dto)
        {
            TEntity result = null;
            if (dto.Id==null||default(TPrimaryKey).Equals(dto.Id)||dto.Id.ToString().IsEmpty())
                result = Repository.Create(ToEntity(dto));
            else
                result = Repository.Update(ToEntity(dto));
            return ToDto(result);
        }

        /// <summary>
        /// 更新或者插入
        /// </summary>
        /// <param name="dto">数据传输对象</param>
        public virtual async Task<TDto> CreateOrUpdateAsync(TDto dto)
        {
            TEntity result = null;
            if (dto.Id == null || default(TPrimaryKey).Equals(dto.Id) || dto.Id.ToString().IsEmpty())
                result = await Repository.CreateAsync(ToEntity(dto));
            else
                result = await Repository.UpdateAsync(ToEntity(dto));
            return ToDto(result);
        }
        /// <summary>
        /// 创建实体对象集合
        /// </summary>
        /// <param name="dtos">数据传输对象集合</param>
        public virtual IList<TDto> CreateList(IList<TDto> dtos)
        {
            var result = Repository.CreateList(dtos.Select(ToEntity).ToList());
            return result.Select(ToDto).ToList();
        }

        /// <summary>
        /// 创建实体对象集合
        /// </summary>
        /// <param name="dtos">数据传输对象集合</param>
        public virtual async Task<IList<TDto>> CreateListAsync(IList<TDto> dtos)
        {
            var result = await Repository.CreateListAsync(dtos.Select(ToEntity).ToList());
            return result.Select(ToDto).ToList();
        }

        /// <summary>
        /// 创建实体对象返回主键
        /// </summary>
        /// <param name="dto">数据传输对象</param>
        public virtual TPrimaryKey CreateReturnId(TDto dto)
        {
            var result = Repository.CreateReturnId(ToEntity(dto));
            return result;
        }


        /// <summary>
        /// 创建实体对象返回主键
        /// </summary>
        /// <param name="dto">数据传输对象</param>
        public virtual async Task<TPrimaryKey> CreateReturnIdAsync(TDto dto)
        {
            var result = await Repository.CreateReturnIdAsync(ToEntity(dto));
            return result;
        }

        /// <summary>
        /// 查询数据集合
        /// </summary>
        /// <param name="query">查询对象</param>
        public virtual IList<TDto> Retrieve(TQuery query)
        {
            var entity = query.IsTracking ? Repository.Retrieve(CreateQuery(query)).Take(query.Top).OrderByDynamic(CreateOrderBy(query)).AsNoTracking() : Repository.Retrieve(CreateQuery(query)).Take(query.Top).OrderByDynamic(CreateOrderBy(query));
            return entity.Select(ToDto).ToList();
        }

        /// <summary>
        /// 查询数据集合
        /// </summary>
        /// <param name="query">查询对象</param>
        public virtual async Task<IList<TDto>> RetrieveAsync(TQuery query)
        {
            var entity = await Repository.RetrieveAsync(CreateQuery(query));
            entity = entity.OrderByDynamic(CreateOrderBy(query)).Take(query.Top);
            return entity.Select(ToDto).ToList();
        }

        /// <summary>
        /// 根据编号查询数据集合
        /// </summary>
        /// <param name="id">编号</param>
        public virtual TDto RetrieveById(TPrimaryKey id)
        {
            return ToDto(Repository.RetrieveById(id));
        }

        /// <summary>
        /// 根据编号查询数据集合
        /// </summary>
        /// <param name="id">编号</param>
        public virtual async Task<TDto> RetrieveByIdAsync(TPrimaryKey id)
        {
            var entity = await Repository.RetrieveByIdAsync(id);
            return ToDto(entity);
        }

        /// <summary>
        /// 分页查询数据集合
        /// </summary>
        /// <param name="query">分页查询对象</param>
        /// <returns></returns>
        public virtual Paged<TDto> Page(TQuery query)
        {
            var where = CreateQuery(query);
            var data = Repository.Retrieve(where).OrderByDynamic(CreateOrderBy(query)).Skip((query.PageIndex - 1) * query.PageSize).Take(query.PageSize);
            var count = Repository.Count(where);
            Paged<TDto> result = new Paged<TDto>();
            result.Result.AddRange(data.Select(ToDto));
            result.PageIndex = query.PageIndex;
            result.TotalCount = count;
            return result;
        }


        /// <summary>
        /// 分页查询数据集合
        /// </summary>
        /// <param name="query">分页查询对象</param>
        /// <returns></returns>
        public virtual async Task<Paged<TDto>> PageAsync(TQuery query)
        {
            var where = CreateQuery(query);
            var data = await Repository.RetrieveAsync(where);
            var pagedata = data.OrderByDynamic(CreateOrderBy(query)).Skip((query.PageIndex - 1) * query.PageSize).Take(query.PageSize);
            var page = await pagedata.ToListAsync();
            var count = Repository.Count(where);
            Paged<TDto> result = new Paged<TDto>();
            result.Result.AddRange(page.Select(ToDto));
            result.PageIndex = query.PageIndex;
            result.TotalCount = count;
            return result;
        }
        /// <summary>
        ///更新数据
        /// </summary>
        /// <param name="dto"></param>
        public virtual void Update(TDto dto)
        {
            var oldEntity = Repository.Retrieve().Where(t => t.Id.Equals(dto.Id)).AsNoTracking().FirstOrDefault();
            if (oldEntity == null)
                throw new Warning("操作错误:保存的对象已经不存在");
            var newEntity = ToEntity(dto);
            newEntity.CreateName = oldEntity.CreateName;
            newEntity.CreateTime = oldEntity.CreateTime;
            newEntity.CreateId = oldEntity.CreateId;
            CheckVersion(oldEntity.Version, newEntity.Version);
            Repository.Update(newEntity);
        }
        /// <summary>
        /// 更新返回数据
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public virtual TDto UpdateReturn(TDto dto)
        {
            var oldEntity = Repository.Retrieve().Where(t => t.Id.Equals(dto.Id)).AsNoTracking().FirstOrDefault();
            if (oldEntity == null)
                throw new Warning("操作错误:保存的对象已经不存在");
            var newEntity = ToEntity(dto);
            newEntity.CreateName = oldEntity.CreateName;
            newEntity.CreateTime = oldEntity.CreateTime;
            newEntity.CreateId = oldEntity.CreateId;
            CheckVersion(oldEntity.Version, newEntity.Version);
            Repository.Update(newEntity);
            return ToDto(newEntity);
        }
        /// <summary>
        /// 异步更新数据
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public virtual async Task UpdateAsync(TDto dto)
        {
            //var oldEntity = await Repository.Retrieve().Where(t => t.Id.Equals(dto.Id)).AsNoTracking().FirstOrDefaultAsync();
            //if (oldEntity == null)
            //    throw new Warning("操作错误:保存的对象已经不存在");
            var newEntity = ToEntity(dto);
            //newEntity.CreateName = oldEntity.CreateName;
            //newEntity.CreateTime = oldEntity.CreateTime;
            //newEntity.CreateId = oldEntity.CreateId;
            //CheckVersion(oldEntity.Version, newEntity.Version);
            await Repository.UpdateAsync(newEntity);
        }
        /// <summary>
        /// 异步更新并返回原有数据
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public virtual async Task<TDto> UpdateReturnAsync(TDto dto)
        {
            //var oldEntity = await Repository.Retrieve().Where(t => t.Id.Equals(dto.Id)).AsNoTracking().FirstOrDefaultAsync();
            //if (oldEntity == null)
            //    throw new Warning("操作错误:保存的对象已经不存在");
            var newEntity = ToEntity(dto);
            //newEntity.CreateName = oldEntity.CreateName;
            //newEntity.CreateTime = oldEntity.CreateTime;
            //newEntity.CreateId = oldEntity.CreateId;
            //CheckVersion(oldEntity.Version, newEntity.Version);
            await Repository.UpdateAsync(newEntity);
            return ToDto(newEntity);
        }
        /// <summary>
        /// 批量更新集合(按照主键编号)
        /// </summary>
        /// <param name="dtos"></param>
        public virtual int UpdateList(IList<TDto> dtos)
        {
            var entities = dtos.MapTo<List<TEntity>>();
            return Repository.Update(entities);
        }
        /// <summary>
        /// 批量更新集合(按照主键编号)
        /// </summary>
        /// <param name="dtos"></param>
        /// <returns></returns>
        public virtual async Task<int> UpdateListAsync(IList<TDto> dtos)
        {
            var entities = dtos.MapTo<List<TEntity>>();
            return  await Repository.UpdateAsync(entities);
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="ids"></param>
        public virtual void Delete(IList<TPrimaryKey> ids)
        {
            foreach (var id in ids)
                Repository.Delete(id);
        }
        /// <summary>
        /// 异步删除数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public virtual async Task DeleteAsync(IList<TPrimaryKey> ids)
        {
            foreach (var id in ids)
                await Repository.DeleteAsync(id);
        }
        /// <summary>
        /// 删除数据集合
        /// </summary>
        /// <param name="dtos"></param>
        public virtual void Delete(IList<TDto> dtos)
        {
            var entityes = dtos.Select(ToEntity).ToList();
            var entity = Repository.RetrieveByIds(entityes.Select(t => t.Id).ToList());
            foreach (var each in entity)
            {
                Repository.Delete(each);
            }
        }
        /// <summary>
        /// 异步删除数据集合
        /// </summary>
        /// <param name="dtos"></param>
        /// <returns></returns>
        public virtual async Task DeleteAsync(IList<TDto> dtos)
        {
            var beforeEntities = dtos.Select(ToEntity).ToList();
            var entities = await Repository.RetrieveByIdsAsync(beforeEntities.Select(t => t.Id).ToList());
            CheckVersion(dtos.Select(t => t.Version).ToList(), entities.Select(t => t.Version).ToList());
            foreach (var each in entities)
            {
                await Repository.DeleteAsync(each);
            }
        }
        /// <summary>
        /// 验证数据过期
        /// </summary>
        /// <param name="updateVersion">更新前数据</param>
        /// <param name="dbVersion">数据库数据</param>
        private void CheckVersion(List<string> updateVersion, List<string> dbVersion)
        {
            if (updateVersion == null || dbVersion == null)
                throw new Warning("当前操作数据不是最新数据,请重新刷新页面再操作！");
            if (!updateVersion.Any() || !dbVersion.Any())
                throw new Warning("当前操作数据不是最新数据,请重新刷新页面再操作！");
            foreach (var item in updateVersion)
            {
                var count = dbVersion.Where(t => t == item);
                if (count.Count() == 0)
                    throw new Warning("当前操作数据不是最新数据,请重新刷新页面再操作！");
            }
        }

        /// <summary>
        /// 验证数据过期
        /// </summary>
        /// <param name="updateVersion">更新前数据</param>
        /// <param name="dbVersion">数据库数据</param>
        private void CheckVersion(string updateVersion, string dbVersion)
        {
            if (updateVersion == null || dbVersion == null)
                throw new Warning("当前操作数据不是最新数据,请重新刷新页面再操作！");
            if (updateVersion != dbVersion)
                throw new Warning("当前操作数据不是最新数据,请重新刷新页面再操作！");
        }

        /// <summary>
        /// 停用对应集合信息
        /// </summary>
        /// <param name="dtos"></param>
        /// <param name="isEnable"></param>
        /// <returns></returns>
        public virtual async Task StopOrEnableAsync(IList<TDto> dtos, bool isEnable = false)
        {
            var beforeEntities = dtos.Select(ToEntity).ToList();
            var ids = beforeEntities.Select(t => t.Id);
            var entities = await Repository.RetrieveByIdsAsync(ids.ToList());
            CheckVersion(dtos.Select(t => t.Version).ToList(), entities.Select(t => t.Version).ToList());
            await Repository.UpdateAsync(t => ids.Contains(t.Id), p=>new TEntity() { IsEnable = isEnable });
        }
        /// <summary>
        /// 停用对应集合信息
        /// </summary>
        /// <param name="dtos"></param>
        /// <param name="isEnable"></param>
        /// <returns></returns>
        public void StopOrEnable(IList<TDto> dtos, bool isEnable = false) { }

        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="dtos"></param>
        public void Validation(IList<TDto> dtos)
        {

        }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TDto"></typeparam>
    /// <typeparam name="TQuery"></typeparam>
    /// <typeparam name="TEntity"></typeparam>
    public class BaseService<TDto, TQuery, TEntity> : BaseService<TDto, TQuery, TEntity, string>
        where TDto : BaseDto, new()
        where TQuery : IQuery<TEntity, string>
        where TEntity : class, IEntity<string>, new()
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="uow">工作单元</param>
        /// <param name="repository">仓储</param>
        public BaseService(IUnitOfWork uow, IRepository<TEntity> repository) : base(uow, repository)
        {
        }
    }
}
