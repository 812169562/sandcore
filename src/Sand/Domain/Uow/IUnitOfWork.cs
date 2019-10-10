using Sand.Dependency;
using System;
using System.Threading.Tasks;
using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using AspectCore.DynamicProxy.Parameters;
using Sand.Data;

namespace Sand.Domain.Uow
{
    /// <summary>
    /// 工作单元
    /// </summary>
    public interface IUnitOfWork : IDisposable, IDependency
    {
        /// <summary>
        /// 跟踪号
        /// </summary>
        string TraceId { get; }
        /// <summary>
        /// 提交事务
        /// </summary>
        void Complete();
        /// <summary>
        /// 提交事务
        /// </summary>
       Task CompleteAsync();
        /// <summary>
        /// 回滚事务
        /// </summary>
        void RollBack();
        /// <summary>
        /// 回滚事务
        /// </summary>
        Task RollBackAsync();
        /// <summary>
        /// 连接
        /// </summary>
        IDbConnection DbConnection { get; }
        /// <summary>
        /// 设置
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        /// <summary>
        /// 附加
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        EntityEntry Entry([NotNullAttribute] object entity);
        ///// <summary>
        ///// 附加
        ///// </summary>
        ///// <typeparam name="TEntity"></typeparam>
        ///// <param name="entity"></param>
        ///// <returns></returns>
        //EntityEntry<TEntity> Attach<TEntity>([NotNullAttribute] TEntity entity) where TEntity : class;
        ///// <summary>
        ///// 附加
        ///// </summary>
        ///// <param name="entity"></param>
        ///// <returns></returns>
        //EntityEntry Attach([NotNullAttribute] object entity);
    }

    /// <summary>
    /// 事务属性
    /// </summary>
    public class UnitOfWorkOptions
    {
        /// <summary>
        /// 
        /// </summary>
        public bool? IsTransactional { get; set; }
        /// <summary>
        /// 超时
        /// </summary>

        public TimeSpan? Timeout { get; set; }
        /// <summary>
        /// /
        /// </summary>
        public UnitOfWorkOptions()
        {
            IsTransactional = true;
        }
    }

}
