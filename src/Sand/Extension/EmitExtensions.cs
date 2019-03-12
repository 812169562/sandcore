using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmitMapper;
using EmitMapper.Mappers;
using EmitMapper.MappingConfiguration;
using Sand.Sys;

namespace Sand.Extensions
{
    public static class EmitExtensions
    {
        /// <summary>
        /// 将源对象映射到目标对象
        /// </summary>
        /// <typeparam name="TSource">源类型</typeparam>
        /// <typeparam name="TDestination">目标类型</typeparam>
        /// <param name="source">源对象</param>
        /// <param name="destination">目标对象</param>
        /// <param name="config">配置</param>
        public static TDestination EmitMapTo<TSource, TDestination>(this TSource source, TDestination destination, DefaultMapConfig config=null)
        {
            if (config == null)
                config=new DefaultMapConfig().NullSubstitution<DateTime?, DateTime>(t => DateTime.Now)
                    .NullSubstitution<bool?,bool>(t=>false)
                    .NullSubstitution<Guid?,Guid>(t=>Uuid.Next())
                    .NullSubstitution<int?,int>(t => -1)
                    .NullSubstitution<decimal?, decimal>(t => -1)
                    .NullSubstitution<float?, float>(t => -1)
                    .NullSubstitution<double?, double>(t => -1);
            var map = ObjectMapperManager.DefaultInstance.GetMapper<TSource, TDestination>(config);
            return map.Map(source, destination);
        }

        /// <summary>
        /// 将源对象映射到目标对象
        /// </summary>
        /// <typeparam name="TSource">源类型</typeparam>
        /// <typeparam name="TDestination">目标类型</typeparam>
        /// <param name="source">源对象</param>
        public static TDestination EmitMapTo<TSource, TDestination>(this TSource source)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<TSource, TDestination>().Map(source); ;
        }


        /// <summary>
        /// 将源对象映射到目标对象
        /// </summary>
        /// <typeparam name="TSource">源类型</typeparam>
        /// <typeparam name="TDestination">目标类型</typeparam>
        /// <param name="source">源对象</param>
        /// <param name="destination">目标对象</param>
        public static IList<TDestination> EmitMapToList<TSource, TDestination>(this IList<TSource> source, IList<TDestination> destination)
        {
            return ObjectMapperManager.DefaultInstance.GetMapper<IList<TSource>, IList<TDestination>>().Map(source);
        }
    }
}
