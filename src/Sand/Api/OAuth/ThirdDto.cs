using System;
using System.Collections.Generic;
using System.Text;

namespace Sand.Open
{
/// <summary>
/// 第三方返回Dto
/// </summary>
public class ThirdDto
{
    /// <summary>
    /// 开发号
    /// </summary>
    public string Openid { get; set; }
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
