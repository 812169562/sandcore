using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sand.Api.Filters
{
    /// <summary>
    /// Swagger拦截器
    /// </summary>
    public class SwaggerOperationFilter<TAuthorize>: IOperationFilter where TAuthorize : AuthorizeAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="context"></param>
        public void Apply(Operation operation, OperationFilterContext context)
        {
            // Policy names map to scopes
            var requiredScopes = context.MethodInfo.ReflectedType
                .GetCustomAttributes(true)
                .OfType<TAuthorize>()
                .Select(attr => attr.Policy)
                .Distinct();

            var requiredScopes2 = context.MethodInfo
               .GetCustomAttributes(true)
               .OfType<TAuthorize>()
               .Select(attr => attr.Policy)
               .Distinct();

            if (requiredScopes.Any())
            {
                operation.Responses.Add("401", new Response { Description = "Unauthorized" });
                operation.Responses.Add("403", new Response { Description = "Forbidden" });

                operation.Security = new List<IDictionary<string, IEnumerable<string>>>
                {
                    new Dictionary<string, IEnumerable<string>>
                {
                    { "oauth2", requiredScopes }
                }
                };
            }
            if (requiredScopes2.Any())
            {
                operation.Responses.Add("401", new Response { Description = "Unauthorized" });
                operation.Responses.Add("403", new Response { Description = "Forbidden" });

                operation.Security = new List<IDictionary<string, IEnumerable<string>>>
                {
                    new Dictionary<string, IEnumerable<string>>
                {
                    { "oauth2", requiredScopes }
                }
                };
            }
        }
    }


    /// <summary>
    /// Swagger拦截器
    /// </summary>
    public class SwaggerOperationFilter : SwaggerOperationFilter<SandAuthorizeAttribute>, IOperationFilter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="context"></param>
        public void Apply(Operation operation, OperationFilterContext context)
        {
            // Policy names map to scopes
            var requiredScopes = context.MethodInfo.ReflectedType
                .GetCustomAttributes(true)
                .OfType<SandAuthorizeAttribute>()
                .Select(attr => attr.Policy)
                .Distinct();

            var requiredScopes2 = context.MethodInfo
               .GetCustomAttributes(true)
               .OfType<SandAuthorizeAttribute>()
               .Select(attr => attr.Policy)
               .Distinct();

            if (requiredScopes.Any())
            {
                operation.Responses.Add("401", new Response { Description = "Unauthorized" });
                operation.Responses.Add("403", new Response { Description = "Forbidden" });

                operation.Security = new List<IDictionary<string, IEnumerable<string>>>
                {
                    new Dictionary<string, IEnumerable<string>>
                {
                    { "oauth2", requiredScopes }
                }
                };
            }
            if (requiredScopes2.Any())
            {
                operation.Responses.Add("401", new Response { Description = "Unauthorized" });
                operation.Responses.Add("403", new Response { Description = "Forbidden" });

                operation.Security = new List<IDictionary<string, IEnumerable<string>>>
                {
                    new Dictionary<string, IEnumerable<string>>
                {
                    { "oauth2", requiredScopes }
                }
                };
            }
        }
    }
}
