using Microsoft.Extensions.Configuration;
using RestSharp;
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
    public class KdniaoExpress : IKdniaoExpress
    {
        /// <summary>
        /// 配置
        /// </summary>

        private readonly IConfiguration _configuration;
        /// <summary>
        /// 配置文件
        /// </summary>
        /// <param name="configuration"></param>
        public KdniaoExpress(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        /// <summary>
        /// 执行实时查询
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<TResponse> ExecuteRealTime<TRequest, TResponse>(TRequest input) where TRequest : IExpressRequest where TResponse : IExpressResponse
        {
            var request = input as KdniaoOrderQueryRequest<OrderQueryRequest>;
            request.AppKey = request.AppKey ?? _configuration.GetSection("kdniao:Appkey").Value;
            request.EBusinessID = request.EBusinessID ?? _configuration.GetSection("kdniao:EBusinessId").Value;
            RestClient restclient = new RestClient((_configuration.GetSection("kdniao:Url").Value ?? "http://api.kdniao.com"));
            RestRequest restrequest = new RestRequest(_configuration.GetSection("kdniao:Path").Value ?? "Ebusiness/EbusinessOrderHandle.aspx");
            var requestdata = request.RequestData;
            restrequest.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            restrequest.AddParameter("RequestData", requestdata);
            restrequest.AddParameter("EBusinessID", request.EBusinessID);
            restrequest.AddParameter("RequestType", request.RequestType);
            var dataSign = (requestdata + request.AppKey).GetMd5Base64String();
            restrequest.AddParameter("DataSign", dataSign);
            restrequest.AddParameter("DataType", "2");
            var result = await restclient.ExecutePostTaskAsync(restrequest);
            return Json.ToObject<KdniaoOrderQueryResponse>(result.Content) as TResponse;
        }
        /// <summary>
        /// /
        /// </summary>
        /// <typeparam name="TRequest"></typeparam>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="request"></param>
        /// <returns></returns>
        public Task<TResponse> ExecuteTrace<TRequest, TResponse>(TRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
