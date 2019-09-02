using AspectCore.DynamicProxy;
using Natasha;
using Natasha.Operator;
using Sand.Cache;
using Sand.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sand.Filter
{
    /// <summary>
    /// 本地缓存日志
    /// </summary>
    public class LocalCacheAttribute : AbstractInterceptorAttribute
    {

        /// <summary>
        /// 缓存日期
        /// </summary>
        public int Expiration { get; set; }
        /// <summary>
        /// 返回类型(必须带上命名空间)
        /// </summary>
        public string ReturnType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expiration">缓存日期（默认60s）</param>
        /// <param name="returnType">返回类型</param>
        public LocalCacheAttribute(int expiration = 60, string returnType = "string")
        {
            Expiration = expiration;
            ReturnType = returnType;
        }
        private delegate object GetLocalCache(string value, out bool issuccess);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public override async Task Invoke(AspectContext context, AspectDelegate next)
        {
            try
            {
                var parm = "";
                if (context.Parameters.Length > 0)
                {
                    parm = Json.ToJson(context.Parameters.GetValue(0));
                }
                bool issuccess;
                var action = DelegateOperator<GetLocalCache>.CreateUsingStrings($"return Sand.Cache.LocalCache.Get<{ReturnType}>(value,out issuccess);");
                var data = action(context.ServiceMethod.ReflectedType.ToString() + context.ServiceMethod.Name + parm, out issuccess);
                context.ReturnValue = data;
                if (!issuccess)
                {
                    await next(context);
                    LocalCache.Set(context.ServiceMethod.ReflectedType.ToString()+context.ServiceMethod.Name + parm, context.ReturnValue, Expiration);
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
    }
}
