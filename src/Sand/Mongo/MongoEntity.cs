using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Sand.Context;
using Sand.Domain.Entities;
using Sand.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sand.Mongo
{
    /// <summary>
    /// 实体
    /// </summary>
    public class MongoEntity : IMongoEntity
    {
        /// <summary>
        /// 
        /// </summary>
        public MongoEntity()
        {
            Init();
        }
        /// <summary>
        /// 创建人信息
        /// </summary>
        public virtual void Init()
        {
            this.VersionInit();
            Id = ObjectId.GenerateNewId().ToString();
            Key = Uuid.Next();
            IsEnable = true;
        }

        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Load(IMongoEntity entity)
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
            this.CreateId = this.CreateId ?? userContext.LoginKey;
            this.CreateName = this.CreateId ?? userContext.LoginName;
            this.CreateTime = DateTime.Now;
            this.StroeId = this.CreateId ?? userContext.StroeId;
            this.TenantId = this.TenantId ?? this.CreateId ?? userContext.TenantId;
            this.IsEnable = true;
            this.SetUpdateUser(userContext);
        }
        /// <summary>
        /// 设置更新人
        /// </summary>
        /// <param name="userContext">用户上下文</param>
        public void SetUpdateUser(IUserContext userContext)
        {
            this.LastUpdateId = this.LastUpdateId ?? userContext.LoginKey;
            this.LastUpdateName = this.LastUpdateId ?? userContext.LoginName;
            this.LastUpdateTime = DateTime.Now;
            this.Version = Uuid.Next();
        }

        /// <summary>
        ///Mongo内部唯一识别号
        /// </summary>
        [BsonElement("_id", Order = 0)]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        /// <summary>
        ///主键
        /// </summary>
        [BsonElement(Order = 1)]
        public virtual string Key { get; set; }
        /// <summary>
        /// 租户编号
        /// </summary>
        public virtual string TenantId { get; set; }
        /// <summary>
        /// 门店号
        /// </summary>
        public virtual string StroeId { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        private  DateTime _createdOn { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [BsonElement("_c", Order = 2)]
        //[BsonRepresentation(BsonType.DateTime)]
        //[BsonDateTimeOptions(Kind=DateTimeKind.Local)]
        public virtual DateTime CreateTime
        {
            get
            {
                if (_createdOn == null || _createdOn == DateTime.MinValue)
                    _createdOn = ObjectId.CreationTime;
                return _createdOn;
            }
            set
            {
                _createdOn = value;
            }
        }

        /// <summary>
        /// 创建者
        /// </summary>
        public virtual string CreateId { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public virtual string CreateName { get; set; }
        /// <summary>
        /// 最近更新时间
        /// </summary>
        [BsonRepresentation(BsonType.DateTime)]
        [BsonElement("_m", Order = 2)]
        public virtual DateTime LastUpdateTime { get; set; }
        /// <summary>
        /// 最近更新者
        /// </summary>
        public virtual string LastUpdateId { get; set; }
        /// <summary>
        /// 最近更新人
        /// </summary>
        //[BsonRepresentation(BsonType.DateTime)]
        public virtual string LastUpdateName { get; set; }
        /// <summary>
        /// 是否可用
        /// </summary>
        public virtual bool IsEnable { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public virtual int Status { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public virtual string Version { get; set; }
        /// <summary>
        /// 批量更新使用编号应该和主键相同
        /// </summary>
        [BsonIgnore]
        public virtual string BatchUpdateId { get { return this.Id; } }
        /// <summary>
        /// mongo数据库唯一识别号
        /// </summary>
        [BsonIgnore]
        public ObjectId ObjectId => ObjectId.Parse(Id);

        /// <summary>
        /// 相等运算
        /// </summary>
        public override bool Equals(object entity)
        {
            if (entity == null)
                return false;
            return this.Id == ((IMongoEntity)entity).Id;
        }

        /// <summary>
        /// 获取哈希
        /// </summary>
        public override int GetHashCode()
        {
            return ReferenceEquals(Id, null) ? 0 : Id.GetHashCode();
        }
        /// <summary>
        /// 数据验证
        /// </summary>
        public virtual void Validation()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 初始化版本号乐观锁
        /// </summary>
        public virtual void VersionInit()
        {
            this.Version = Uuid.Next();
        }
        /// <summary>
        /// 加载数据
        /// </summary>
        /// <param name="entity">实体</param>
        public void Load(IEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
