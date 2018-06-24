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

        public IPageableOptions PageableOptions { get; set; } = new PageableOptions();

        /// <summary>
        /// Gets or sets the items for the current page.
        /// </summary>
        public IList<TItem> Items { get; set; } = new List<TItem>();

        IList IBaseTableDataSet.Items => Items is List<TItem> list ? list : Items.ToList();


        public TableDataSet(IQueryable<TItem> source)
        {
            this.source = source ?? throw new ArgumentNullException(nameof(source));
        }

        public Task GoToPage(int index)
        {
            Console.Error.WriteLine($"GoToPage {index}");
            try
            {
                PageableOptions.CurrentPage = index;
                LoadFromQueryableSource();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"{ex}");
            }

            return Task.FromResult(0);
        }

        private void LoadFromQueryableSource()
        {
            PageableOptions.TotalItemsCount = source.Count();
            Items = ApplyFiltersToQueryable(source).ToList();
        }

        private IQueryable<TItem> ApplyFiltersToQueryable(IQueryable<TItem> queryable)
        {
            queryable = ApplyPagingToQueryable(queryable);

            return queryable;
        }

        private IQueryable<TItem> ApplyPagingToQueryable(IQueryable<TItem> queryable)
        {
            return PageableOptions != null && PageableOptions.PageSize > 0 ?
                queryable.Skip(PageableOptions.PageSize * PageableOptions.CurrentPage).Take(PageableOptions.PageSize) :
                queryable;
        }
    }
}
