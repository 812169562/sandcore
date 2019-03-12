using System;

namespace Sand.Lambdas.Dynamics {
    /// <summary>
    /// 
    /// </summary>
    public class DynamicProperty {
        string name;
        Type type;
        /// <summary>
        /// /
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        public DynamicProperty( string name, Type type ) {
            if ( name == null ) throw new ArgumentNullException( "name" );
            if ( type == null ) throw new ArgumentNullException( "type" );
            this.name = name;
            this.type = type;
        }
        /// <summary>
        /// 
        /// </summary>
        public string Name {
            get { return name; }
        }
        /// <summary>
        /// 
        /// </summary>
        public Type Type {
            get { return type; }
        }
    }
}
