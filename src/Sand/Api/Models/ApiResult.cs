using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Sand.Extensions;
using Sand.Utils.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sand.Api.Models
{
    /// <summary>
    /// 返回结果
    /// </summary>
    public class ApiResult : JsonResult
    {
        /// <summary>
        /// 错误代码
        /// </summary>
        public string _errorCode;
        /// <summary>
        /// 状态码
        /// </summary>
        public readonly StateCode _code;
        /// <summary>
        /// 消息
        /// </summary>
        public readonly string _message;
        /// <summary>
        /// 数据
        /// </summary>
        public readonly dynamic _data;
        /// <summary>
        /// 标题
        /// </summary>
        private readonly string _title;
        /// <summary>
        /// 序列化规则
        /// </summary>
        private readonly JsonSerializerSettings _jsonSerializerSettings;

        /// <summary>
        /// 初始化返回结果
        /// </summary>
        /// <param name="code">状态码</param>
        /// <param name="message">消息</param>
        /// <param name="errorCode">错误代码</param>
        /// <param name="data">数据</param>
        /// <param name="title">标题</param>
        public ApiResult(StateCode code, string message, string errorCode, dynamic data = null, string title = "") : base(null)
        {
            _errorCode = errorCode;
            _code = code;
            _message = message;
            _data = data;
            _title = title;
        }


        /// <summary>
        /// 初始化返回结果
        /// </summary>
        /// <param name="jsonSerializerSettings">序列化规则</param>
        /// <param name="code">状态码</param>
        /// <param name="message">消息</param>
        /// <param name="errorCode">错误代码</param>
        /// <param name="data">数据</param>
        /// <param name="title">标题</param>
        public ApiResult(JsonSerializerSettings jsonSerializerSettings, StateCode code, string message, string errorCode, dynamic data = null, string title = "") : base(null)
        {
            _code = code;
            _errorCode = errorCode;
            _message = message;
            _data = data;
            _title = title;
            _jsonSerializerSettings = jsonSerializerSettings;
            if (_jsonSerializerSettings == null)
            {
                _jsonSerializerSettings = new JsonSerializerSettings();
                _jsonSerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                _jsonSerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                _jsonSerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            }
        }

        /// <summary>
        /// 执行结果
        /// </summary>
        public override Task ExecuteResultAsync(ActionContext context)
        {
            this.Value = new
            {
                ErrorCode=_errorCode,
                Code = _code.Value(),
                Message = _message,
                Data = _data,
                Title = _title
            };
            if (_jsonSerializerSettings != null)
            {
                this.SerializerSettings = _jsonSerializerSettings;
            }
            return base.ExecuteResultAsync(context);
        }
    }
}
