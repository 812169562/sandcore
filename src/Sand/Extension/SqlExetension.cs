using Autofac;
using Sand.Context;
using Sand.DI;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sand.Extension
{
    /// <summary>
    /// sql扩展
    /// </summary>
    public static class SqlExetension
    {
        /// <summary>
        /// 过滤条件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="before">原始sql</param>
        /// <param name="filter">过滤条件</param>
        /// <param name="obj">传入值</param>
        /// <returns></returns>

        public static string Filter<T>(this string before, string filter, T obj)
        {
            if (obj == null)
                return before;
            var defaultvalue = default(T);
            if (obj.Equals(defaultvalue))
                return before;
            if (string.IsNullOrEmpty(obj.ToString()))
                return before;
            return before + " And " + filter;
        }

        /// <summary>
        /// 如果成立添加sql
        /// </summary>
        /// <param name="before">原始sql</param>
        /// <param name="fliter">拼接sql</param>
        /// <param name="obj">拼接sql</param>
        /// <returns>拼接后sql</returns>
        public static string WhereIf(this string before, string fliter, bool obj)
        {
            return obj ? before + " And " + fliter : before;
        }

        /// <summary>
        /// 添加租户编号
        /// </summary>
        /// <param name="before"></param>
        /// <param name="fliter"></param>
        /// <returns></returns>
        public static string AddTenant(this string before, string fliter)
        {
            var user = DefaultIocConfig.Container.Resolve<IUserContext>();
            if (user.Type == SystemUserType.SuperAdmin)
            {
                return before;
            }
            return before + " And " + fliter;
        }

        /// <summary>
        /// 添加药房编号
        /// </summary>
        /// <param name="before"></param>
        /// <param name="fliter"></param>
        /// <returns></returns>
        public static string AddStroe(this string before, string fliter)
        {
            var user = DefaultIocConfig.Container.Resolve<IUserContext>();
            if (user.Type == SystemUserType.SuperAdmin || user.Type == SystemUserType.StroeAdmin)
            {
                return before;
            }
            return before + " And " + fliter;
        }
        /// <summary>
        /// 拼接无where的情况
        /// </summary>
        /// <param name="before">原始sql</param>
        /// <returns>拼接后sql</returns>
        public static string AppendWhere(this string before)
        {
            return before + " where 1=1 ";
        }

        /// <summary>
        /// 条件成立排序
        /// </summary>
        /// <param name="before">拼接前</param>
        /// <param name="filter">过滤</param>
        /// <param name="isdesc">是否逆序</param>
        /// <param name="obj">传入值</param>
        /// <returns></returns>
        public static string Order(this string before, string filter, bool isdesc, bool obj)
        {
            var desc = isdesc ? "desc" : "asc";
            if (before.ToUpper().Contains("ORDER BY"))
                return before.FilterOrder(filter, isdesc, obj);
            return obj ? before + " Order By " + filter + " " + desc : before;
        }

        /// <summary>
        /// 条件成立排序
        /// </summary>
        /// <param name="before">拼接前</param>
        /// <param name="filter">过滤</param>
        /// <param name="isdesc">是否逆序</param>
        /// <param name="obj">传入值</param>
        /// <returns></returns>
        public static string FilterOrder(this string before, string filter, bool isdesc, bool obj)
        {
            var desc = isdesc ? "desc" : "asc";
            return obj ? before + "," + filter + " " + desc : before;
        }
        /// <summary>
        /// 拼接sql
        /// </summary>
        /// <param name="str"></param>
        /// <param name="str1"></param>
        /// <returns></returns>
        public static string Add(this string str, string str1)
        {
            return str + " " + str1;
        }

        /// <summary>
        /// 获取分页总条数SQL
        /// </summary>
        /// <param name="str">获取查询条数SQL</param>
        /// <returns></returns>
        public static string PageCount(this string str)
        {
            return "select count(*) as Count  from ( " + str + " ) as PageCount";
        }

        /// <summary>
        /// 返回like
        /// </summary>
        /// <param name="str"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string AppendLike(this string str, int type = 1)
        {
            if (type == 1)
                return "%" + str + "%";
            if (type == 2)
                return "%" + str;
            if (type == 2)
                return str + "%";
            return str;
        }
    }
}
