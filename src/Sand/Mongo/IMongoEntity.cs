using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Sand.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sand.Mongo
{
    /// <summary>
    /// mongo实体接口
    /// </summary>
    public interface IMongoEntity : IEntity<string>
    {
        /// <summary>
        /// mongo数据库唯一识别号
        /// </summary>
        [BsonIgnore]
        ObjectId ObjectId { get; }

        /// <summary>
        /// Mongo内部唯一识别号
        /// </summary>
        string Key { get; set; }
    }
}
