using Sand.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sand.Cloud.Express.Kdniao
{
    /// <summary>
    /// 快递鸟跟踪订单请求(请求指令类型：1008)
    /// </summary>
    public class KdniaoOrderQueryRequest<TSubscribeData> : KdniaoRequest, IExpressRequest where TSubscribeData : new()
    {
        /// <summary>
        /// 
        /// </summary>
        public KdniaoOrderQueryRequest()
        {
            SubscribeData = new TSubscribeData();
        }
        /// <summary>
        /// （快递鸟跟踪订单请求(请求指令类型：1008)）
        /// </summary>
        public override string RequestType => "1002";
        /// <summary>
        /// 参数订阅时的数据
        /// </summary>
        public TSubscribeData SubscribeData { get; set; }
        /// <summary>
        /// 订阅时候数据
        /// </summary>
        public override string RequestData => Json.ToJson(SubscribeData);
    }

    /// <summary>
    /// 订单查询数据
    /// </summary>
    public class OrderQueryRequest
    {
        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderCode { get; set; }
        /// <summary>
        /// 快递公司编码
        /// </summary>
        public string ShipperCode { get; set; }
        /// <summary>
        /// 物流单号
        /// </summary>
        public string LogisticCode { get; set; }
    }
}
