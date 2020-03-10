using Blazor.FlexGrid.DataSet;
using Blazor.FlexGrid.DataSet.Options;
using Microsoft.AspNetCore.Http.Extensions;
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
        private readonly ILazyGroupableDataSetLoader<TItem> lazyGroupableDataSetLoader;
        private readonly ILazyDataSetItemManipulator<TItem> lazyDataSetItemSaver;

        public override Type UnderlyingTypeOfItem => typeof(TItem);

        public Action<QueryBuilder> AddRequestParamsAction { get; set; }

        public LazyLoadedTableDataAdapter(
            ILazyDataSetLoader<TItem> lazyDataSetLoader,
            ILazyGroupableDataSetLoader<TItem> lazyGroupableDataSetLoader,
            ILazyDataSetItemManipulator<TItem> lazyDataSetItemSaver)
        {
            this.lazyDataSetLoader = lazyDataSetLoader ?? throw new ArgumentNullException(nameof(lazyDataSetLoader));
            this.lazyGroupableDataSetLoader = lazyGroupableDataSetLoader ?? throw new ArgumentNullException(nameof(lazyGroupableDataSetLoader));
            this.lazyDataSetItemSaver = lazyDataSetItemSaver ?? throw new ArgumentNullException(nameof(lazyDataSetItemSaver));
        }

        public override ITableDataSet GetTableDataSet(Action<TableDataSetOptions> configureDataSet)
        {
            var tableDataSetOptions = new TableDataSetOptions();
            configureDataSet?.Invoke(tableDataSetOptions);

            var lazyLoadingOptions = tableDataSetOptions.LazyLoadingOptions ?? new LazyLoadingOptions();
            AddRequestParamsAction?.Invoke(lazyLoadingOptions.RequestParams);

            currentTableDataSet = new LazyTableDataSet<TItem>(lazyDataSetLoader, lazyGroupableDataSetLoader, lazyDataSetItemSaver)
            {
                LazyLoadingOptions = lazyLoadingOptions,
                PageableOptions = tableDataSetOptions.PageableOptions,
                SortingOptions = tableDataSetOptions.SortingOptions,
                GroupingOptions = tableDataSetOptions.GroupingOptions,
                GridViewEvents = tableDataSetOptions.GridViewEvents
            };

            return currentTableDataSet;
        }

        public override object Clone()
            => new LazyLoadedTableDataAdapter<TItem>(lazyDataSetLoader, lazyGroupableDataSetLoader, lazyDataSetItemSaver);
    }

    /// <summary>
    /// Only for type check purposes
    /// </summary>
    internal interface ILazyLoadedTableDataAdapter
    {
        Action<QueryBuilder> AddRequestParamsAction { set; }
    }
}
