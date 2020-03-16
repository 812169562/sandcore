using Sand.Domain.Uow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Sand.EntityFramework.UpdatePlus
{
    /// <summary>
    /// 部分更新
    /// </summary>
    public static class UpdateExtension
    {
        /// <summary>
        /// 部分更新
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="uow"></param>
        /// <param name="updateFactory"></param>
        /// <param name="defaultFactory"></param>
        /// <returns></returns>
        public static int Update<T>(this IQueryable<T> query, IWriteUnitOfWork uow, Expression<Func<T, T>> updateFactory, Expression<Func<T, T>> defaultFactory = null) where T : class
        {

            var context = uow;
            var list = query.ToList();
            var compiled = updateFactory.Compile();
            var compiledDefault = defaultFactory.Compile();
            var memberBindings = ((MemberInitExpression)updateFactory.Body).Bindings;
            var accessors = memberBindings
                .Select(x => x.Member.Name)
                .Select(x => new PropertyOrFieldAccessor(typeof(T).GetProperty(x, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)))
                .ToList();
            if (updateFactory != null)
            {
                var memberBindingsDefault = ((MemberInitExpression)defaultFactory.Body).Bindings;
                accessors.AddRange(memberBindingsDefault
               .Select(x => x.Member.Name)
               .Select(x => new PropertyOrFieldAccessor(typeof(T).GetProperty(x, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)))
               .ToList());
            }

            foreach (var item in list)
            {
                var newItem = compiled(item);
                var defaultnewItem = compiledDefault(item);
                foreach (var accessor in accessors)
                {
                    var value = accessor.GetValue(newItem);
                    if (accessor.PropertyOrField.Name == "LastUpdateId")
                    {
                        value = accessor.GetValue(defaultnewItem);
                    }
                    if (accessor.PropertyOrField.Name == "LastUpdateName")
                    {
                        value = accessor.GetValue(defaultnewItem);
                    }
                    if (accessor.PropertyOrField.Name == "LastUpdateTime")
                    {
                        value = accessor.GetValue(defaultnewItem);
                    }
                    if (accessor.PropertyOrField.Name == "Version")
                    {
                        value = accessor.GetValue(defaultnewItem);
                    }
                    accessor.SetValue(item, value);
                }
            }
            return list.Count;
        }

        /// <summary>
        /// 部分更新
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="uow"></param>
        /// <param name="updateFactory"></param>
        /// <param name="defaultFactory"></param>
        /// <returns></returns>

        public static async Task<int> UpdateAsync<T>(this IQueryable<T> query, IWriteUnitOfWork uow, Expression<Func<T, T>> updateFactory, Expression<Func<T, T>> defaultFactory = null) where T : class
        {
            NLog.LogManager.GetLogger("Debug").Error($"1工作单元释放_" + uow.TraceId);
            var count = Update(query, uow, updateFactory, defaultFactory);
            return await Task.FromResult(count);
        }
    }
}
