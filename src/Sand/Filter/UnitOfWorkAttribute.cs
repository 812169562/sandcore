using AspectCore.DynamicProxy;
using AspectCore.Injector;
using Sand.Domain.Uow;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Sand.Log.Less;
using Sand.Exceptions;

namespace Sand.Filter
{
    /// <summary>
    /// 事务aop
    /// </summary>
    public class UowAttribute : AbstractInterceptorAttribute
    {
        /// <summary>
        /// 事务aop
        /// </summary>
        //[FromContainer]
        private IWriteUnitOfWork _uow;

        /// <summary>
        /// 事务aop
        /// </summary>
        //[FromContainer]
        private ILog _log;
        /// <summary>
        /// 事务aop
        /// </summary>
        public UowAttribute()
        {

        }
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public async override Task Invoke(AspectContext context, AspectDelegate next)
        {
            try
            {
                _uow = context.ServiceProvider.GetService<IWriteUnitOfWork>();
                await next(context);
                _uow.Complete();
            }
            catch (System.Exception ex)
            {
                if (ex is Warning)
                {
                    var exception = ex as Warning;
                    throw exception;
                }
                if (ex is Transform)
                {
                    var exception = ex as Transform;
                    throw exception;
                }
                if (ex.InnerException is DbUpdateException)
                {
                    var dbex = ex.InnerException as DbUpdateException;
                    _log = Log.Log.GetLog("UowAsync");
                    _log.Error(dbex.InnerException.Message);
                }
                else
                {
                    _log = Log.Log.GetLog("UowAsync");
                    ex.Submit();
                    if (ex.InnerException != null)
                    {
                        ex.InnerException.Submit();
                        if (ex.InnerException.InnerException != null)
                        {
                            ex.InnerException.InnerException.Submit();
                        }
                        throw ex.InnerException;
                    }
                }
                throw ex;
            }
        }
    }

    /// <summary>
    /// 事务aop
    /// </summary>
    public class UowAsyncAttribute : AbstractInterceptorAttribute
    {
        private IWriteUnitOfWork _uow;

        private ILog _log;

        /// <summary>
        /// 事务aop
        /// </summary>
        public UowAsyncAttribute()
        {

        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public async override Task Invoke(AspectContext context, AspectDelegate next)
        {
            try
            {
                _uow = context.ServiceProvider.GetService<IWriteUnitOfWork>();
                await next(context);
                await _uow.CompleteAsync();
            }
            catch (DbUpdateException ex)
            {
                _log = Log.Log.GetLog("UowAsync");
                _log.Error(ex.Message);
                throw ex;
            }
            catch (System.Exception ex)
            {
                if (ex is Warning)
                {
                    var exception = ex as Warning;
                    throw exception;
                }
                if (ex is Transform)
                {
                    var exception = ex as Transform;
                    throw exception;
                }
                if (ex.InnerException is DbUpdateException)
                {
                    var dbex = ex.InnerException as DbUpdateException;
                    _log = Log.Log.GetLog("UowAsync");
                    _log.Error(dbex.InnerException.Message);
                }
                ex.Submit("提交事务");
                throw ex;
            }
        }
    }
}
