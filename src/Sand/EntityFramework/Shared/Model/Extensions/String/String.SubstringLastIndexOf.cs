﻿// Description: Entity Framework Bulk Operations & Utilities (EF Bulk SaveChanges, Insert, Update, Delete, Merge | LINQ Query Cache, Deferred, Filter, IncludeFilter, IncludeOptimize | Audit)
// Website & Documentation: https://github.com/zzzprojects/Entity-Framework-Plus
// Forum & Issues: https://github.com/zzzprojects/EntityFramework-Plus/issues
// License: https://github.com/zzzprojects/EntityFramework-Plus/blob/master/LICENSE
// More projects: http://www.zzzprojects.com/
// Copyright © ZZZ Projects Inc. 2014 - 2016. All rights reserved.


#if FULL || BATCH_DELETE || BATCH_UPDATE
#if EF5 || EF6

namespace Sand.EntityFramework.Shared
{
    internal static partial class InternalExtensions
    {
        public static string SubstringLastIndexOf(this string @this, string lastIndexOf)
        {
            var lastIndexOfPos = @this.LastIndexOf(lastIndexOf);
            return @this.Substring(lastIndexOfPos + 1);
        }
    }
}

#endif
#endif