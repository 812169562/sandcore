using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Sand.Events
{
    /// <summary>
    /// 
    /// </summary>
    public static class Utils
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="registry"></param>
        public static void ConcurrentDictionarySafeRegister<TKey, TValue>(TKey key, TValue value, ConcurrentDictionary<TKey, List<TValue>> registry)
        {
            if (registry.TryGetValue(key, out List<TValue> registryItem))
            {
                if (registryItem != null)
                {
                    if (!registryItem.Contains(value))
                    {
                        registry[key].Add(value);
                    }
                }
                else
                {
                    registry[key] = new List<TValue> { value };
                }
            }
            else
            {
                registry.TryAdd(key, new List<TValue> { value });
            }
        }
    }
}
