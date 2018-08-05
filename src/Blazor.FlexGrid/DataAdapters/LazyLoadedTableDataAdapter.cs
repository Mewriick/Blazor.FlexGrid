using Blazor.FlexGrid.DataSet;
using Blazor.FlexGrid.DataSet.Options;
using System;

namespace Blazor.FlexGrid.DataAdapters
{
    /// <summary>
    /// Create <seealso cref="LazyTableDataSet{TItem}"/> 
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    public class LazyLoadedTableDataAdapter<TItem> : BaseTableDataAdapter where TItem : class
    {
        private readonly ILazyDataSetLoader<TItem> lazyDataSetLoader;

        public override Type UnderlyingTypeOfItem => typeof(TItem);

        public LazyLoadedTableDataAdapter(ILazyDataSetLoader<TItem> lazyDataSetLoader)
        {
            this.lazyDataSetLoader = lazyDataSetLoader ?? throw new ArgumentNullException(nameof(lazyDataSetLoader));
        }

        public override ITableDataSet GetTableDataSet(Action<TableDataSetOptions> configureDataSet)
        {
            var tableDataSetOptions = new TableDataSetOptions();
            configureDataSet?.Invoke(tableDataSetOptions);

            var tableDataSet = new LazyTableDataSet<TItem>(lazyDataSetLoader)
            {
                LazyLoadingOptions = tableDataSetOptions.LazyLoadingOptions,
                PageableOptions = tableDataSetOptions.PageableOptions,
                SortingOptions = new SortingOptions()
            };

            return tableDataSet;
        }

        public override object Clone()
            => new LazyLoadedTableDataAdapter<TItem>(lazyDataSetLoader);
    }
}
