using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
//using Pomelo.Data.MySql;
using Sand.Domain.Entities;
using Sand.Domain.Query;
using Sand.Result;

namespace Sand.Data
{
    /// <summary>
    /// 数据访问
    /// </summary>
    public interface ISqlQuery : IDisposable
    {
        /// <summary>
        /// 开启数据库
        /// </summary>
        /// <param name="iswrite">是否为写入数据库</param>
        /// <returns></returns>
        ISqlQuery Begin(bool iswrite = false);
        /// <summary>
        /// 开启数据库
        /// </summary>
        /// <param name="iswrite">是否为写入数据库</param>
        /// <returns></returns>
        Task<ISqlQuery> BeginAsync(bool iswrite = false);
        /// <summary>
        /// 初始化where（1=1）
        /// </summary>
        string AppendWhere { get; set; }
        /// <summary>
        /// 数据库连接
        /// </summary>
        DbConnection DbConnection { get; set; }
        /// <summary>
        /// 数据库连接（写入数据库使用）
        /// </summary>
        DbConnection WriteDbConnection { get; set; }
        /// <summary>
        /// 查询数据对象
        /// </summary>
        /// <param name="sql">sql</param>
        /// <param name="param">参数</param>
        /// <returns>结果集合</returns>
        List<TResult> Query<TResult>(string sql, IDictionary<string, object> param = null);
        /// <summary>
        /// 查询数据对象
        /// </summary>
        /// <param name="sql">sql</param>
        /// <param name="param">参数</param>
        /// <returns>结果集合</returns>
        List<TResult> Query<TResult>(string sql, object param = null);
        /// <summary>
        /// 查询数据对象
        /// </summary>
        /// <param name="sql">sql</param>
        /// <param name="param">参数</param>
        /// <returns>结果集合</returns>
        Task<IEnumerable<TResult>> QueryAsync<TResult>(string sql, IDictionary<string, object> param = null);
        /// <summary>
        /// 查询数据对象
        /// </summary>
        /// <param name="sql">sql</param>
        /// <param name="param">参数</param>
        /// <returns>结果集合</returns>
        Task<IEnumerable<TResult>> QueryAsync<TResult>(string sql, object param = null);
        /// <summary>
        /// 执行sql
        /// </summary>
        /// <param name="sql">sql</param>
        /// <param name="param">参数</param>
        /// <returns></returns>
        Task ExecuteAsync(string sql, object param = null);
        /// <summary>
        /// 查询分页数据对象
        /// </summary>
        /// <param name="selectSql">截至到from</param>
        /// <param name="whereSql">where</param>
        /// <param name="query">查询对象,PageIndex起始页,每页数量PageSize,Param参数格式L new {A="1",B="2"}</param>
        /// <returns>结果集合</returns>
        Paged<TResult> QueryPage<TResult>(string selectSql, string whereSql, IQuery query);
        /// <summary>
        /// 查询分页数据对象
        /// </summary>
        /// <param name="selectSql">截至到from</param>
        /// <param name="whereSql">where</param>
        /// <param name="orderbySql">排序</param>
        /// <param name="query">查询对象,PageIndex起始页,每页数量PageSize,Param参数格式L new {A="1",B="2"}</param>
        /// <returns>结果集合</returns>
        Paged<TResult> QueryPage<TResult>(string selectSql, string whereSql, string orderbySql, IQuery query);
        /// <summary>
        /// 查询分页数据对象
        /// </summary>
        /// <param name="selectSql">截至到from(msql截至到from之前)</param>
        /// <param name="whereSql">where</param>
        /// <param name="orderbySql">排序(必须传入)</param>
        /// <param name="query">查询对象,PageIndex起始页,每页数量PageSize,Param参数格式L new {A="1",B="2"}</param>
        /// <returns>结果集合</returns>
        Task<Paged<TResult>> QueryPageAsync<TResult>(string selectSql, string whereSql, string orderbySql, IQuery query);

        /// <summary>
        /// 批量更新数据
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <typeparam name="TKey">Key</typeparam>
        /// <param name="sql">sql</param>
        /// <param name="updateList">更新列表</param>
        void UpdateBatch<T, TKey>(string sql, List<T> updateList) where T : IEntity<TKey>;
        /// <summary>
        /// 批量更新数据
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="sql">sql</param>
        /// <param name="updateList">更新列表</param>
        void UpdateBatch<T>(string sql, List<T> updateList) where T : IEntity<string>;
    }

}
