using AspectCore.DynamicProxy;
using AspectCore.Injector;
using Sand.Domain.Uow;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Sand.Log.Less;

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
        private IUnitOfWork _uow;

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
                _uow = context.ServiceProvider.GetService<IUnitOfWork>();
                await next(context);
                _uow.Complete();
            }
            catch (System.Exception ex)
            {
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
        private IUnitOfWork _uow;

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
                _uow = context.ServiceProvider.GetService<IUnitOfWork>();
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
                if (ex.InnerException is DbUpdateException)
                {
                    var dbex = ex.InnerException as DbUpdateException;
                    _log = Log.Log.GetLog("UowAsync");
                    _log.Error(dbex.InnerException.Message);
                }
                else
                {
                    _log = Log.Log.GetLog("UowAsync");
                    _log.Error(ex.Message);
                }
             
                throw ex;
            }
        }
    }
}
