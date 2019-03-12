using System.Collections.Generic;

namespace Sand.EntityFramework.Shared
{
    /// <summary>
    /// 
    /// </summary>
	public static partial class DbModelPlusExtentions
	{
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="this"></param>
        /// <param name="key"></param>
        /// <returns></returns>
		public static TValue GetValueOrNull<TKey, TValue>(this Dictionary<TKey, TValue> @this, TKey key) where TValue : class
		{
			if (@this.ContainsKey(key))
			{
				return @this[key];
			}

			return null;
		}
	}
}