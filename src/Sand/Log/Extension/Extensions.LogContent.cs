﻿using Sand.Log.Abstractions;
using System.Text;

namespace Sand.Log.Extensions
{
    /// <summary>
    /// 日志扩展
    /// </summary>
    public static partial class Extensions {
        /// <summary>
        /// 追加内容
        /// </summary>
        public static void Append( this ILogContent content, StringBuilder result, string value, params object[] args ) {
            if( string.IsNullOrWhiteSpace( value ) )
                return;
            result.Append( "   " );
            if( args == null || args.Length == 0 ) {
                result.Append( value );
                return;
            }
            result.AppendFormat( value, args );
        }

        /// <summary>
        /// 追加内容并换行
        /// </summary>
        public static void AppendLine( this ILogContent content, StringBuilder result, string value, params object[] args ) {
            content.Append( result, value, args );
            result.AppendLine();
        }

        /// <summary>
        /// 设置内容并换行
        /// </summary>
        /// <param name="content">日志内容</param>
        /// <param name="value">值</param>
        /// <param name="args">变量值</param>
        public static void Content( this ILogContent content, string value, params object[] args ) {
            content.AppendLine( content.Content, value, args );
        }
    }
}
