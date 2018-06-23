using Blazor.FlexGrid.DataSet.Options;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazor.FlexGrid.DataSet
{
    public class LazyTableDataSet<TItem> : ILazyTableDataSet, IBaseTableDataSet<TItem> where TItem : class
    {
        private readonly ILazyDataSetLoader<TItem> lazyDataSetLoader;

        public IPageableOptions PageableOptions { get; set; } = new PageableOptions();

        /// <summary>
        /// Gets or sets the items for the current page.
        /// </summary>
        public IList<TItem> Items { get; set; } = new List<TItem>();

        IList IBaseTableDataSet.Items => Items is List<TItem> list ? list : Items.ToList();

        public ILazyLoadingOptions LazyLoadingOptions { get; set; } = new LazyLoadingOptions();

        public LazyTableDataSet(ILazyDataSetLoader<TItem> lazyDataSetLoader)
        {
            this.lazyDataSetLoader = lazyDataSetLoader ?? throw new ArgumentNullException(nameof(lazyDataSetLoader));
        }

        public async Task GoToPage(int index)
        {
            PageableOptions.CurrentPage = index;
            var pagedDataResult = await lazyDataSetLoader.GetTablePageData(LazyLoadingOptions, PageableOptions);
            PageableOptions.TotalItemsCount = pagedDataResult.TotalCount;
            Items = pagedDataResult.Items;
        }
    }
}
