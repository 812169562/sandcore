using Sand.Dependency;
using Sand.Filter;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Sand.Result;
using Sand.Domain.Query;
using Sand.Domain.Entities;
using Sand.Cache;

namespace Sand.Service
{
    /// <summary>
    /// 服务接口
    /// </summary>
    public interface IService : IDependency
    {
    }

    /// <summary>
    /// 服务接口
    /// </summary>
    /// <typeparam name="TDto">数据传输对象</typeparam>
    /// <typeparam name="TQuery">查询对象</typeparam>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TPrimaryKey"></typeparam>
    public interface IService<TDto, TQuery, TEntity, TPrimaryKey> : IService where TDto : IDto<TPrimaryKey> where TQuery : IQuery<TEntity, TPrimaryKey> where TEntity : class, IEntity<TPrimaryKey>
    {
        /// <summary>
        /// 创建对象
        /// </summary>
        /// <param name="dto">数据传输对象</param>
        [Uow]
        TDto Create(TDto dto);
        /// <summary>
        /// 创建实体
        /// </summary>
        /// <param name="dto">实体</param>
        [UowAsync]
        Task<TDto> CreateAsync(TDto dto);
        /// <summary>
        /// 创建实体集
        /// </summary>
        /// <param name="dtos">实体集</param>
        /// <returns>创建后的实体集</returns>
        [Uow]
        IList<TDto> CreateList(IList<TDto> dtos);
        /// <summary>
        /// 创建实体集
        /// </summary>
        /// <param name="dtos">实体集</param>
        /// <returns>创建后的实体集</returns>
        [UowAsync]
        Task<IList<TDto>> CreateListAsync(IList<TDto> dtos);
        /// <summary>
        /// 创建实体返回编号
        /// </summary>
        /// <param name="dto">实体</param>
        [Uow]
        TPrimaryKey CreateReturnId(TDto dto);
        /// <summary>
        /// 创建实体返回编号
        /// </summary>
        /// <param name="dto">实体</param>
        [UowAsync]
        Task<TPrimaryKey> CreateReturnIdAsync(TDto dto);
        /// <summary>
        /// 查询集合
        /// </summary>
        /// <param name="query">查询对象</param>
        /// <returns></returns>
        IList<TDto> Retrieve(TQuery query);
        /// <summary>
        /// 查询集合
        /// </summary>
        /// <param name="query">查询对象</param>
        /// <returns></returns>
        Task<IList<TDto>> RetrieveAsync(TQuery query);
        /// <summary>
        /// 根据编号查询实体
        /// </summary>
        /// <param name="id">实体编号</param>
        /// <returns>查询实体集</returns>
        TDto RetrieveById(TPrimaryKey id);
        /// <summary>
        /// 根据编号查询实体
        /// </summary>
        /// <param name="id">实体编号</param>
        /// <returns>查询实体集</returns>
        Task<TDto> RetrieveByIdAsync(TPrimaryKey id);
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="query">查询对象</param>
        /// <returns></returns>
        Paged<TDto> Page(TQuery query);
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="query">查询对象</param>
        /// <returns></returns>
        Task<Paged<TDto>> PageAsync(TQuery query);
        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="dto">数据传输对象</param>
        [Uow]
        void Update(TDto dto);
        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="dto">数据传输对象</param>
        [Uow]
        TDto UpdateReturn(TDto dto);
        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="dto">数据传输对象</param>
        /// <returns>更新实体</returns>
        [Uow]
        Task UpdateAsync(TDto dto);
        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="dto">数据传输对象</param>
        [UowAsync]
        Task<TDto> UpdateReturnAsync(TDto dto);
        /// <summary>
        /// 更新实体集合
        /// </summary>
        /// <param name="dtos">数据传输对象集合</param>
        [Uow]
        int UpdateList(IList<TDto> dtos);
        /// <summary>
        /// 更新实体集合
        /// </summary>
        /// <param name="dtos">数据传输对象集合</param>
        [UowAsync]
        Task<int> UpdateListAsync(IList<TDto> dtos);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids">编号集合</param>
        [Uow]
        void Delete(IList<TPrimaryKey> ids);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids">编号集合</param>
        [UowAsync]
        Task DeleteAsync(IList<TPrimaryKey> ids);
        /// <summary>
        /// 删除集合
        /// </summary>
        /// <param name="dtos">编号集合</param>
        [Uow]
        void Delete(IList<TDto> dtos);
        /// <summary>
        /// 删除集合
        /// </summary>
        /// <param name="dtos">编号集合</param>
        [UowAsync]
        Task DeleteAsync(IList<TDto> dtos);
        /// <summary>
        /// 新增或者更新
        /// </summary>
        /// <param name="dto">实体</param>
        [Uow]
        TDto CreateOrUpdate(TDto dto);
        /// <summary>
        /// 新增或者更新
        /// </summary>
        /// <param name="dto">实体</param>
        [UowAsync]
        Task<TDto> CreateOrUpdateAsync(TDto dto);
        /// <summary>
        /// 停用对应集合信息
        /// </summary>
        /// <param name="dtos"></param>
        /// <param name="isEnable"></param>
        /// <returns></returns>
        [UowAsync]
        Task StopOrEnableAsync(IList<TDto> dtos,bool isEnable= false);
        /// <summary>
        /// 停用对应集合信息
        /// </summary>
        /// <param name="dtos"></param>
        /// <param name="isEnable"></param>
        /// <returns></returns>
        [Uow]
        void StopOrEnable(IList<TDto> dtos, bool isEnable = false);
        /// <summary>
        /// 验证
        /// </summary>
        void Validation(IList<TDto> dtos);
    }

    /// <summary>
    /// 服务接口
    /// </summary>
    /// <typeparam name="TDto">Dto</typeparam>
    /// <typeparam name="TQuery">Query</typeparam>
    /// <typeparam name="TEntity">Entity</typeparam>
    public interface IService<TDto, TQuery, TEntity> : IService<TDto, TQuery, TEntity, string> where TDto : IDto<string> where TQuery : IQuery<TEntity, string> where TEntity : class, IEntity<string>
    {

    }
}
