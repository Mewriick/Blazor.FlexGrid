using Blazor.FlexGrid.DataSet;
using Blazor.FlexGrid.DataSet.Options;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Blazor.FlexGrid.DataAdapters
{
    /// <summary>
    /// Create <seealso cref="TableDataSet{TItem}"/> from collection of items
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    public class CollectionTableDataAdapter<TItem> : BaseTableDataAdapter where TItem : class
    {
        private readonly ICollection<TItem> items;

        public Func<TItem, bool> Filter { get; set; } = item => true;

        public CollectionTableDataAdapter(ICollection<TItem> items)
        {
            this.items = items ?? throw new ArgumentNullException(nameof(items));
        }

        public override ITableDataSet GetTableDataSet(Action<TableDataSetOptions> configureDataSet)
        {
            var tableDataSetOptions = new TableDataSetOptions();
            configureDataSet?.Invoke(tableDataSetOptions);

            var tableDataSet = new TableDataSet<TItem>(items.Where(Filter).AsQueryable())
            {
                PageableOptions = tableDataSetOptions.PageableOptions,
                SortingOptions = new SortingOptions()
            };

            return tableDataSet;
        }
    }
}
