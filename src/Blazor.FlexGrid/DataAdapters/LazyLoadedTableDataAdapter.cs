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

        public LazyLoadedTableDataAdapter(ILazyDataSetLoader<TItem> lazyDataSetLoader)
        {
            this.lazyDataSetLoader = lazyDataSetLoader ?? throw new ArgumentNullException(nameof(lazyDataSetLoader));
        }

        public override ITableDataSet GetTableDataSet(Action<TableDataSetOptions> configureDataSet)
        {
            var tableDataSetOptions = new TableDataSetOptions();
            configureDataSet?.Invoke(tableDataSetOptions);

            if (string.IsNullOrWhiteSpace(tableDataSetOptions.LazyLoadingOptions.DataUri))
            {
                throw new ArgumentNullException($"When you using {nameof(LazyLoadedTableDataAdapter<TItem>)} you must specify " +
                    $"{nameof(LazyLoadingOptions.DataUri)} for lazy data retrieving. If you do not want use lazy loading feature use {nameof(CollectionTableDataAdapter<TItem>)} instead.");
            }

            var tableDataSet = new LazyTableDataSet<TItem>(lazyDataSetLoader)
            {
                LazyLoadingOptions = tableDataSetOptions.LazyLoadingOptions,
                PageableOptions = tableDataSetOptions.PageableOptions,
                SortingOptions = new SortingOptions()
            };

            return tableDataSet;
        }
    }
}
