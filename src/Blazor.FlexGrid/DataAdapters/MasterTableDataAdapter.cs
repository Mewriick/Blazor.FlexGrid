using Blazor.FlexGrid.Components.Configuration;
using Blazor.FlexGrid.DataSet;
using Blazor.FlexGrid.DataSet.Options;
using System;
using System.Collections.Generic;

namespace Blazor.FlexGrid.DataAdapters
{
    public sealed class MasterTableDataAdapter<TItem> : BaseTableDataAdapter where TItem : class
    {
        private readonly ITableDataAdapter mainTableDataAdapter;
        private readonly IGridConfigurationProvider gridConfigurationProvider;
        private readonly ITableDataAdapterProvider tableDataAdapterProvider;
        private readonly List<ITableDataAdapter> detailTableDataAdapters;

        public override Type UnderlyingTypeOfItem => typeof(TItem);

        public MasterTableDataAdapter(
            CollectionTableDataAdapter<TItem> mainTableDataAdapter,
            IGridConfigurationProvider gridConfigurationProvider,
            ITableDataAdapterProvider tableDataAdapterProvider)
            : this(gridConfigurationProvider, tableDataAdapterProvider, mainTableDataAdapter)
        {
        }

        public MasterTableDataAdapter(
            LazyLoadedTableDataAdapter<TItem> mainTableDataAdapter,
            IGridConfigurationProvider gridConfigurationProvider,
            ITableDataAdapterProvider tableDataAdapterProvider)
            : this(gridConfigurationProvider, tableDataAdapterProvider, mainTableDataAdapter)
        {

        }

        internal MasterTableDataAdapter(
            IGridConfigurationProvider gridConfigurationProvider,
            ITableDataAdapterProvider tableDataAdapterProvider,
            ITableDataAdapter mainTableDataAdapter)
        {
            this.mainTableDataAdapter = mainTableDataAdapter ?? throw new ArgumentNullException(nameof(mainTableDataAdapter));
            this.gridConfigurationProvider = gridConfigurationProvider ?? throw new ArgumentNullException(nameof(gridConfigurationProvider));
            this.tableDataAdapterProvider = tableDataAdapterProvider ?? throw new ArgumentNullException(nameof(tableDataAdapterProvider));
            this.detailTableDataAdapters = new List<ITableDataAdapter>();
        }

        public void AddDetailTableSet(ITableDataAdapter tableDataAdapter)
        {
            if (tableDataAdapter is null)
            {
                throw new ArgumentNullException(nameof(tableDataAdapter));
            }

            detailTableDataAdapters.Add(tableDataAdapter);
        }

        public override ITableDataSet GetTableDataSet(Action<TableDataSetOptions> configureDataSet)
        {
            var mainTableDataSet = mainTableDataAdapter.GetTableDataSet(configureDataSet);
            var masterTableDataSet = new MasterDetailTableDataSet<TItem>(mainTableDataSet, gridConfigurationProvider, tableDataAdapterProvider);

            detailTableDataAdapters.ForEach(dt => masterTableDataSet.AttachDetailDataSetAdapter(dt));

            return masterTableDataSet;
        }

        public override object Clone()
        {
            var masterTableAdapter = new MasterTableDataAdapter<TItem>(gridConfigurationProvider, tableDataAdapterProvider, mainTableDataAdapter);
            detailTableDataAdapters.ForEach(adapter => masterTableAdapter.AddDetailTableSet(adapter));

            return masterTableAdapter;
        }
    }
}
