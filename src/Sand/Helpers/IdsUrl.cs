using System;
using System.Collections.Generic;
using System.Text;

namespace Sand.Helpers
{
    /// <summary>
    /// id4url获取
    /// </summary>
    public static class IdsUrl
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string IdsGet(this string url)
        {
            return "";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string IdsData(this string url)
        {
            return "";
        }
    }

    /// <summary>
    /// 认证服务器配置
    /// </summary>
    public class IdsConfig
    {
        /// <summary>
        /// 认证服务器
        /// </summary>
        public string authority { get; set; }
        /// <summary>
        /// 客户端id
        /// </summary>
        public string client_id { get; set; }
        /// <summary>
        /// 认证完成回调页面
        /// </summary>
        public string redirect_uri { get; set; }
        /// <summary>
        /// 相应类型
        /// </summary>
        public string response_type { get; set; }
        /// <summary>
        /// 作用域
        /// </summary>
        public string scope { get; set; }
        /// <summary>
        /// 注销地址
        /// </summary>
        public string post_logout_redirect_uri { get; set; }
    }
}
