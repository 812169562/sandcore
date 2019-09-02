using System;
using System.Collections.Generic;
using System.Text;

namespace Sand.Maps
{
    /// <summary>
    /// 复杂类型映射(实体)
    /// </summary>
    public class MapAttribute : Attribute
    {
        /// <summary>
        /// 映射名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">当该项仅有一个属性使用时可为空</param>
        public MapAttribute(string name="")
        {
            Name = name;
        }
    }
}
