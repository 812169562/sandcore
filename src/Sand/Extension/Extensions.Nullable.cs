using System.Collections.Generic;

namespace Sand.Extension
{
    /// <summary>
    /// 扩展 - 可空类型
    /// </summary>
    public static partial class Nullable
    {
        /// <summary>
        /// 安全返回值
        /// </summary>
        /// <param name="value">可空值</param>
        public static T SafeValue<T>( this T? value ) where T : struct {
            return value ?? default( T );
        }
        /// <summary>
        /// 转换为用分隔符连接的字符串
        /// </summary>
        /// <typeparam name="T">集合元素类型</typeparam>
        /// <param name="list">集合</param>
        /// <param name="quotes">引号，默认不带引号，范例：单引号 "'"</param>
        /// <param name="separator">分隔符，默认使用逗号分隔</param>
        public static string Join<T>(this IEnumerable<T> list, string quotes = "", string separator = ",")
        {
            return Helpers.String.Join(list, quotes, separator);
        }
    }
}
