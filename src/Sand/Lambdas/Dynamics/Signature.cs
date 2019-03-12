using System;
using System.Collections.Generic;
using System.Linq;

namespace Sand.Lambdas.Dynamics {
    /// <summary>
    /// 
    /// </summary>
    public class Signature : IEquatable<Signature> {
        /// <summary>
        /// 
        /// </summary>
        public DynamicProperty[] properties;
        /// <summary>
        /// 
        /// </summary>
        public int hashCode;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="properties"></param>
        public Signature( IEnumerable<DynamicProperty> properties ) {
            this.properties = properties.ToArray();
            hashCode = 0;
            foreach ( DynamicProperty p in properties ) {
                hashCode ^= p.Name.GetHashCode() ^ p.Type.GetHashCode();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() {
            return hashCode;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals( object obj ) {
            return obj is Signature ? Equals( (Signature)obj ) : false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals( Signature other ) {
            if ( properties.Length != other.properties.Length ) return false;
            for ( int i = 0; i < properties.Length; i++ ) {
                if ( properties[i].Name != other.properties[i].Name ||
                    properties[i].Type != other.properties[i].Type ) return false;
            }
            return true;
        }
    }
}
