using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sand.Domain.Uow
{
    /// <summary>
    /// mysql配置实现
    /// </summary>
    public class MySqlConfig : ISqlConfig
    {
        private readonly IConfiguration _configuration;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        public MySqlConfig(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        /// <summary>
        /// 数据库连接
        /// </summary>
        public string SqlConnectionString { get => _configuration.GetConnectionString("DefaultConnection"); }
        /// <summary>
        /// 
        /// </summary>
        public DbType DbType { get => DbType.Mysql; }
    }
}
