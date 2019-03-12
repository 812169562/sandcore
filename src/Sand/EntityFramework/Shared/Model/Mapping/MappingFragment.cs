﻿// Description: Entity Framework Bulk Operations & Utilities (EF Bulk SaveChanges, Insert, Update, Delete, Merge | LINQ Query Cache, Deferred, Filter, IncludeFilter, IncludeOptimize | Audit)
// Website & Documentation: https://github.com/zzzprojects/Entity-Framework-Plus
// Forum & Issues: https://github.com/zzzprojects/EntityFramework-Plus/issues
// License: https://github.com/zzzprojects/EntityFramework-Plus/blob/master/LICENSE
// More projects: http://www.zzzprojects.com/
// Copyright © ZZZ Projects Inc. 2014 - 2016. All rights reserved.

#if FULL || BATCH_DELETE || BATCH_UPDATE
#if EF5 || EF6
using System.Collections.Generic;
using System.Xml.Serialization;
using Z.EntityFramework.Extensions.Core.Mapping;
using Sand.EntityFramework.Shared;

namespace Sand.EntityFramework.Shared
{
    /// <summary>
    ///     Please visit the
    ///     <see href="http://msdn.microsoft.com/en-us/library/vstudio/bb399202(v=vs.100).aspx">
    ///         Microsoft documentation
    ///     </see>
    ///     for more detail.
    /// </summary>
    public class MappingFragment
    {
	    /// <summary>Default constructor.</summary>
	    public MappingFragment()
	    {
		    ScalarAccessors = new List<ScalarAccessorMapping>();
	    }

		/// <summary>Gets or sets the set the store entity belongs to.</summary>
		/// <value>The store entity set.</value>
		[XmlIgnore]
        public EntityContainerEntitySet StoreEntitySet { get; set; }

	    /// <summary>Gets or sets the scalar accessors.</summary>
	    /// <value>The scalar accessors.</value>
	    [XmlIgnore]
	    public List<ScalarAccessorMapping> ScalarAccessors { get; set; }

#region XmlDeserialization

		/// <summary>
		///     Please visit the
		///     <see href="http://msdn.microsoft.com/en-us/library/vstudio/bb399202(v=vs.100).aspx">
		///         Microsoft documentation
		///     </see>
		///     for more detail.
		/// </summary>
		/// <value>The name of the store entity set.</value>
		[XmlAttribute("StoreEntitySet")]
        public string StoreEntitySetName { get; set; }

        /// <summary>
        ///     Please visit the
        ///     <see href="http://msdn.microsoft.com/en-us/library/vstudio/bb399202(v=vs.100).aspx">
        ///         Microsoft documentation
        ///     </see>
        ///     for more detail.
        /// </summary>
        /// <value>The scalar properties.</value>
        [XmlElement("ScalarProperty")]
        public List<ScalarPropertyMapping> ScalarProperties { get; set; }

	    /// <summary>
	    ///     Please visit the
	    ///     <see href="http://msdn.microsoft.com/en-us/library/vstudio/bb399202(v=vs.100).aspx">
	    ///         Microsoft documentation
	    ///     </see>
	    ///     for more detail.
	    /// </summary>
	    /// <value>The complex properties.</value>
	    [XmlElement("ComplexProperty")]
	    public List<ComplexPropertyMapping> ComplexProperties { get; set; }

	    /// <summary>
	    ///     Please visit the
	    ///     <see href="http://msdn.microsoft.com/en-us/library/vstudio/bb399202(v=vs.100).aspx">
	    ///         Microsoft documentation
	    ///     </see>
	    ///     for more detail.
	    /// </summary>
	    /// <value>The conditions.</value>
	    [XmlElement("Condition")]
	    public List<ConditionPropertyMapping> Conditions { get; set; }

#endregion
	}
}

#endif
#endif