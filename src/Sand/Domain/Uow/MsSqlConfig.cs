﻿using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sand.Domain.Uow
{
    /// <summary>
    /// mysql配置实现
    /// </summary>
    public class MsSqlConfig : ISqlConfig
    {
        private readonly IConfiguration _configuration;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        public MsSqlConfig(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        /// <summary>
        /// 数据库连接
        /// </summary>
        public string SqlConnectionString { get => _configuration.GetConnectionString("DefaultConnection"); }

        /// <summary>
        /// 读取连接字符串
        /// </summary>

        public string ReadSqlConnectionString { get => _configuration.GetConnectionString("ReadConnection"); }
        /// <summary>
        /// 
        /// </summary>
        public DbType DbType { get => DbType.Mssql; }
    }
}
