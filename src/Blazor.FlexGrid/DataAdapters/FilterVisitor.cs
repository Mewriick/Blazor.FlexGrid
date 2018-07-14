using System;

namespace Blazor.FlexGrid.DataAdapters
{
    public class FilterVisitor<TItem> : IDataTableAdapterVisitor where TItem : class
    {
        private readonly Func<TItem, bool> filter;

        public FilterVisitor(Func<TItem, bool> filter)
        {
            this.filter = filter ?? throw new ArgumentNullException(nameof(filter));
        }

        public void Visit(ITableDataAdapter tableDataAdapter)
        {
            if (tableDataAdapter is CollectionTableDataAdapter<TItem> collectionTableDataAdapter)
            {
                collectionTableDataAdapter.Filter = filter;
            }
        }
    }
}
