using Exceptionless.Extensions;
using Microsoft.AspNetCore.Mvc.Filters;
using Sand.Api.Models;
using Sand.Exceptions;
using Sand.Utils.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sand.Log.Less;

namespace Sand.Api.Filters
{
    /// <summary>
    /// 异常处理过滤器
    /// </summary>
    public class ExceptionHandlerAttribute : ExceptionFilterAttribute
    {
        /// <summary>
        /// 异常处理（系统异常，警告,迁移）
        /// </summary>
        public override void OnException(ExceptionContext context)
        {
            context.ExceptionHandled = true;
            context.HttpContext.Response.StatusCode = 200;
            var message = "";
            if (context.Exception is Warning)
            {
                var exception = context.Exception as Warning;
                message = exception.Messages;
                context.Result = new ApiResult(StateCode.Fail, message, null, exception.Code);
            }
            else if (context.Exception is Transform)
            {
                var exception = context.Exception as Transform;
                message = exception.Messages;
                var ex = context.Exception as Transform;
                context.Result = new ApiResult(StateCode.Transform, message, ex.Data, ex.Code);
            }
            else if (context.Exception.InnerException is Pomelo.Data.MySql.MySqlException)
            {
                message = context.Exception.GetMessage();
                Log.Log.GetLog("Sql错误").Error(message);
                message = "Pomelo错误";
                context.Result = new ApiResult(StateCode.Fail, "操作超时", "");
            }
            else
            {
                message = context.Exception.GetMessage();
                Log.Log.GetLog("SystemErrorTraceLog").Error(message);
                context.Result = new ApiResult(StateCode.Fail, "操作超时", "");
            }
            context.Exception.Submit();
        }
    }
}
