using Blazor.FlexGrid.DataSet.Options;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazor.FlexGrid.DataSet
{
    public class TableDataSet<TItem> : ITableDataSet, IBaseTableDataSet<TItem> where TItem : class
    {
        private IQueryable<TItem> source;
        private HashSet<object> selectedItems;

        public IPagingOptions PageableOptions { get; set; } = new PageableOptions();

        public ISortingOptions SortingOptions { get; set; } = new SortingOptions();

        /// <summary>
        /// Gets or sets the items for the current page.
        /// </summary>
        public IList<TItem> Items { get; set; } = new List<TItem>();

        IList IBaseTableDataSet.Items => Items is List<TItem> list ? list : Items.ToList();


        public TableDataSet(IQueryable<TItem> source)
        {
            this.source = source ?? throw new ArgumentNullException(nameof(source));
            this.selectedItems = new HashSet<object>();
        }

        public Task GoToPage(int index)
        {
            PageableOptions.CurrentPage = index;
            LoadFromQueryableSource();

            return Task.FromResult(0);
        }

        public Task SetSortExpression(string expression)
        {
            if (SortingOptions.SortExpression != expression)
            {
                SortingOptions.SortExpression = expression;
                SortingOptions.SortDescending = false;
            }
            else
            {
                SortingOptions.SortDescending = !SortingOptions.SortDescending;
            }

            return GoToPage(0);
        }

        public void ToggleRowItem(object item)
        {
            if (ItemIsSelected(item))
            {
                selectedItems.Remove(item);
                return;
            }

            selectedItems.Add(item);
        }

        public bool ItemIsSelected(object item)
            => selectedItems.Contains(item);

        private void LoadFromQueryableSource()
        {
            PageableOptions.TotalItemsCount = source.Count();
            Items = ApplyFiltersToQueryable(source).ToList();
        }

        private IQueryable<TItem> ApplyFiltersToQueryable(IQueryable<TItem> queryable)
        {
            queryable = ApplySortingToQueryable(queryable);
            queryable = ApplyPagingToQueryable(queryable);

            return queryable;
        }

        private IQueryable<TItem> ApplyPagingToQueryable(IQueryable<TItem> queryable)
        {
            return PageableOptions != null && PageableOptions.PageSize > 0
                ? queryable.Skip(PageableOptions.PageSize * PageableOptions.CurrentPage)
                    .Take(PageableOptions.PageSize)
                : queryable;
        }

        private IQueryable<TItem> ApplySortingToQueryable(IQueryable<TItem> queryable)
        {
            if (string.IsNullOrEmpty(SortingOptions?.SortExpression))
            {
                return queryable;
            }

            return queryable.ApplyOrder(SortingOptions.SortExpression, SortingOptions.SortDescending ? "OrderByDescending" : "OrderBy");
        }
    }
}
