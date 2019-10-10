using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Autofac;
using Dapper;
using Dapper.Contrib;
using MySql.Data.MySqlClient;
//using Pomelo.Data.MySql;
using Sand.Context;
using Sand.DI;
using Sand.Domain.Entities;
using Sand.Domain.Query;
using Sand.Domain.Uow;
using Sand.Extensions;
using Sand.Filter;
using Sand.Helpers;
using Sand.Log.Extensions;
using Sand.Result;

namespace Sand.Data
{
    /// <summary>
    /// 数据访问
    /// </summary>
    public interface ISqlQuery
    {
        /// <summary>
        /// 开启数据库
        /// </summary>
        ISqlQuery Begin();
        /// <summary>
        /// 初始化where（1=1）
        /// </summary>
        string AppendWhere { get; set; }

        /// <summary>
        /// 数据库连接
        /// </summary>
        IDbConnection DbConnection { get; set; }
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
    /// <summary>
    /// Sql数据库访问
    /// </summary>
    public class SqlQuery : ISqlQuery, IDisposable
    {
        #region 属性变量

        /// <summary>
        /// 初始化where 1=1
        /// </summary>
        public string AppendWhere { get; set; }

        /// <summary>
        /// 数据库连接
        /// </summary>
        public IDbConnection DbConnection { get; set; }
        /// <summary>
        /// 连接配置
        /// </summary>
        private ISqlConfig _sqlConfig { get; set; }
        /// <summary>
        /// 用户上下文
        /// </summary>
        private IUserContext _userContext { get; set; }
        /// <summary>
        /// 日志
        /// </summary>
        private ILog _log { get; set; }

        #endregion 

        #region 构造

        ///// <summary>
        ///// Sql数据库访问
        ///// </summary>
        //public DbAccess() : this(DefaultIocConfig.Container.Resolve<ISqlConfig>())
        //{
        //}
        /// <summary>
        /// Sql数据库访问
        /// </summary>
        /// <param name="sqlConfig">数据库配置</param>
        /// <param name="userContext">用户上下文</param>
        public SqlQuery(ISqlConfig sqlConfig, IUserContext userContext)
        {
            AppendWhere = " where 1=1 ";
            _sqlConfig = sqlConfig;
            _userContext = userContext;
            _log = Log.Log.GetLog("SqlTraceLog");
            //this.CreateDbConnection();
            //this.Open();
        }

        #endregion

        #region  数据库操作

        /// <summary>
        /// 查询数据对象
        /// </summary>
        /// <param name="sql">sql</param>
        /// <param name="param">参数</param>
        /// <returns>结果集合</returns>
        public List<TResult> Query<TResult>(string sql, IDictionary<string, object> param = null)
        {
            this.CreateDbConnection();
            this.Open();
            WriteTraceLog(sql, param);
            var query = DbConnection.Query<TResult>(sql, param).ToList();
            return query;
        }
        /// <summary>
        /// 查询数据对象
        /// </summary>
        /// <param name="sql">sql</param>
        /// <param name="param">参数</param>
        /// <returns>结果集合</returns>
        public List<TResult> Query<TResult>(string sql, object param = null)
        {
            return this.Query<TResult>(sql, ToDictionary(param));
        }
        /// <summary>
        /// 查询数据对象
        /// </summary>
        /// <param name="sql">sql</param>
        /// <param name="param">参数</param>
        /// <returns>结果集合</returns>
        public async Task<IEnumerable<TResult>> QueryAsync<TResult>(string sql, IDictionary<string, object> param = null)
        {
            this.CreateDbConnection();
            this.Open();
            WriteTraceLog(sql, param);
            var query = await DbConnection.QueryAsync<TResult>(sql, param);
            _log.Trace("查询结束");
            return query;
        }

