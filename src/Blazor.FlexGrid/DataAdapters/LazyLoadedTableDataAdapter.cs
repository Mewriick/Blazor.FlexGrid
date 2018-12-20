using Blazor.FlexGrid.DataSet;
using Blazor.FlexGrid.DataSet.Options;
using System;

namespace Blazor.FlexGrid.DataAdapters
{
    /// <summary>
    /// Create <seealso cref="LazyTableDataSet{TItem}"/> 
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    public class LazyLoadedTableDataAdapter<TItem> : BaseTableDataAdapter, ILazyLoadedTableDataAdapter where TItem : class
    {
        private readonly ILazyDataSetLoader<TItem> lazyDataSetLoader;
        private readonly ILazyDataSetItemSaver<TItem> lazyDataSetItemSaver;

        public override Type UnderlyingTypeOfItem => typeof(TItem);

        public Action<LazyRequestParams> AddRequestParamsAction { get; set; }

        public LazyLoadedTableDataAdapter(ILazyDataSetLoader<TItem> lazyDataSetLoader, ILazyDataSetItemSaver<TItem> lazyDataSetItemSaver)
        {
            this.lazyDataSetLoader = lazyDataSetLoader ?? throw new ArgumentNullException(nameof(lazyDataSetLoader));
            this.lazyDataSetItemSaver = lazyDataSetItemSaver ?? throw new ArgumentNullException(nameof(lazyDataSetItemSaver));
        }

        public override ITableDataSet GetTableDataSet(Action<TableDataSetOptions> configureDataSet)
        {
            var tableDataSetOptions = new TableDataSetOptions();
            configureDataSet?.Invoke(tableDataSetOptions);

            var lazyLoadingOptions = tableDataSetOptions.LazyLoadingOptions;
            AddRequestParamsAction?.Invoke(lazyLoadingOptions.RequestParams);

            var tableDataSet = new LazyTableDataSet<TItem>(lazyDataSetLoader, lazyDataSetItemSaver)
            {
                LazyLoadingOptions = lazyLoadingOptions,
                PageableOptions = tableDataSetOptions.PageableOptions,
                SortingOptions = tableDataSetOptions.SortingOptions,
                GridViewEvents = tableDataSetOptions.GridViewEvents
            };

            return tableDataSet;
        }

        public override object Clone()
            => new LazyLoadedTableDataAdapter<TItem>(lazyDataSetLoader, lazyDataSetItemSaver);
    }

    /// <summary>
    /// Only for type check purposes
    /// </summary>
    internal interface ILazyLoadedTableDataAdapter
    {
        Action<LazyRequestParams> AddRequestParamsAction { set; }
    }
}
