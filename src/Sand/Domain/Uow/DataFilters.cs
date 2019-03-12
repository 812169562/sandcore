using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sand.Domain.Uow
{
   /// <summary>
   /// 数据拦截器
   /// </summary>
   public   static class DataFilters
   {
        /// <summary>
        /// 逻辑删除
        /// </summary>
       public const string SoftDelete = "IsDeleted";
   }
}
