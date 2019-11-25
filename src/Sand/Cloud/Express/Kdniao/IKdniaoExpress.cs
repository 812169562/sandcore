using Microsoft.Extensions.Configuration;
using RestSharp;
using Sand.Dependency;
using Sand.Extensions;
using Sand.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sand.Cloud.Express.Kdniao
{
    /// <summary>
    /// 快递鸟接口
    /// </summary>
    public interface IKdniaoExpress : IExpress,IDependency
    {
    }
}
