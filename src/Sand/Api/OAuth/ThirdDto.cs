using System;
using System.Collections.Generic;
using System.Text;

namespace Sand.Open
{
    /// <summary>
    /// 第三方返回Dto
    /// </summary>
    [Serializable]
    public class ThirdDto
    {
        /// <summary>
        /// 开发号
        /// </summary>
        public string Openid { get; set; }
        /// <summary>
        /// 用户统一标识。针对一个微信开放平台帐号下的应用，同一用户的unionid是唯一的。
        /// </summary>
        public string unionid { get; set; }
        /// <summary>
        /// 错误编号
        /// </summary>
        public string errcode { get; set; }
        /// <summary>
        /// 错误信息
        /// </summary>
        public string errmsg { get; set; }
    }
}
