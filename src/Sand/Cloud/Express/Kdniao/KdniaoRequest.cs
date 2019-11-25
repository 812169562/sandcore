using System;
using System.Collections.Generic;
using System.Text;

namespace Sand.Cloud.Express.Kdniao
{
    /// <summary>
    /// 快递请求参数
    /// </summary>
    public abstract class KdniaoRequest : IExpressRequest
    {
        /// <summary>
        /// 请求内容需进行URL(utf-8)编码。请求内容JSON格式，须和DataType一致。
        /// </summary>
        public virtual string RequestData { get; set; }
        /// <summary>
        /// 商户ID，请在我的服务页面查看。
        /// </summary>
        public virtual string EBusinessID { get; set; }
        /// <summary>
        /// 请求指令类型
        /// </summary>
        public virtual string RequestType { get; set; }
        /// <summary>
        /// 数据内容签名：把(请求内容(未编码)+AppKey)进行MD5加密，然后Base64编码，最后 进行URL(utf-8)编码。详细过程请查看Demo。
        /// </summary>
        public virtual string DataSign { get; set; }
        /// <summary>
        /// 请求、返回数据类型：2-json；
        /// </summary>
        public virtual string DataType { get; set; }
        /// <summary>
        /// AppKey
        /// </summary>
        public virtual string AppKey { get; set; }
    }
}
