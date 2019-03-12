﻿using System.Reflection;
using System.Text;

namespace Sand.Lambdas.Dynamics {
    /// <summary>
    /// 
    /// </summary>
    public abstract class DynamicClass {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString() {
            PropertyInfo[] props = this.GetType().GetProperties( BindingFlags.Instance | BindingFlags.Public );
            StringBuilder sb = new StringBuilder();
            sb.Append( "{" );
            for ( int i = 0; i < props.Length; i++ ) {
                if ( i > 0 ) sb.Append( ", " );
                sb.Append( props[i].Name );
                sb.Append( "=" );
                sb.Append( props[i].GetValue( this, null ) );
            }
            sb.Append( "}" );
            return sb.ToString();
        }
    }
}
