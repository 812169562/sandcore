using Microsoft.Extensions.Caching.Memory;
using Sand.Extensions;
using Sand.Helpers;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Sand.Cache
{
    /// <summary>
    /// 简单本地缓存
    /// </summary>
    public static class LocalCache
    {
        /// <summary>
        /// 
        /// </summary>
        public static ConcurrentDictionary<string, string> Cache { get; private set; }
        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="key"></param>
        public static void Remove(string key)
        {
            Cache.TryRemove(key, out string str);
        }
        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="key">不可带</param>
        /// <param name="issuccess">是否成功获取缓存值</param>
        public static T Get<T>(string key, out bool issuccess)
        {
            issuccess = false;
            if (Cache == null)
            {
                return default;
            }
            string cachejsondata;
            if (!Cache.TryGetValue(key, out cachejsondata))
                return default;
            var data = Json.ToObject<LocalCancheData<T>>(cachejsondata);
            if (data.Date.AddSeconds(data.Expiration) >= DateTime.Now)
            {
                issuccess = true;
                return data.Value;
            }
            Remove(key);
            return default;
        }


        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="key">不可带</param>
        public static T Get<T>(string key)
        {
            if (Cache == null)
                return default;
            string cachejsondata;
            if (!Cache.TryGetValue(key, out cachejsondata))
                return default;
            var data = Json.ToObject<LocalCancheData<T>>(cachejsondata);
            if (data.Date.AddSeconds(data.Expiration) >= DateTime.Now)
                return data.Value;
            Remove(key);
            return default;
        }
        /// <summary>
        /// 加入缓存
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="key">key不可带","</param>
        /// <param name="value"></param>
        /// <param name="expiration"></param>
        /// <param name="secondExpiration"></param>
        public static void Set<T>(string key, T value, int expiration, int secondExpiration = 60)
        {
            if (string.IsNullOrEmpty(key))
                return;
            if (key.Contains("^"))
                return;
            Cache = Cache ?? new ConcurrentDictionary<string, string>();
            var date = DateTime.Now;
            var localCancheData = new LocalCancheData<T>();
            localCancheData.Key = key;
            localCancheData.SecondKey = key + "^" + date.ToMillisecondString();
            localCancheData.SecondExpiration = secondExpiration;
            localCancheData.Value = value;
            localCancheData.Date = date;
            localCancheData.Expiration = expiration;
            localCancheData.SecondExpiration = secondExpiration;
            var newValue = Json.ToJson(localCancheData);
            Cache.AddOrUpdate(key, newValue, (oldkey, oldvalue) => newValue);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    internal class LocalCancheData<T>
    {
        /// <summary>
        /// key
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// 第二次过期Key
        /// </summary>
        public string SecondKey { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        public T Value { get; set; }
        /// <summary>
        /// 加入时间
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// 过期日期
        /// </summary>
        public int Expiration { get; set; }
        /// <summary>
        /// 缓存过期60秒内有效key(传入0该值无效)
        /// </summary>
        public int SecondExpiration { get; set; }
    }
}