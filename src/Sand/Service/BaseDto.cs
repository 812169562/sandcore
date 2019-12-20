using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ProtoBuf;
using Sand.Extensions;

namespace Sand.Service
{
    /// <summary>
    /// DTO接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IDto<T>
    {
        /// <summary>
        /// 编号
        /// </summary>
        T Id { get; set; }
        /// <summary>
        /// 租户编号
        /// </summary>
        string TenantId { get; set; }
        /// <summary>
        /// 门店编号
        /// </summary>
        string StroeId { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        DateTime? CreateTime { get; set; }
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
        DateTime? LastUpdateTime { get; set; }
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
        bool? IsEnable { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        string Version { get; set; }
        /// <summary>
        /// 是否选择
        /// </summary>
        bool Selected { get; set; }
        /// <summary>
        /// 验证
        /// </summary>
        void Validate();
    }
    /// <summary>
    /// DTO接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [DataContract]
    [Serializable]
    public class BaseDto<T> : IDto<T>
    {
        /// <summary>
        /// 主键
        /// </summary>
        [ProtoMember(1)]
        [DataMember]
        public virtual T Id { get; set; }
        /// <summary>
        /// 诊所编号
        /// </summary>
        [ProtoMember(2)]
        [DataMember]
        public virtual string TenantId { get; set; }
        /// <summary>
        /// 门店编号
        /// </summary>
        [ProtoMember(12)]
        [DataMember]
        public virtual string StroeId { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [ProtoMember(3)]
        [DataMember]
        public virtual DateTime? CreateTime { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [DataMember]
        public virtual string CreateTimeStr => this.CreateTime.ToDateTimeString(true);
        /// <summary>
        /// 创建者
        /// </summary>
        [ProtoMember(4)]
        //[DataMember]
        public virtual string CreateId { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        [ProtoMember(5)]
        //[DataMember]
        public virtual string CreateName { get; set; }
        /// <summary>
        /// 最近更新时间
        /// </summary>
        [ProtoMember(6)]
        //[DataMember]
        public virtual DateTime? LastUpdateTime { get; set; }
        /// <summary>
        /// 最近更新时间
        /// </summary>
        //[DataMember]
        public virtual string LastUpdateTimeStr => this.LastUpdateTime.ToDateTimeString(true);
        /// <summary>
        /// 最近更新者
        /// </summary>
        [ProtoMember(7)]
        //[DataMember]
        public virtual string LastUpdateId { get; set; }
        /// <summary>
        /// 最近更新人
        /// </summary>
        [ProtoMember(8)]
        //[DataMember]
        public virtual string LastUpdateName { get; set; }
        /// <summary>
        /// 是否可用
        /// </summary>
        [ProtoMember(9)]
        [DataMember]
        public virtual bool? IsEnable { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        [ProtoMember(10)]
        [DataMember]
        public virtual string Version { get; set; }
        /// <summary>
        /// 是否选择
        /// </summary>
        [ProtoMember(11)]
        [DataMember]
        public virtual bool Selected { get; set; }

        /// <summary>
        /// 验证
        /// </summary>
        public virtual void Validate()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TBaseDto"></typeparam>
        /// <returns></returns>
        public virtual IList<TBaseDto> CreateList<TBaseDto>() where TBaseDto : BaseDto<T>
        {
            return new List<TBaseDto>();
        }
    }

    /// <summary>
    /// Dto
    /// </summary>
    [DataContract]
    [Serializable]
    public class BaseDto : BaseDto<string>
    {
        /// <summary>
        /// 主键编号
        /// </summary>
        [DataMember]
        public override string Id { get; set; }
    }
}
