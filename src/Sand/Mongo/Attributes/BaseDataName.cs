using System;
using System.Collections.Generic;
using System.Text;

namespace Sand.Mongo.Attributes
{
    /// <summary>
    /// 配置数据库
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    public class BaseDataNameAttribute : Attribute
    {

        /// <summary>
        /// Initializes a new instance of the CollectionName class attribute with the desired name.
        /// </summary>
        /// <param name="value">Name of the collection.</param>
        public BaseDataNameAttribute(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                Name = "default";
            Name = value;
        }
        /// <summary>
        /// 数据库名称
        /// </summary>
        /// <value>The name of the collection.</value>
        public virtual string Name { get; private set; }
    }
}
