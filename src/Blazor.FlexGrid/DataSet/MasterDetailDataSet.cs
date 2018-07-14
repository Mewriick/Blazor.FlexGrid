using Blazor.FlexGrid.DataAdapters;
using Blazor.FlexGrid.DataSet.Options;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazor.FlexGrid.DataSet
{
    public class MasterDetailDataSet<TItem> : IMasterTableDataSet, IBaseTableDataSet<TItem> where TItem : class
    {
        private readonly ITableDataSet tableDataSet;
        private readonly HashSet<ITableDataAdapter> tableDataAdapters;

        public IEnumerable<ITableDataAdapter> DetailDataAdapters => tableDataAdapters;

        public IPagingOptions PageableOptions => tableDataSet.PageableOptions;

        public ISortingOptions SortingOptions => tableDataSet.SortingOptions;

        public IList<TItem> Items => tableDataSet.Items as List<TItem>;

        IList IBaseTableDataSet.Items => Items is List<TItem> list ? list : Items.ToList();

        public MasterDetailDataSet(ITableDataSet tableDataSet)
        {
            this.tableDataSet = tableDataSet ?? throw new ArgumentNullException(nameof(tableDataSet));
            this.tableDataAdapters = new HashSet<ITableDataAdapter>();
        }

        public void AttachDetailDataSetAdapter(ITableDataAdapter tableDataAdapter)
        {
            if (tableDataAdapters.Contains(tableDataAdapter))
            {
                return;
            }

            tableDataAdapters.Add(tableDataAdapter);
        }

        public Task GoToPage(int index)
            => tableDataSet.GoToPage(index);

        public Task SetSortExpression(string expression)
            => tableDataSet.SetSortExpression(expression);

        public void ToggleRowItem(object item)
            => tableDataSet.ToggleRowItem(item);

        public bool ItemIsSelected(object item)
            => tableDataSet.ItemIsSelected(item);
    }
}
