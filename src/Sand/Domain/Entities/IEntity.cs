using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Sand.Context;
using Sand.Helpers;
using Sand.Extensions;
using System.Linq;
using System.ComponentModel.DataAnnotations.Schema;
using Sand.Utils.Enums;

namespace Sand.Domain.Entities
{
    /// <summary>
    /// 实体接口
    /// </summary>
    public interface IEntity
    {
        /// <summary>
        /// 版本号
        /// </summary>
        string Version { get; set; }
    }

    /// <summary>
    /// 实体接口
    /// </summary>
    /// <typeparam name="TPrimaryKey">主键类型</typeparam>
    public interface IEntity<TPrimaryKey> : IEntity
    {
        #region 属性
        /// <summary>
        /// 租户编号
        /// </summary>
        string TenantId { get; set; }
        /// <summary>
        /// 门店号
        /// </summary>
        string StroeId { get; set; }
        /// <summary>
        /// 主键约束
        /// </summary>
        TPrimaryKey Id { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        DateTime CreateTime { get; set; }
        /// <summary>
        /// 创建者
        /// </summary>
        string CreateId { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        string CreateName { get; set; }
        /// <summary>
        /// 最近更新时间
        /// </summary>
        DateTime LastUpdateTime { get; set; }
        /// <summary>
        /// 最近更新者
        /// </summary>
        string LastUpdateId { get; set; }
        /// <summary>
        /// 最近更新人
        /// </summary>
        string LastUpdateName { get; set; }
        /// <summary>
        /// 是否可用
        /// </summary>
        bool IsEnable { get; set; }
        /// <summary>
        /// 是否可用
        /// </summary>
        int Status { get; set; }
        /// <summary>
        /// 批量更新使用编号应该和主键相同
        /// </summary>
        [NotMapped]
        TPrimaryKey BatchUpdateId { get; }
        #endregion

        #region 方法
        /// <summary>
        /// 初始化(包含创建信息)
        /// </summary>
        void Init();
        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="entity"></param>
        void Load(IEntity entity);
        /// <summary>
        /// 设置创建人
        /// </summary>
        /// <param name="userContext"></param>
        void SetCreateUser(IUserContext userContext);
        /// <summary>
        /// 设置更新人
        /// </summary>
        /// <param name="userContext"></param>
        void SetUpdateUser(IUserContext userContext);
        /// <summary>
        /// 验证
        /// </summary>
        void Validation();
        /// <summary>
        /// 创建人信息
        /// </summary>
        void VersionInit();
        #endregion
    }

    /// <summary>
    /// 实体
    /// </summary>
    /// <typeparam name="TPrimaryKey">主键类型</typeparam>
    [Serializable]
    public abstract class Entity<TPrimaryKey> : IEntity<TPrimaryKey>
    {
        /// <summary>
        /// 
        /// </summary>
        public Entity()
        {
        }
        /// <summary>
        /// 创建人信息
        /// </summary>
        public virtual void Init()
        {
            this.VersionInit();
            IsEnable = true;
        }

        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Load(IEntity entity)
        {
        }

        /// <summary>
        /// 设置创建人
        /// </summary>
        /// <param name="userContext">用户上下文</param>
        public void SetCreateUser(IUserContext userContext)
        {
            if (userContext == null)
            {
                userContext = new TestUserContext();
            }
            this.CreateId = this.CreateId.IsEmpty() ? userContext.LoginKey : this.CreateId;
            this.CreateName = this.CreateName.IsEmpty() ? userContext.LoginName : this.CreateName;
            this.CreateTime = DateTime.Now;
            this.StroeId = this.CreateId;
            this.TenantId = this.CreateId;
            this.IsEnable = true;
            this.SetUpdateUser(userContext);
        }
        /// <summary>
        /// 设置更新人
        /// </summary>
        /// <param name="userContext">用户上下文</param>
        public void SetUpdateUser(IUserContext userContext)
        {
            this.LastUpdateId = userContext.LoginKey;
            this.LastUpdateName = userContext.LoginName;
            this.LastUpdateTime = DateTime.Now;
            this.Version = Uuid.Next();
        }

        /// <summary>
        /// 主键
        /// </summary>
        [Required]
        [Key]
        public TPrimaryKey Id { get; set; }
        /// <summary>
        /// 租户编号
        /// </summary>
        [NotMapped]
        public virtual string TenantId { get; set; }
        /// <summary>
        /// 门店号
        /// </summary>
        [NotMapped]
        public virtual string StroeId { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public virtual int Status { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [Required]
        public virtual DateTime CreateTime { get; set; }
        /// <summary>
        /// 创建者
        /// </summary>
        [Required(ErrorMessage = "请登录再操作")]
        [MaxLength(36)]
        public virtual string CreateId { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        [Required(ErrorMessage = "请登录再操作")]
        [MaxLength(50)]
        public virtual string CreateName { get; set; }
        /// <summary>
        /// 最近更新时间
        /// </summary>
        [Required]
        public virtual DateTime LastUpdateTime { get; set; }
        /// <summary>
        /// 最近更新者
        /// </summary>
        [Required(ErrorMessage = "请登录再操作")]
        public virtual string LastUpdateId { get; set; }
        /// <summary>
        /// 最近更新人
        /// </summary>
        [Required(ErrorMessage = "请登录再操作")]
        [MaxLength(50)]
        public virtual string LastUpdateName { get; set; }
        /// <summary>
        /// 是否可用
        /// </summary>
        [Required]
        public virtual bool IsEnable { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        [Required]
        public virtual string Version { get; set; }
        /// <summary>
        /// 批量更新使用编号应该和主键相同
        /// </summary>
        [NotMapped]
        public virtual TPrimaryKey BatchUpdateId { get { return this.Id; } }
        /// <summary>
        /// 相等运算
        /// </summary>
        public override bool Equals(object entity)
        {
            if (!(entity is Entity<TPrimaryKey>))
                return false;
            return this == (Entity<TPrimaryKey>)entity;
        }

        /// <summary>
        /// 获取哈希
        /// </summary>
        public override int GetHashCode()
        {
            return ReferenceEquals(Id, null) ? 0 : Id.GetHashCode();
        }

        /// <summary>
        /// 相等比较
        /// </summary>
        /// <param name="entity1">领域实体1</param>
        /// <param name="entity2">领域实体2</param>
        public static bool operator ==(Entity<TPrimaryKey> entity1, Entity<TPrimaryKey> entity2)
        {
            if ((object)entity1 == null && (object)entity2 == null)
                return true;
            if ((object)entity1 == null || (object)entity2 == null)
                return false;
            if (Equals(entity1.Id, null))
                return false;
            if (entity1.Id.Equals(default(TPrimaryKey)))
                return false;
            return entity1.Id.Equals(entity2.Id);
        }

        /// <summary>
        /// 不相等比较
        /// </summary>
        /// <param name="entity1">领域实体1</param>
        /// <param name="entity2">领域实体2</param>
        public static bool operator !=(Entity<TPrimaryKey> entity1, Entity<TPrimaryKey> entity2)
        {
            return !(entity1 == entity2);
        }

        /// <summary>
        /// 数据验证
        /// </summary>
        public virtual void Validation()
        {
            var collection = DataAnnotationValidation.Validate(this);
            if (!collection.IsValid) throw new Exceptions.Warning(collection.Select(t => t.ErrorMessage).ToList());
        }

        /// <summary>
        /// 初始化版本号乐观锁
        /// </summary>
        public virtual void VersionInit()
        {
            this.Version = Guid.Empty.ToString("N");
        }
    }

    /// <summary>
    /// 实体
    /// </summary>
    [Serializable]
    public abstract class Entity : Entity<string> { }
}
