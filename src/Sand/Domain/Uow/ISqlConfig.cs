using Sand.Dependency;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sand.Domain.Uow
{
    /// <summary>
    /// 数据库配置
    /// </summary>
    public interface ISqlConfig: IDependencySingleton
    {
        /// <summary>
        /// 链接字符串
        /// </summary>
        string SqlConnectionString { get; }
        /// <summary>
        /// 数据库类型
        /// </summary>
        DbType DbType { get; }
    }

    /// <summary>
    /// 数据库类型
    /// </summary>
    public enum DbType
    {
        /// <summary>
        /// 
        /// </summary>
        Mysql = 0,
        /// <summary>
        /// 
        /// </summary>
        Mssql = 1,
        /// <summary>
        /// 
        /// </summary>
        Pgsql = 2
    }
}