        /// <summary>
        /// 执行完成执行sql
        /// </summary>
        /// <param name="sql">sql</param>
        /// <param name="param">参数</param>
        /// <returns>结果集合</returns>
        public async Task ExecuteAsync(string sql, object param = null)
        {
            this.CreateDbConnection();
            this.Open();
            WriteTraceLog(sql, ToDictionary(param));
            await DbConnection.ExecuteAsync(sql, param);
            _log.Trace("执行完成");
            //return;
        }
        /// <summary>
        /// 查询数据对象
        /// </summary>
        /// <param name="sql">sql</param>
        /// <param name="param">参数</param>
        /// <returns>结果集合</returns>
        public async Task<IEnumerable<TResult>> QueryAsync<TResult>(string sql, object param = null)
        {
            return await this.QueryAsync<TResult>(sql, ToDictionary(param));
        }
        /// <summary>
        /// 查询分页数据对象
        /// </summary>
        /// <param name="selectSql">截至到from</param>
        /// <param name="whereSql">where</param>
        /// <param name="query">查询对象,PageIndex起始页,每页数量PageSize,Param参数格式L new {A="1",B="2"}</param>
        /// <returns>结果集合</returns>
        public Paged<TResult> QueryPage<TResult>(string selectSql, string whereSql, IQuery query)
        {
            return this.QueryPage<TResult>(selectSql, whereSql, "", query);
        }
        /// <summary>
        /// 查询分页数据对象
        /// </summary>
        /// <param name="selectSql">截至到from</param>
        /// <param name="whereSql">where</param>
        /// <param name="orderbySql">排序</param>
        /// <param name="query">查询对象,PageIndex起始页,每页数量PageSize,Param参数格式L new {A="1",B="2"}</param>
        /// <returns>结果集合</returns>
        public Paged<TResult> QueryPage<TResult>(string selectSql, string whereSql, string orderbySql, IQuery query)
        {
            this.CreateDbConnection();
            this.Open();
            Paged<TResult> pagedResult = new Paged<TResult>();
            var parameters = ToDictionary(query.Param);
            var sql = selectSql.Add(whereSql);
            var limit = "limit ?PageIndex,?PageSize";
            var dataSql = sql.Add(orderbySql).Add(limit);
            var countSql = sql.PageCount();
            parameters.Add("PageIndex", (query.PageIndex - 1) * query.PageSize);
            parameters.Add("PageSize", query.PageSize);
            WriteTraceLogPage(dataSql, countSql, parameters);
            var count = DbConnection.QueryFirst<int>(countSql, parameters);
            var data = DbConnection.Query<TResult>(dataSql, parameters);
            pagedResult.PageIndex = query.PageIndex;
            pagedResult.TotalCount = count;
            pagedResult.Result = data.ToList();
            _log.Trace("分页结束：");
            return pagedResult;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="selectSql"></param>
        /// <param name="whereSql"></param>
        /// <param name="orderbySql"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<Paged<TResult>> QueryPageAsync<TResult>(string selectSql, string whereSql, string orderbySql, IQuery query)
        {
            this.CreateDbConnection();
            this.Open();
            Paged<TResult> pagedResult = new Paged<TResult>();
            var parameters = ToDictionary(query.Param);
            var sql = selectSql.Add(whereSql);
            var limit = "limit ?PageIndex,?PageSize";
            var dataSql = sql.Add(orderbySql).Add(limit);
            var countSql = sql.PageCount();
            parameters.Add("PageIndex", (query.PageIndex - 1) * query.PageSize);
            parameters.Add("PageSize", query.PageSize);
            WriteTraceLogPage(dataSql, countSql, parameters);
            var count = await DbConnection.QueryFirstAsync<int>(countSql, parameters);
            var data = await DbConnection.QueryAsync<TResult>(dataSql, parameters);
            pagedResult.PageIndex = query.PageIndex;
            pagedResult.TotalCount = count;
            pagedResult.Result = data.ToList();
            _log.Trace("分页结束：");
            return pagedResult;
        }

        /// <summary>
        /// 批量更新数据
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <typeparam name="TKey">Key</typeparam>
        /// <param name="sql">sql</param>
        /// <param name="updateList">更新列表</param>
        public void UpdateBatch<T, TKey>(string sql, List<T> updateList) where T : IEntity<TKey>
        {
            this.CreateDbConnection();
            this.Open();
            foreach (var item in updateList)
            {
                item.VersionInit();
                item.SetUpdateUser(_userContext);
            }
            DbConnection.Execute(sql, updateList);
        }

        /// <summary>
        /// 批量更新数据
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="sql">sql</param>
        /// <param name="updateList">更新列表</param>
        public void UpdateBatch<T>(string sql, List<T> updateList) where T : IEntity<string>
        {
            this.CreateDbConnection();
            this.Open();
            foreach (var item in updateList)
            {
                item.VersionInit();
                item.SetUpdateUser(_userContext);
            }
            DbConnection.Execute(sql, updateList);
        }
        #endregion

        #region 基础操作

        /// <summary>
        /// 设置数据库连接
        /// </summary>
        protected void CreateDbConnection()
        {
            if (DbConnection == null)
            {
                if (_sqlConfig.DbType == Domain.Uow.DbType.Mysql)
                {
                    DbConnection = new MySqlConnection(_sqlConfig.SqlConnectionString);
                }
                if (_sqlConfig.DbType == Domain.Uow.DbType.Mssql)
                {
                    DbConnection = new SqlConnection(_sqlConfig.SqlConnectionString);
                }
            }
        }

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="sql">Sql语句</param>
        /// <param name="param">参数</param>
        protected void WriteTraceLog(string sql, IDictionary<string, object> param)
        {
            var debugSql = sql;
            foreach (var parameter in param)
            {
                debugSql = debugSql.Replace("?" + parameter.Key, SqlHelper.GetParamLiterals(parameter.Value));
            }
            //_log.Content("请求:" + "浏览器：" + Web.Browser + "  请求地址：" + Web.Url)
            _log.Sql("原始Sql:")
           .Caption("执行sql:")
           .Sql($"{sql}{Environment.NewLine}")
           .Sql("调试Sql")
           .Sql(debugSql)
           .SqlParams(param)
           .Trace();
        }
        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="sql">Sql语句</param>
        /// <param name="countsql">总条数Sql语句</param>
        /// <param name="param">参数</param>
        protected void WriteTraceLogPage(string sql, string countsql, IDictionary<string, object> param)
        {
            var debugSql = sql;
            foreach (var parameter in param)
            {
                debugSql = debugSql.Replace("?" + parameter.Key, SqlHelper.GetParamLiterals(parameter.Value));
            }
            var debugCountSql = countsql;
            foreach (var parameter in param)
            {
                debugCountSql = debugCountSql.Replace("?" + parameter.Key, SqlHelper.GetParamLiterals(parameter.Value));
            }
            _log.Sql("总条数原始Sql:")
           .Caption("执行分页sql:")
           .Sql($"{sql}{Environment.NewLine}")
           .Sql("总条数调试Sql")
           .Sql(debugCountSql)
           .Sql("分页Sql:")
           .Sql($"{sql}{Environment.NewLine}")
           .Sql("分页调试Sql")
           .Sql(debugSql)
           .SqlParams(param)
           .Trace();
        }
        /// <summary>
        /// 打开数据库连接
        /// </summary>
        protected void Open()
        {
            try
            {
                if (DbConnection == null)
                {
                    CreateDbConnection();
                    if (DbConnection.State == ConnectionState.Closed)
                    {
                        DbConnection.Open();
                    }
                }
                if (DbConnection.State == ConnectionState.Closed)
                {
                    DbConnection.Open();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 关闭数据库连接
        /// </summary>

        protected void Close()
        {
            try
            {
                if (DbConnection == null)
                {
                    return;
                }
                if (DbConnection.State == ConnectionState.Open)
                {
                    DbConnection.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 释放
        /// </summary>
        public void Dispose()
        {
            Close();
            DbConnection?.Dispose();
        }

        #endregion

        #region  私有方法

        /// <summary>
        /// 转换为字典类型
        /// </summary>
        /// <param name="param">对象</param>
        /// <returns></returns>
        private static IDictionary<string, object> ToDictionary(object param)
        {
            if (param == null)
                return null;
            var properties = param.GetType().GetProperties();
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            foreach (var item in properties)
            {
                parameters.Add(item.Name, item.GetValue(param));
            }
            return parameters;
        }

        #endregion

        #region Log


        /// <summary>
        /// 获取Sql
        /// </summary>
        private static string GetSql(string sql, string sqlParams)
        {
            var parameters = GetSqlParameters(sqlParams);
            foreach (var parameter in parameters)
            {
                var regex = new System.Text.RegularExpressions.Regex($@"{parameter.Key}\b", RegexOptions.Compiled);
                sql = regex.Replace(sql, parameter.Value);
            }
            return sql;
        }

        /// <summary>
        /// 获取Sql参数字典
        /// </summary>
        /// <param name="sqlParams">Sql参数</param>
        private static IDictionary<string, string> GetSqlParameters(string sqlParams)
        {
            var result = new Dictionary<string, string>();
            var paramName = GetParamName(sqlParams);
            if (string.IsNullOrWhiteSpace(paramName))
                return result;
            string pattern = $@",\s*?{paramName}";
            var parameters = Sand.Helpers.Regex.Split(sqlParams, pattern);
            foreach (var parameter in parameters)
                AddParameter(result, parameter, paramName);
            return result;
        }

        /// <summary>
        /// 获取参数名
        /// </summary>
        private static string GetParamName(string sqlParams)
        {
            string pattern = $@"([@].*?)\d+=";
            return Sand.Helpers.Regex.GetValue(sqlParams, pattern, "$1");
        }

        /// <summary>
        /// 添加参数
        /// </summary>
        private static void AddParameter(Dictionary<string, string> result, string parameter, string paramName)
        {
            string pattern = $@"(?:{paramName})?(\d+)='(.*)'(.*)";
            var values = Helpers.Regex.GetValues(parameter, pattern, new[] { "$1", "$2", "$3" }).Select(t => t.Value).ToList();
            if (values.Count != 3)
                return;
            result.Add($"{paramName}{values[0]}", GetValue(values[1], values[2]));
        }
        /// <summary>
        /// 添加Sql参数
        /// </summary>
        private static void AddSqlParameter(Dictionary<string, string> result, string parameter)
        {
            var items = parameter.Split('=');
            if (items.Length < 2)
                return;
            result.Add(items[0].Trim(), GetValue(parameter, items[1]));
        }

        /// <summary>
        /// 获取值
        /// </summary>
        private static string GetValue(string parameter, string value)
        {
            value = value.SafeString();
            parameter = parameter.SafeString();
            if (string.IsNullOrWhiteSpace(value) && parameter.Contains("DbType = Guid"))
                return "null";
            return $"'{value}'";
        }

        /// <summary>
        /// 添加日志内容
        /// </summary>
        private void AddContent<TState>(TState state)
        {
            if (EfConfig.LogLevel == EfLogLevel.All)
                _log.Content("事件内容：").Content(state.SafeString());
            if (!(state is IEnumerable list))
                return;
            var dictionary = new Dictionary<string, string>();
            foreach (KeyValuePair<string, object> item in list)
                dictionary.Add(item.Key, item.Value.SafeString());
            AddDictionary(dictionary);
        }

        /// <summary>
        /// 添加字典内容
        /// </summary>
        private void AddDictionary(IDictionary<string, string> dictionary)
        {
            AddElapsed(GetValue(dictionary, "elapsed"));
            var sqlParams = GetValue(dictionary, "parameters");
            AddSql(GetValue(dictionary, "commandText"), sqlParams);
            AddSqlParams(sqlParams);
        }

        /// <summary>
        /// 获取值
        /// </summary>
        private string GetValue(IDictionary<string, string> dictionary, string key)
        {
            if (dictionary.ContainsKey(key))
                return dictionary[key];
            return string.Empty;
        }

        /// <summary>
        /// 添加执行时间
        /// </summary>
        private void AddElapsed(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return;
            _log.Content($"执行时间: {value} 毫秒");
        }

        /// <summary>
        /// 添加Sql
        /// </summary>
        private void AddSql(string sql, string sqlParams)
        {
            if (string.IsNullOrWhiteSpace(sql))
                return;
            _log.Sql("原始Sql: ").Sql($"{sql}{Environment.NewLine}");
            sql = sql.Replace("SET NOCOUNT ON;", "");
            _log.Sql($"调试Sql: {GetSql(sql, sqlParams)}{Environment.NewLine}");
        }

        /// <summary>
        /// 添加Sql参数
        /// </summary>
        private void AddSqlParams(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return;
            _log.SqlParams(value);
        }

        /// <summary>
        /// 启动开始
        /// </summary>
        /// <returns></returns>
        public ISqlQuery Begin()
        {
            this.CreateDbConnection();
            this.Open();
            return this;
        }
        #endregion
    }

    /// <summary>
    /// mssqlserver
    /// </summary>
    public class MsSqlQuery : ISqlQuery, IDisposable
    {
        #region 属性变量

        /// <summary>
        /// 初始化where 1=1
        /// </summary>
        public string AppendWhere { get; set; }

        /// <summary>
        /// 数据库连接
        /// </summary>
        public IDbConnection DbConnection { get; set; }
        /// <summary>
        /// 连接配置
        /// </summary>
        private ISqlConfig _sqlConfig { get; set; }
        /// <summary>
        /// 用户上下文
        /// </summary>
        private IUserContext _userContext { get; set; }
        /// <summary>
        /// 日志
        /// </summary>
        private ILog _log { get; set; }

        #endregion 

        #region 构造

        ///// <summary>
        ///// Sql数据库访问
        ///// </summary>
        //public DbAccess() : this(DefaultIocConfig.Container.Resolve<ISqlConfig>())
        //{
        //}
        /// <summary>
        /// Sql数据库访问
        /// </summary>
        /// <param name="sqlConfig">数据库配置</param>
        /// <param name="userContext">用户上下文</param>
        public MsSqlQuery(ISqlConfig sqlConfig, IUserContext userContext)
        {
            AppendWhere = " where 1=1 ";
            _sqlConfig = sqlConfig;
            _userContext = userContext;
            _log = Log.Log.GetLog("SqlTraceLog");
            //this.CreateDbConnection();
            //this.Open();
        }

        #endregion

        #region  数据库操作

        /// <summary>
        /// 查询数据对象
        /// </summary>
        /// <param name="sql">sql</param>
        /// <param name="param">参数</param>
        /// <returns>结果集合</returns>
        public List<TResult> Query<TResult>(string sql, IDictionary<string, object> param = null)
        {
            this.CreateDbConnection();
            this.Open();
            WriteTraceLog(sql, param);
            var query = DbConnection.Query<TResult>(sql, param).ToList();
            return query;
        }
        /// <summary>
        /// 查询数据对象
        /// </summary>
        /// <param name="sql">sql</param>
        /// <param name="param">参数</param>
        /// <returns>结果集合</returns>
        public List<TResult> Query<TResult>(string sql, object param = null)
        {
            return this.Query<TResult>(sql, ToDictionary(param));
        }
        /// <summary>
        /// 查询数据对象
        /// </summary>
        /// <param name="sql">sql</param>
        /// <param name="param">参数</param>
        /// <returns>结果集合</returns>
        public async Task<IEnumerable<TResult>> QueryAsync<TResult>(string sql, IDictionary<string, object> param = null)
        {
            this.CreateDbConnection();
            this.Open();
            WriteTraceLog(sql, param);
            var query = await DbConnection.QueryAsync<TResult>(sql, param);
            _log.Content("sql查询结束");
            return query;
        }

        /// <summary>
        /// 执行完成执行sql
        /// </summary>
        /// <param name="sql">sql</param>
        /// <param name="param">参数</param>
        /// <returns>结果集合</returns>
        public async Task ExecuteAsync(string sql, object param = null)
        {
            this.CreateDbConnection();
            this.Open();
            WriteTraceLog(sql, ToDictionary(param));
            await DbConnection.ExecuteAsync(sql, param);
            _log.Content("sql执行完成");
            //return;
        }
        /// <summary>
        /// 查询数据对象
        /// </summary>
        /// <param name="sql">sql</param>
        /// <param name="param">参数</param>
        /// <returns>结果集合</returns>
        public async Task<IEnumerable<TResult>> QueryAsync<TResult>(string sql, object param = null)
        {
            return await this.QueryAsync<TResult>(sql, ToDictionary(param));
        }
        /// <summary>
        /// 查询分页数据对象
        /// </summary>
        /// <param name="selectSql">截至到from</param>
        /// <param name="whereSql">where</param>
        /// <param name="query">查询对象,PageIndex起始页,每页数量PageSize,Param参数格式L new {A="1",B="2"}</param>
        /// <returns>结果集合</returns>
        public Paged<TResult> QueryPage<TResult>(string selectSql, string whereSql, IQuery query)
        {
            return this.QueryPage<TResult>(selectSql, whereSql, "", query);
        }
        /// <summary>
        /// 查询分页数据对象
        /// </summary>
        /// <param name="selectSql">截至到from</param>
        /// <param name="whereSql">where</param>
        /// <param name="orderbySql">排序</param>
        /// <param name="query">查询对象,PageIndex起始页,每页数量PageSize,Param参数格式L new {A="1",B="2"}</param>
        /// <returns>结果集合</returns>
        public Paged<TResult> QueryPage<TResult>(string selectSql, string whereSql, string orderbySql, IQuery query)
        {
            this.CreateDbConnection();
            this.Open();
            Paged<TResult> pagedResult = new Paged<TResult>();
            var parameters = ToDictionary(query.Param);
            var dataSql = selectSql.Add(", ROW_NUMBER() over (" + orderbySql + ") as NUMBER");
            dataSql = dataSql.Add(whereSql);
            dataSql = "select *   from ( " + dataSql + " ) Temp_M where NUMBER<@end and NUMBER>@start";
            var countSql = whereSql.MsPageCount();
            parameters.Add("start", (query.PageIndex - 1) * query.PageSize);
            parameters.Add("end", query.PageSize * query.PageIndex + 1);
            _log.OperationTime("分页查询开始：" + DateTime.Now);
            WriteTraceLogPage(dataSql, countSql, parameters);
            var count = DbConnection.QueryFirst<int>(countSql, parameters);
            var data = DbConnection.Query<TResult>(dataSql, parameters);
            pagedResult.PageIndex = query.PageIndex;
            pagedResult.TotalCount = count;
            pagedResult.Result = data.ToList();
            _log.Duration("分页查询结束：" + DateTime.Now);
            return pagedResult;
        }


        /// <summary>
        /// 查询分页数据对象
        /// </summary>
        /// <param name="selectSql">截至到from</param>
        /// <param name="whereSql">where</param>
        /// <param name="orderbySql">排序</param>
        /// <param name="query">查询对象,PageIndex起始页,每页数量PageSize,Param参数格式L new {A="1",B="2"}</param>
        /// <returns>结果集合</returns>
        public async Task<Paged<TResult>> QueryPageAsync<TResult>(string selectSql, string whereSql, string orderbySql, IQuery query)
        {
            this.CreateDbConnection();
            this.Open();
            Paged<TResult> pagedResult = new Paged<TResult>();
            var parameters = ToDictionary(query.Param);
            var dataSql = selectSql.Add(", ROW_NUMBER() over (" + orderbySql + ") as NUMBER");
            dataSql = dataSql.Add(whereSql);
            dataSql = "select *   from ( " + dataSql + " ) Temp_M where NUMBER<@end and NUMBER>@start";
            var countSql = whereSql.MsPageCount();
            parameters.Add("start", (query.PageIndex - 1) * query.PageSize);
            parameters.Add("end", query.PageSize * query.PageIndex + 1);
            _log.OperationTime("分页查询开始：" + DateTime.Now);
            WriteTraceLogPage(dataSql, countSql, parameters);
            var count = await DbConnection.QueryFirstAsync<int>(countSql, parameters);
            var data = await DbConnection.QueryAsync<TResult>(dataSql, parameters);
            pagedResult.PageIndex = query.PageIndex;
            pagedResult.TotalCount = count;
            pagedResult.Result = data.ToList();
            _log.Duration("分页查询结束：" + DateTime.Now);
            return pagedResult;
        }

        /// <summary>
        /// 批量更新数据
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <typeparam name="TKey">Key</typeparam>
        /// <param name="sql">sql</param>
        /// <param name="updateList">更新列表</param>
        public void UpdateBatch<T, TKey>(string sql, List<T> updateList) where T : IEntity<TKey>
        {
            this.CreateDbConnection();
            this.Open();
            foreach (var item in updateList)
            {
                item.VersionInit();
                item.SetUpdateUser(_userContext);
            }
            DbConnection.Execute(sql, updateList);
        }

        /// <summary>
        /// 批量更新数据
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="sql">sql</param>
        /// <param name="updateList">更新列表</param>
        public void UpdateBatch<T>(string sql, List<T> updateList) where T : IEntity<string>
        {
            this.CreateDbConnection();
            this.Open();
            foreach (var item in updateList)
            {
                item.VersionInit();
                item.SetUpdateUser(_userContext);
            }
            DbConnection.Execute(sql, updateList);
        }
        #endregion

        #region 基础操作

        /// <summary>
        /// 设置数据库连接
        /// </summary>
        protected void CreateDbConnection()
        {
            if (DbConnection == null)
            {
                if (_sqlConfig.DbType == Domain.Uow.DbType.Mysql)
                {
                    DbConnection = new MySqlConnection(_sqlConfig.SqlConnectionString);
                }
                if (_sqlConfig.DbType == Domain.Uow.DbType.Mssql)
                {
                    DbConnection = new SqlConnection(_sqlConfig.SqlConnectionString);
                }
            }
        }

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="sql">Sql语句</param>
        /// <param name="param">参数</param>
        protected void WriteTraceLog(string sql, IDictionary<string, object> param)
        {
            var debugSql = sql;
            foreach (var parameter in param)
            {
                debugSql = debugSql.Replace("@" + parameter.Key, SqlHelper.GetParamLiterals(parameter.Value));
            }
            //_log.Content("请求:" + "浏览器：" + Web.Browser + "  请求地址：" + Web.Url)
            _log.Sql("原始Sql:")
           .Caption("执行sql:")
           .Sql($"{sql}{Environment.NewLine}")
           .Sql("调试Sql")
           .Sql(debugSql)
           .SqlParams(param)
           .Trace();
        }
        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="sql">Sql语句</param>
        /// <param name="countsql">总条数Sql语句</param>
        /// <param name="param">参数</param>
        protected void WriteTraceLogPage(string sql, string countsql, IDictionary<string, object> param)
        {
            var debugSql = sql;
            foreach (var parameter in param)
            {
                debugSql = debugSql.Replace("@" + parameter.Key, SqlHelper.GetParamLiterals(parameter.Value));
            }

            var debugCountSql = countsql;
            foreach (var parameter in param)
            {
                debugCountSql = debugCountSql.Replace("@" + parameter.Key, SqlHelper.GetParamLiterals(parameter.Value));
            }
            _log.Sql("总条数原始Sql:")
           .Caption("执行分页sql:")
           .Sql($"{sql}{Environment.NewLine}")
           .Sql("总条数调试Sql")
           .Sql(debugCountSql)
           .Sql("分页Sql:")
           .Sql($"{sql}{Environment.NewLine}")
           .Sql("分页调试Sql")
           .Sql(debugSql)
           .SqlParams(param)
           .Trace();
        }
        /// <summary>
        /// 打开数据库连接
        /// </summary>
        protected void Open()
        {
            try
            {
                if (DbConnection == null)
                {
                    CreateDbConnection();
                    if (DbConnection.State == ConnectionState.Closed)
                    {
                        DbConnection.Open();
                    }
                }
                if (DbConnection.State == ConnectionState.Closed)
                {
                    DbConnection.Open();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 关闭数据库连接
        /// </summary>

        protected void Close()
        {
            try
            {
                if (DbConnection == null)
                {
                    return;
                }
                if (DbConnection.State == ConnectionState.Open)
                {
                    DbConnection.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 释放
        /// </summary>
        public void Dispose()
        {
            Close();
            DbConnection?.Dispose();
        }

        #endregion

        #region  私有方法

        /// <summary>
        /// 转换为字典类型
        /// </summary>
        /// <param name="param">对象</param>
        /// <returns></returns>
        private static IDictionary<string, object> ToDictionary(object param)
        {
            if (param == null)
                return null;
            var properties = param.GetType().GetProperties();
            IDictionary<string, object> parameters = new Dictionary<string, object>();
            foreach (var item in properties)
            {
                parameters.Add(item.Name, item.GetValue(param));
            }
            return parameters;
        }

        #endregion

        #region Log


        /// <summary>
        /// 获取Sql
        /// </summary>
        private static string GetSql(string sql, string sqlParams)
        {
            var parameters = GetSqlParameters(sqlParams);
            foreach (var parameter in parameters)
            {
                var regex = new System.Text.RegularExpressions.Regex($@"{parameter.Key}\b", RegexOptions.Compiled);
                sql = regex.Replace(sql, parameter.Value);
            }
            return sql;
        }

        /// <summary>
        /// 获取Sql参数字典
        /// </summary>
        /// <param name="sqlParams">Sql参数</param>
        private static IDictionary<string, string> GetSqlParameters(string sqlParams)
        {
            var result = new Dictionary<string, string>();
            var paramName = GetParamName(sqlParams);
            if (string.IsNullOrWhiteSpace(paramName))
                return result;
            string pattern = $@",\s*@{paramName}";
            var parameters = Sand.Helpers.Regex.Split(sqlParams, pattern);
            foreach (var parameter in parameters)
                AddParameter(result, parameter, paramName);
            return result;
        }

        /// <summary>
        /// 获取参数名
        /// </summary>
        private static string GetParamName(string sqlParams)
        {
            string pattern = $@"([@].*@)\d+=";
            return Sand.Helpers.Regex.GetValue(sqlParams, pattern, "$1");
        }

        /// <summary>
        /// 添加参数
        /// </summary>
        private static void AddParameter(Dictionary<string, string> result, string parameter, string paramName)
        {
            string pattern = $@"(?:{paramName})@(\d+)='(.*)'(.*)";
            var values = Helpers.Regex.GetValues(parameter, pattern, new[] { "$1", "$2", "$3" }).Select(t => t.Value).ToList();
            if (values.Count != 3)
                return;
            result.Add($"{paramName}{values[0]}", GetValue(values[1], values[2]));
        }
        /// <summary>
        /// 添加Sql参数
        /// </summary>
        private static void AddSqlParameter(Dictionary<string, string> result, string parameter)
        {
            var items = parameter.Split('=');
            if (items.Length < 2)
                return;
            result.Add(items[0].Trim(), GetValue(parameter, items[1]));
        }

        /// <summary>
        /// 获取值
        /// </summary>
        private static string GetValue(string parameter, string value)
        {
            value = value.SafeString();
            parameter = parameter.SafeString();
            if (string.IsNullOrWhiteSpace(value) && parameter.Contains("DbType = Guid"))
                return "null";
            return $"'{value}'";
        }

        /// <summary>
        /// 添加日志内容
        /// </summary>
        private void AddContent<TState>(TState state)
        {
            if (EfConfig.LogLevel == EfLogLevel.All)
                _log.Content("事件内容：").Content(state.SafeString());
            if (!(state is IEnumerable list))
                return;
            var dictionary = new Dictionary<string, string>();
            foreach (KeyValuePair<string, object> item in list)
                dictionary.Add(item.Key, item.Value.SafeString());
            AddDictionary(dictionary);
        }

        /// <summary>
        /// 添加字典内容
        /// </summary>
        private void AddDictionary(IDictionary<string, string> dictionary)
        {
            AddElapsed(GetValue(dictionary, "elapsed"));
            var sqlParams = GetValue(dictionary, "parameters");
            AddSql(GetValue(dictionary, "commandText"), sqlParams);
            AddSqlParams(sqlParams);
        }

        /// <summary>
        /// 获取值
        /// </summary>
        private string GetValue(IDictionary<string, string> dictionary, string key)
        {
            if (dictionary.ContainsKey(key))
                return dictionary[key];
            return string.Empty;
        }

        /// <summary>
        /// 添加执行时间
        /// </summary>
        private void AddElapsed(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return;
            _log.Content($"执行时间: {value} 毫秒");
        }

        /// <summary>
        /// 添加Sql
        /// </summary>
        private void AddSql(string sql, string sqlParams)
        {
            if (string.IsNullOrWhiteSpace(sql))
                return;
            _log.Sql("原始Sql: ").Sql($"{sql}{Environment.NewLine}");
            sql = sql.Replace("SET NOCOUNT ON;", "");
            _log.Sql($"调试Sql: {GetSql(sql, sqlParams)}{Environment.NewLine}");
        }

        /// <summary>
        /// 添加Sql参数
        /// </summary>
        private void AddSqlParams(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return;
            _log.SqlParams(value);
        }

        /// <summary>
        /// 启动开始
        /// </summary>
        /// <returns></returns>
        public ISqlQuery Begin()
        {
            this.CreateDbConnection();
            this.Open();
            return this;
        }
        #endregion
    }
}
