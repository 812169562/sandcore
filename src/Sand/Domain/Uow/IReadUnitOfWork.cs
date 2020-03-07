using Sand.Dependency;
using System;
using System.Threading.Tasks;
using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using AspectCore.DynamicProxy.Parameters;
using Sand.Data;

namespace Sand.Domain.Uow
{
    /// <summary>
    /// 工作单元
    /// </summary>
    public interface IReadUnitOfWork : IUnitOfWork,IDependency
    {
    }
}
