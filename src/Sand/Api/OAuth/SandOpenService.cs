using Microsoft.Extensions.Configuration;
using RestSharp;
using Sand.Dependency;
using Sand.Exceptions;
using Sand.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sand.Open
{
    /// <summary>
    /// 第三方唯一识别码服务
    /// </summary>
    public interface IOpenService : IDependency
    {
        /// <summary>
        /// 获取第三方唯一识别码
        /// </summary>
        /// <param name="type">类型(1微信，2qq)</param>
        /// <param name="code">识别号</param>
        /// <returns></returns>
        Task<ThirdDto> GetOpenId(string type, string code);
    }
    /// <summary>
    /// 第卅方认证获取
    /// </summary>
    public class SandOpenService : IOpenService
    {
        /// <summary>
        /// 系统配置
        /// </summary>
        private readonly IConfiguration _configuration;
        /// <summary>
        /// 系统配置
        /// </summary>
        /// <param name="configuration">系统配置</param>
        public SandOpenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        /// <summary>
        /// 获取第三方唯一识别码
        /// </summary>
        /// <param name="type">类型(1微信，2qq)</param>
        /// <param name="code">识别号</param>
        /// <returns></returns>
        public async Task<ThirdDto> GetOpenId(string type, string code)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                throw new Warning("未取得授权");
            }
            if (string.IsNullOrWhiteSpace(type))
            {
                throw new Warning("未知的授权类型");
            }
            var url = _configuration.GetSection("Wechat:url").Value;
            var path = _configuration.GetSection("Wechat:path").Value;
            RestClient client = new RestClient(url);
            var request = new RestRequest(string.Format(path, code));
            var response = await client.ExecuteTaskAsync(request);
            return Json.ToObject<ThirdDto>(response.Content);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public enum OAuthType
    {
        /// <summary>
        /// 无
        /// </summary>
        None = 0,
        /// <summary>
        /// 微信
        /// </summary>
        Wechat = 1
    }
}
