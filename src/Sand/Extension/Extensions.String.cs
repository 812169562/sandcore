using Sand.Utils.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sand.Extensions
{
    /// <summary>
    /// 字符串扩展
    /// </summary>
    public static class StringExtension
    {
        /// <summary>
        /// 判断字符为空
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static bool IsEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }
        /// <summary>
        /// 判断字符不为空
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static bool IsNotEmpty(this string str)
        {
            return !str.IsEmpty();
        }

        /// <summary>
        /// 判断字符为空或者空格
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static bool IsWhiteSpaceEmpty(this string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }

        /// <summary>
        /// 判断字符为空或者空格
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        public static bool IsNotWhiteSpaceEmpty(this string str)
        {
            return !str.IsWhiteSpaceEmpty();
        }

        /// <summary>
        /// 将性别int类型转为汉字
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static string ToSexStr(this int? val)
        {
            return val == 1 ? "男" : "女";
        }

        /// <summary>
        /// 将bool类型转为汉字
        /// </summary>
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string ToIsOrNot(this bool? value, StatusType type = StatusType.Whether)
        {
            switch (type)
            {
                case StatusType.Whether:
                    return value == true ? Whether.Yes.DisplayName() : Whether.No.DisplayName();
                default:
                    return value == true ? SystemStatus.Using.DisplayName() : SystemStatus.Pause.DisplayName();
            }

        }

        /// <summary>
        /// 将1，0转换为是，否
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static string IntToIsOrNot(this int? val)
        {
            switch (val)
            {
                case 1:
                    return "是";
                case 0:
                    return "否";
                default:
                    return null;
            }
        }
    }
}
