using Blazor.FlexGrid.Components.Configuration;
using Blazor.FlexGrid.Components.Configuration.MetaData;
using Blazor.FlexGrid.Components.Configuration.ValueFormatters;
using Blazor.FlexGrid.Components.Events;
using Blazor.FlexGrid.DataAdapters;
using Blazor.FlexGrid.DataSet.Options;
using Blazor.FlexGrid.Filters;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazor.FlexGrid.DataSet
{
    internal class MasterDetailTableDataSet<TItem> : IMasterTableDataSet, IBaseTableDataSet<TItem> where TItem : class
    {
        private IEntityType gridConfiguration;
        private readonly Dictionary<object, ITableDataAdapter> selectedDataAdapters;
        private readonly ITableDataSet tableDataSet;
        private readonly ITableDataAdapterProvider tableDataAdapterProvider;
        private readonly HashSet<ITableDataAdapter> tableDataAdapters;

        public IEnumerable<ITableDataAdapter> DetailDataAdapters => tableDataAdapters;

        public IPagingOptions PageableOptions => tableDataSet.PageableOptions;

        public ISortingOptions SortingOptions => tableDataSet.SortingOptions;

        public IRowEditOptions RowEditOptions => tableDataSet.RowEditOptions;

        public GridViewEvents GridViewEvents => tableDataSet.GridViewEvents;

        public IList<TItem> Items => tableDataSet.Items as List<TItem>;

        IList IBaseTableDataSet.Items => Items is List<TItem> list ? list : Items.ToList();

        public MasterDetailTableDataSet(
            ITableDataSet tableDataSet,
            IGridConfigurationProvider gridConfigurationProvider,
            ITableDataAdapterProvider tableDataAdapterProvider)
        {
            this.tableDataSet = tableDataSet ?? throw new ArgumentNullException(nameof(tableDataSet));
            this.tableDataAdapterProvider = tableDataAdapterProvider ?? throw new ArgumentNullException(nameof(tableDataAdapterProvider));
            this.tableDataAdapters = new HashSet<ITableDataAdapter>();
            this.selectedDataAdapters = new Dictionary<object, ITableDataAdapter>();
            this.gridConfiguration = gridConfigurationProvider?.FindGridEntityConfigurationByType(typeof(TItem)) ?? throw new ArgumentNullException(nameof(gridConfigurationProvider));
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

            selectedDataAdapters[masterDetailRowArguments.SelectedItem] = tableDataAdapterProvider.ConvertToDetailTableDataAdapter(
                masterDetailRowArguments.DataAdapter,
                masterDetailRowArguments.SelectedItem);
        }

        public Task GoToPage(int index)
            => tableDataSet.GoToPage(index);

        public Task SetSortExpression(string expression)
            => tableDataSet.SetSortExpression(expression);

        public void ToggleRowItem(object item)
        {
            tableDataSet.ToggleRowItem(item);
            if (selectedDataAdapters.ContainsKey(item))
            {
                return;
            }

            var tableDataAdapter = default(ITableDataAdapter);

            if (tableDataAdapters.Any())
            {
                tableDataAdapter = tableDataAdapterProvider.ConvertToDetailTableDataAdapter(
                    tableDataAdapters.First(), item);
            }
            else
            {
                tableDataAdapter = tableDataAdapterProvider.CreateCollectionTableDataAdapter(
                    item, gridConfiguration.ClrTypeCollectionProperties.First());
            }

            selectedDataAdapters.Add(item, tableDataAdapter);
        }

        public bool ItemIsSelected(object item)
            => tableDataSet.ItemIsSelected(item);

        public void StartEditItem(object item)
            => tableDataSet.StartEditItem(item);

        public void CancelEditation()
            => tableDataSet.CancelEditation();

        public void EditItemProperty(string propertyName, object propertyValue)
            => tableDataSet.EditItemProperty(propertyName, propertyValue);

        public Task<bool> SaveItem(ITypePropertyAccessor propertyValueAccessor)
            => tableDataSet.SaveItem(propertyValueAccessor);

        public Task<bool> DeleteItem(object item)
            => tableDataSet.DeleteItem(item);

        public Task ApplyFilters(IReadOnlyCollection<IFilterDefinition> filters)
            => tableDataSet.ApplyFilters(filters);
    }
}
