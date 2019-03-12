#if FULL || BATCH_DELETE || BATCH_UPDATE
#if EF5 || EF6
using System;
using System.Collections.Generic;
using Sand.EntityFramework.Shared.Core.Mapping;

namespace Sand.EntityFramework.Shared
{
    public class TableDefinition
    {
	    public TableDefinition()
	    {
		    Values = new List<Tuple<string, object>>();
		    Keys = new List<ScalarPropertyMapping>();
		}

		public List<Tuple<string, object>> Values { get; set; }
		public List<ScalarPropertyMapping> Keys { get; set; }
		public List<ScalarPropertyMapping> Properties { get; set; }
		public string Schema { get; set; }
		public string Table { get; set; }

		/// <summary>
		/// When dealing with inherited types, update the entity with the highest order (no dependencies first)
		/// </summary>
		public int Order { get; set; }
    }
}
#endif
#endif