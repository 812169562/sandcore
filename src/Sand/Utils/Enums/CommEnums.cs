using EnumsNET;
using System;
using System.ComponentModel;

namespace Sand.Utils.Enums
{
    /// <summary>
    /// 系统表示状态使用还是停用
    /// </summary>
    public enum SystemStatus
    {
        /// <summary>
        /// 使用中
        /// </summary>
        [Description("正常")]
        [DisplayName("正常")]
        Using = 0,
        /// <summary>
        /// 停止使用
        /// </summary>
        [DisplayName("停用")]
        [Description("停用")]
        Pause = 1
    }

    /// <summary>
    /// 状态码
    /// </summary>
    public enum StateCode
    {
        /// <summary>
        /// 成功
        /// </summary>
        [Description("成功")]
        Ok = 1,
        /// <summary>
        /// 失败
        /// </summary>
        [Description("失败")]
        Fail = 2,
        /// <summary>
        /// 跳转
        /// </summary>
        [Description("跳转")]
        Transform = 3
    }

    /// <summary>
    /// 是否
    /// </summary>
    public enum Whether
    {
        /// <summary>
        /// 是
        /// </summary>
        [Description("是")]
        [DisplayName("是")]
        Yes = 0,
        /// <summary>
        /// 否
        /// </summary>
        [DisplayName("否")]
        [Description("否")]
        No = 1
    }

    /// <summary>
    /// 是否
    /// </summary>
    public enum OtherWhether
    {
        /// <summary>
        /// 是
        /// </summary>
        [Description("是")]
        [DisplayName("是")]
        Yes = 1,
        /// <summary>
        /// 否
        /// </summary>
        [DisplayName("否")]
        [Description("否")]
        No = 0
    }

    /// <summary>
    /// 状态类型
    /// </summary>
    public enum StatusType
    {
        /// <summary>
        /// 正常，停用
        /// </summary>
        SystemStatus = 0,
        /// <summary>
        /// 是否
        /// </summary>
        Whether = 1,
    }

    /// <summary>
    /// 性别
    /// </summary>
    public enum SexType
    {
        /// <summary>
        /// 未知
        /// </summary>
        [Description("保密")]
        [DisplayName("保密")]
        Unkown = 0,
        /// <summary>
        /// 男
        /// </summary>
        [Description("男")]
        [DisplayName("男")]
        Man = 1,
        /// <summary>
        /// 女
        /// </summary>
        [Description("女")]
        [DisplayName("男")]
        Woman = 2
    }
}
