using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sand.Api.Filters
{
    /// <summary>
    /// 权限认证
    /// </summary>
    public class SandAuthorizeAttribute : AuthorizeAttribute
    {
    }
}
