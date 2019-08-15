using Sand.Log.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sand.Log.Payment
{
    /// <summary>
    /// 支付日志
    /// </summary>
    public class PaymentLog
    {
        /// <summary>
        /// 订单编号
        /// </summary>
        public string Order { get; set; }
        /// <summary>
        /// 支付类型
        /// </summary>
        public string Type { get; set; }
        public void Write()
        {
            Log.GetLog("wechatpaymentlog")
            .Caption($"{Type}订单公众号支付：" + this.Order)
            .Content()
            .Trace();
        }
    }
}
