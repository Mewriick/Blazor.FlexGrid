using Blazor.FlexGrid.DataSet;
using Blazor.FlexGrid.DataSet.Options;
using Blazor.FlexGrid.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Blazor.FlexGrid.DataAdapters
{
    /// <summary>
    /// Create <seealso cref="TableDataSet{TItem}"/> from collection of items
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    public class CollectionTableDataAdapter<TItem> : BaseTableDataAdapter where TItem : class
    {
        private readonly ICollection<TItem> items;

        public Expression<Func<TItem, bool>> Filter { get; set; } = item => true;

        public override Type UnderlyingTypeOfItem => typeof(TItem);


        public CollectionTableDataAdapter(ICollection<TItem> items)
        {
            this.items = items ?? throw new ArgumentNullException(nameof(items));
        }

        public override ITableDataSet GetTableDataSet(Action<TableDataSetOptions> configureDataSet)
        {
            var tableDataSetOptions = new TableDataSetOptions();
            configureDataSet?.Invoke(tableDataSetOptions);

            var tableDataSet = new TableDataSet<TItem>(items.Where(Filter.Compile()).AsQueryable(), new FilterExpressionTreeBuilder<TItem>())
            {
                PageableOptions = tableDataSetOptions.PageableOptions,
                SortingOptions = tableDataSetOptions.SortingOptions,
                GridViewEvents = tableDataSetOptions.GridViewEvents,
                GroupingOptions = tableDataSetOptions.GroupingOptions,
            };

            return tableDataSet;
        }

        public override object Clone()
            => new CollectionTableDataAdapter<TItem>(items);
    }
}
