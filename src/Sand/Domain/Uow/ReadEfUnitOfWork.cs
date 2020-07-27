using System;
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
using Sand.DI;
using Sand.Context;
using Sand.Helpers;
using System.Collections.Concurrent;

namespace Sand.Domain.Uow
{
    /// <summary>
    /// ef工作单元
    /// </summary>
    public class ReadEfUnitOfWork : DbContext, IReadUnitOfWork
    {
        private readonly ILog _log;
        private readonly ISqlConfig _sqlConfig;
        /// <summary>
        /// ef工作单元
        /// </summary>
        /// <param name="sqlConfig">sql配置</param>
        public ReadEfUnitOfWork(ISqlConfig sqlConfig)
        {
            _log = Log.Log.GetLog("EfTraceLog");
            TraceId = DateTimeExtensions.GetUnixTimestamp().ToString() + "___" + base.GetHashCode();
            //_log.Warn("R工作单元创建" + this.TraceId);
            //NLog.LogManager.GetLogger("Debug").Error("R工作单元创建" + this.TraceId);
            _sqlConfig = sqlConfig;
        }
        /// <summary>
        /// 连接
        /// </summary>
        public string ConnectionString { get; set; }
        /// <summary>
        /// 跟踪号
        /// </summary>
        public string TraceId { get; }
        /// <summary>
        /// 此处不用过时
        /// </summary>
        public IDbConnection DbConnection => null;

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
        }

        /// <summary>
        /// 回滚
        /// </summary>
        /// <returns></returns>

        public async Task RollBackAsync()
        {
            await Task.FromResult(0);
        }
        /// <summary>
        /// 构建map
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (_sqlConfig.ReadSqlConnectionString.IsEmpty())
            {
                return;
            }
            base.OnModelCreating(modelBuilder);
            foreach (IMapRegister mapper in GetMaps())
                mapper.Register(modelBuilder);
        }
        /// <summary>
        /// Configure context options<br/>
        /// 配置上下文选项<br/>
        /// </summary>
        /// <param name="optionsBuilder">Options builder</param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (_sqlConfig.ReadSqlConnectionString.IsEmpty())
                return;
            try
            {
                ConnectionString = _sqlConfig.ReadSqlConnectionString.IsEmpty() ? _sqlConfig.SqlConnectionString : _sqlConfig.ReadSqlConnectionString;
                if (_sqlConfig.DbType == DbType.Mssql)
                {
                    optionsBuilder.UseSqlServer(ConnectionString);
                }
                else if (_sqlConfig.DbType == DbType.Mysql)
                {
                    optionsBuilder.UseMySql(ConnectionString);
                }
               optionsBuilder.EnableSensitiveDataLogging();
               optionsBuilder.UseLoggerFactory(new LoggerFactory(new[] { new EfLoggerProvider(_log, this) }));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private static ConcurrentDictionary<string, IMapRegister> _dic;

        /// <summary>
        /// 获取映射配置
        /// </summary>
        private IEnumerable<IMapRegister> GetMaps()
        {
            _dic = _dic ?? new ConcurrentDictionary<string, IMapRegister>();
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
        /// 
        /// </summary>
        public override void Dispose()
        {
            //NLog.LogManager.GetLogger("Debug").Error("R工作单元释放1_" + this.TraceId);
            base.Dispose();
            //NLog.LogManager.GetLogger("Debug").Error("R工作单元释放2_" + this.TraceId+"\r\n");
        }
    }
}
