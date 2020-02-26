using Sand.Dependency;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sand.Domain.Uow
{
    /// <summary>
    /// 写入数据库上下文
    /// </summary>
    public interface IWriteUnitOfWork : IUnitOfWork, IDependency
    {
    }
}
