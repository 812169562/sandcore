using Sand.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sand.Cloud.Express.Kdniao
{
    /// <summary>
    /// 快递鸟跟踪订单请求(请求指令类型：1008)
    /// </summary>
    public class KdniaoOrderTraceRequest<TSubscribeData> : KdniaoRequest
    {
        /// <summary>
        /// （快递鸟跟踪订单请求(请求指令类型：1008)）
        /// </summary>
        public override string RequestType => "1008";
        /// <summary>
        /// 参数订阅时的数据
        /// </summary>
        public TSubscribeData SubscribeData { get; set; }
        /// <summary>
        /// 订阅时候数据
        /// </summary>
        public override string RequestData => Json.ToJson(SubscribeData);
    }
}
