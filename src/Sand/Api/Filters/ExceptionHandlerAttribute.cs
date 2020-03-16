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
            var exception = context.Exception.GetOriginalException();
            if (exception is Warning)
            {
                var ex = exception as Warning;
                message = ex.Messages;
                context.Result = new ApiResult(StateCode.Fail, message, null, ((Warning)exception).Code);
            }
            else if (context.Exception.GetOriginalException() is Transform)
            {
                var ex = exception as Transform;
                message = ex.Messages;
                context.Result = new ApiResult(StateCode.Transform, message, ex.Data, ex.Code);
            }
            else if (context.Exception.GetOriginalException() is Pomelo.Data.MySql.MySqlException)
            {
                var ex = exception as Pomelo.Data.MySql.MySqlException;
                message = ex.GetMessage();
                Log.Log.GetLog("Pomelo错误").Error(message);
                context.Result = new ApiResult(StateCode.Fail, "操作超时D.", "");
                var ex2 = context.Exception.GetOriginalException();
                if (ex2 != null)
                {
                    ex2.Submit();
                }
                else
                {
                    context.Exception.Submit();
                }
            }
            else
            {
                message = context.Exception.GetMessage();
                Log.Log.GetLog("SystemErrorTraceLog").Error(message);
                context.Result = new ApiResult(StateCode.Fail, "请求失败,联系管理员", "");
                if (context.Exception.InnerException!=null)
                {
                    context.Exception.InnerException.Submit();
                }
                else
                {
                    var ex = context.Exception.GetOriginalException();
                    if (ex!=null)
                    {
                        ex.Submit();
                    }
                    context.Exception.Submit();
                }
            }
        }
    }

    /// <summary>
    /// 获取最底层的异常类型
    /// </summary>
    public static class ExceptionExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static Exception GetOriginalException(this Exception ex)
        {
            if (ex is Warning)
                return ex;
            if (ex is Transform)
                return ex;
            if (ex is Pomelo.Data.MySql.MySqlException)
                return ex;
            if (ex.InnerException == null) return ex;
            return ex.InnerException.GetOriginalException();
        }
    }
}
