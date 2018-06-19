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
            PageableOptions.CurrentPage = index;
            LoadFromQueryableSource();

            return Task.FromResult(0);
        }

        private void LoadFromQueryableSource()
        {
            Items = source.ToList();
        }
    }
}
