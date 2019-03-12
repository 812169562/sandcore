using System;
using System.Collections.Generic;
using System.Text;
using StaticDotNet.EntityFrameworkCore.ModelConfiguration;
using Microsoft.EntityFrameworkCore;

namespace Sand.Data
{
    /// <summary>
    /// 注册映射配置
    /// </summary>
    public interface IMapRegister
    {
        /// <summary>
        /// 注册映射配置
        /// </summary>
        /// <param name="registrar">配置管理器</param>
        void Register(ModelBuilder registrar);
    }
}
