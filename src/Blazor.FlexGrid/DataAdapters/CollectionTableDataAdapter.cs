using Blazor.FlexGrid.DataSet;
using Blazor.FlexGrid.DataSet.Options;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Blazor.FlexGrid.DataAdapters
{
    public class CollectionTableDataAdapter<TItem> : ITableDataAdapter where TItem : class
    {
        private readonly ICollection<TItem> items;

        public CollectionTableDataAdapter(ICollection<TItem> items)
        {
            this.items = items ?? throw new ArgumentNullException(nameof(items));
        }

        public ITableDataSet GetTableDataSet(Action<TableDataSetOptions> configureDataSet)
        {
            var tableDataSetOptions = new TableDataSetOptions();
            configureDataSet?.Invoke(tableDataSetOptions);

            var tableDataSet = new TableDataSet<TItem>(items.AsQueryable())
            {
                PageableOptions = tableDataSetOptions.PageableOptions,
                SortingOptions = new SortingOptions()
            };

            return tableDataSet;
        }
    }
}
