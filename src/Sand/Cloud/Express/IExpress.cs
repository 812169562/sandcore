using Sand.Dependency;
using Sand.DI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sand.Cloud.Express
{
    /// <summary>
    /// 快递接口
    /// </summary>
    public interface IExpress
    {
        /// <summary>
        /// 执行实时查询
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<TResponse> ExecuteRealTime<TRequest, TResponse>(TRequest request) where TRequest:IExpressRequest where TResponse:IExpressResponse;
        /// <summary>
        /// 物流跟踪
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<TResponse> ExecuteTrace<TRequest, TResponse>(TRequest request);
    }
}
