﻿// Description: Entity Framework Bulk Operations & Utilities (EF Bulk SaveChanges, Insert, Update, Delete, Merge | LINQ Query Cache, Deferred, Filter, IncludeFilter, IncludeOptimize | Audit)
// Website & Documentation: https://github.com/zzzprojects/Entity-Framework-Plus
// Forum & Issues: https://github.com/zzzprojects/EntityFramework-Plus/issues
// License: https://github.com/zzzprojects/EntityFramework-Plus/blob/master/LICENSE
// More projects: http://www.zzzprojects.com/
// Copyright © ZZZ Projects Inc. 2014 - 2016. All rights reserved.

#if FULL
using System.Collections.Generic;
using System.Linq;

namespace Sand.EntityFramework.EF
{
    internal static partial class InternalExtensions
    {
        internal static IOrderedQueryable<TSource> OrderBy<TSource>(this IQueryable<TSource> source, string propertyName)
        {
            return Order(source, propertyName, true, true);
        }

        internal static IOrderedQueryable<TSource> OrderBy<TSource, TKey>(this IQueryable<TSource> source, string propertyName, IComparer<TKey> comparer)
        {
            return Order(source, propertyName, true, true, comparer);
        }
    }
}
#endif