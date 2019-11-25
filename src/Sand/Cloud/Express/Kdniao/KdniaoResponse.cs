using System;
using System.Collections.Generic;
using System.Text;

namespace Sand.Cloud.Express.Kdniao
{
    /// <summary>
    /// 快递响应返回接口
    /// </summary>
    public abstract class KdniaoResponse : IExpressResponse
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public string EBusinessID { get; set; }
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
        /// <summary>
        /// 成功与否
        /// </summary>
        public bool Success { get; set; }
        /// <summary>
        /// 失败原因
        /// </summary>
        public string Reason { get; set; }
        /// <summary>
        /// 物流状态：2-在途中,3-签收,4-问题件
        /// </summary>
        public string State { get; set; }
        /// <summary>
        /// 物流状态：2-在途中,3-签收,4-问题件
        /// </summary>
        public string StateName
        {
            get
            {
                if (State == "2")
                {
                    return "在途中";
                }
                if (State == "3")
                {
                    return "签收";
                }
                if (State == "4")
                {
                    return "问题件";
                }
                return "";
            }
        }
    }

    /// <summary>
    /// 订单跟踪响应详细
    /// </summary>
    public class KdniaoResponseInfo {
        /// <summary>
        /// 时间
        /// </summary>
        public string AcceptTime { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string AcceptStation { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

    }
}
