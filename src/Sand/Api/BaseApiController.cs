using AspectCore.Injector;
using Autofac;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Sand.Context;
using Sand.DI;
using Sand.Utils.Enums;
using Sand.Api.Models;
using Sand.Api.Filters;
using Sand.Filter;
using Sand.Exceptions;

namespace Sand.Api
{
    /// <summary>
    /// api基类
    /// </summary>
    [ExceptionHandler]
    [TraceLog]
    public class BaseApiController : Controller
    {
        /// <summary>
        /// api版本号
        /// </summary>
        public const string ApiVersion = "api/[controller]";

        /// <summary>
        /// api版本号
        /// </summary>
        public const string ApiStoragesVersion = "storages/[controller]";
        /// <summary>
        /// 用户信息
        /// </summary>
        public IUserContext UserContext { get; set; }

        /// <summary>
        /// api基类
        /// </summary>
        public BaseApiController()
        {
            UserContext = DefaultIocConfig.Container.Resolve<IUserContext>();
        }

        /// <summary>
        /// 返回成功消息
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="title">标题</param>
        /// <param name="message">消息</param>
        protected virtual IActionResult Success(dynamic data = null, string title = null, string message = null)
        {
            if (message == null)
                message = "成功";
            if (title == null)
                title = "";
            throw new Transform("");
            return new ApiResult(StateCode.Ok, message, data, title);
        }

        /// <summary>
        /// 返回失败消息
        /// </summary>
        /// <param name="message">消息</param>
        protected IActionResult Fail(string message)
        {
            return new ApiResult(StateCode.Fail, message);
        }

        /// <summary>
        /// 返回成功消息(默认不序列化为null的项目)
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="title">标题</param>
        /// <param name="message">消息</param>
        /// <param name="jsonSerializerSettings">序列化规则</param>
        protected virtual IActionResult SuccessNotNull(dynamic data = null, string title = null, string message = null, JsonSerializerSettings jsonSerializerSettings = null)
        {
            return new ApiResult(jsonSerializerSettings, StateCode.Ok, message, data, title);
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="json">json数据</param>
        /// <returns></returns>
        protected virtual T ToJsonObject<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
