using Blazor.FlexGrid.DataAdapters;
using Blazor.FlexGrid.DataSet.Options;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazor.FlexGrid.DataSet
{
    public class MasterDetailTableDataSet<TItem> : IMasterTableDataSet, IBaseTableDataSet<TItem> where TItem : class
    {
        private readonly IDetailDataAdapterDependencies detailDataAdapterDependencies;
        private readonly Dictionary<object, ITableDataAdapter> selectedDataAdapters;
        private readonly ITableDataSet tableDataSet;
        private readonly HashSet<ITableDataAdapter> tableDataAdapters;

        public IEnumerable<ITableDataAdapter> DetailDataAdapters => tableDataAdapters;

        public IPagingOptions PageableOptions => tableDataSet.PageableOptions;

        public ISortingOptions SortingOptions => tableDataSet.SortingOptions;

        public IList<TItem> Items => tableDataSet.Items as List<TItem>;

        IList IBaseTableDataSet.Items => Items is List<TItem> list ? list : Items.ToList();

        public MasterDetailTableDataSet(ITableDataSet tableDataSet, IDetailDataAdapterDependencies detailDataAdapterDependencies)
        {
            this.tableDataSet = tableDataSet ?? throw new ArgumentNullException(nameof(tableDataSet));
            this.detailDataAdapterDependencies = detailDataAdapterDependencies ?? throw new ArgumentNullException(nameof(detailDataAdapterDependencies));
            this.tableDataAdapters = new HashSet<ITableDataAdapter>();
            this.selectedDataAdapters = new Dictionary<object, ITableDataAdapter>();
        }

        public void AttachDetailDataSetAdapter(ITableDataAdapter tableDataAdapter)
        {
            if (tableDataAdapters.Contains(tableDataAdapter))
            {
                return;
            }

            tableDataAdapters.Add(tableDataAdapter);
        }

        public ITableDataAdapter GetSelectedDataAdapter(object selectedItem)
            => selectedDataAdapters[selectedItem];

        public void SelectDataAdapter(IMasterDetailRowArguments masterDetailRowArguments)
        {
            if (masterDetailRowArguments is null)
            {
                throw new ArgumentNullException(nameof(masterDetailRowArguments));
            }

            selectedDataAdapters[masterDetailRowArguments.SelectedItem] = CreateDetailTableDataAdapter(masterDetailRowArguments.DataAdapter, masterDetailRowArguments.SelectedItem);
        }

        public Task GoToPage(int index)
        {
            return tableDataSet.GoToPage(index);
        }

        public Task SetSortExpression(string expression)
            => tableDataSet.SetSortExpression(expression);

        public void ToggleRowItem(object item)
        {
            tableDataSet.ToggleRowItem(item);

            if (!selectedDataAdapters.ContainsKey(item))
            {
                selectedDataAdapters.Add(item, CreateDetailTableDataAdapter(tableDataAdapters.First(), item));
            }
        }

        public bool ItemIsSelected(object item)
            => tableDataSet.ItemIsSelected(item);

        private ITableDataAdapter CreateDetailTableDataAdapter(ITableDataAdapter dataAdapter, object selectedItem)
        {
            var detailAdapterType = typeof(DetailTableDataAdapter<>).MakeGenericType(dataAdapter.GetUnderlyingType());

            return Activator.CreateInstance(detailAdapterType,
                new object[] { detailDataAdapterDependencies, new MasterDetailRowArguments(dataAdapter, selectedItem) }) as ITableDataAdapter;
        }
    }
}
