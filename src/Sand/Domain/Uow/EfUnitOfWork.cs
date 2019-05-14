﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sand.Dependency;
using Microsoft.EntityFrameworkCore.Storage;
using Sand.Maps;
using Sand.Domain.Entities;
using Sand.Data;
using System.Reflection;
using Microsoft.Extensions.DependencyModel;
using System.Linq;
using System.Runtime.Loader;
using Sand.Extensions;
using Microsoft.Extensions.Logging;
using Sand.Filter;
using Sand.EntityFramework.UpdatePlus;

namespace Sand.Domain.Uow
{
    /// <summary>
    /// ef工作单元
    /// </summary>
    public class EfUnitOfWork : DbContext, IUnitOfWork
    {
        private readonly ILog _log;
        private readonly ISqlConfig _sqlConfig;
        /// <summary>
        /// ef工作单元
        /// </summary>
        /// <param name="sqlConfig">sql配置</param>
        public EfUnitOfWork(ISqlConfig sqlConfig)
        {
            _log = Log.Log.GetLog("EfTraceLog");
            _sqlConfig = sqlConfig;
        }
        /// <summary>
        /// 连接
        /// </summary>
        public string ConnectionString { get; set; }
        /// <summary>
        /// 连接
        /// </summary>
        /// <param name="connectionString"></param>
        public EfUnitOfWork(string connectionString)
        {
            //ConnectionString = connectionString;
        }
        /// <summary>
        /// 跟踪号
        /// </summary>
        public string TraceId { get { return DateTimeExtensions.GetUnixTimestamp().ToString(); } }
        /// <summary>
        /// 完成提交
        /// </summary>
        public void Complete()
        {
            this.SaveChanges();
        }

        /// <summary>
        /// 异步完成提交
        /// </summary>
        /// <returns></returns>

        public async Task CompleteAsync()
        {
            await this.SaveChangesAsync();
        }
        /// <summary>
        /// 回滚
        /// </summary>
        public void RollBack()
        {
            //this.RollBack();
        }

        /// <summary>
        /// 回滚
        /// </summary>
        /// <returns></returns>

        public async Task RollBackAsync()
        {
            await Task.FromResult(0);
            // await this.RollBackAsync();
        }
        /// <summary>
        /// 数据连接
        /// </summary>
        public IDbConnection DbConnection { get { return this.Database.GetDbConnection(); } }
        /// <summary>
        /// 构建map
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            foreach (IMapRegister mapper in GetMaps())
                mapper.Register(modelBuilder);
        }
        //public static readonly LoggerFactory MyLoggerFactory = new LoggerFactory(new[] { new EfLoggerProvider(_log, this) });

        /// <summary>
        /// Configure context options<br/>
        /// 配置上下文选项<br/>
        /// </summary>
        /// <param name="optionsBuilder">Options builder</param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            try
            {
                ConnectionString = _sqlConfig.SqlConnectionString;
                if (_sqlConfig.DbType == DbType.Mysql)
                {
                    optionsBuilder.UseMySql(ConnectionString);
                }
                else if (_sqlConfig.DbType == DbType.Mssql)
                {
                    optionsBuilder.UseSqlServer(ConnectionString);
                }
                //var s = @"Data Source=DESKTOP-9G02PJO\MSSQLSERVER3;database=IdentityServer4.EntityFramework-2.0.0;user=sa;password=sa;";
                //optionsBuilder.UseSqlServer(s);
                optionsBuilder.EnableSensitiveDataLogging();
                //optionsBuilder.UseInMemoryDatabase();
                BatchUpdateManager.InMemoryDbContextFactory = () => this;
                //optionsBuilder.UseLoggerFactory(MyLoggerFactory);
                optionsBuilder.UseLoggerFactory(new LoggerFactory(new[] { new EfLoggerProvider(_log, this) }));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取映射配置
        /// </summary>
        private IEnumerable<IMapRegister> GetMaps()
        {
            var result = new List<IMapRegister>();
            foreach (var assembly in GetAssemblies())
                result.AddRange(Helpers.Reflection.GetTypesByInterface<IMapRegister>(assembly));
            return result;
        }

        /// <summary>
        /// 获取定义映射配置的程序集列表
        /// </summary>
        protected virtual Assembly[] GetAssemblies()
        {
            var assemblies = new List<Assembly>();
            var dependencyContext = DependencyContext.Default;
            var libs = dependencyContext.CompileLibraries.Where(lib => !lib.Serviceable && lib.Type != "package");
            foreach (var lib in libs)
            {
                var assembly = AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(lib.Name));
                assemblies.Add(assembly);
            }
            return assemblies.ToArray();
        }
        /// <summary>
        /// 释放
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
