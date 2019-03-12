using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sand.Dependency;
using Sand.Domain.Entities;

namespace Sand.Data
{
    /// <summary>
    /// map构建
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public abstract class BaseEntityMap<TEntity> : IMapRegister where TEntity : class, IEntity
    {
        /// <summary>
        /// 注册map
        /// </summary>
        /// <param name="modelBuilder"></param>
        public void Register(ModelBuilder modelBuilder)
        {
            var builder = modelBuilder.Entity<TEntity>();
            MapTable(builder);
            MapVersion(builder);
            MapProperties(builder);
            MapAssociations(builder);
            MapSoftDelete(builder);
        }

        /// <summary>
        /// 映射表
        /// </summary>
        protected abstract void MapTable(EntityTypeBuilder<TEntity> builder);

        /// <summary>
        /// 映射乐观离线锁
        /// </summary>
        protected virtual void MapVersion(EntityTypeBuilder<TEntity> builder)
        {
            //builder.Property(t => t.Version).IsRowVersion();
        }

        /// <summary>
        /// 映射属性
        /// </summary>
        protected virtual void MapProperties(EntityTypeBuilder<TEntity> builder)
        {
        }

        /// <summary>
        /// 映射导航属性
        /// </summary>
        protected virtual void MapAssociations(EntityTypeBuilder<TEntity> builder)
        {
        }

        /// <summary>
        /// 添加软删除映射
        /// </summary>
        protected virtual void MapSoftDelete(EntityTypeBuilder<TEntity> builder)
        {
        }
    }
}