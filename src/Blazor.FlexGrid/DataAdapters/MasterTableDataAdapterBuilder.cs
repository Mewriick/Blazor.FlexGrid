using Blazor.FlexGrid.Components.Configuration;
using System;

namespace Blazor.FlexGrid.DataAdapters
{
    public class MasterTableDataAdapterBuilder<TItem> where TItem : class
    {
        private MasterTableDataAdapter<TItem> masterTableDataAdapter;
        private readonly IGridConfigurationProvider gridConfigurationProvider;
        private readonly ITableDataAdapterProvider tableDataAdapterProvider;

        public MasterTableDataAdapterBuilder(IGridConfigurationProvider gridConfigurationProvider, ITableDataAdapterProvider tableDataAdapterProvider)
        {
            this.gridConfigurationProvider = gridConfigurationProvider ?? throw new ArgumentNullException(nameof(gridConfigurationProvider));
            this.tableDataAdapterProvider = tableDataAdapterProvider ?? throw new ArgumentNullException(nameof(tableDataAdapterProvider));
        }

        public MasterTableDataAdapterBuilder<TItem> MasterTableDataAdapter(CollectionTableDataAdapter<TItem> mainTableDataAdapter)
        {
            masterTableDataAdapter = new MasterTableDataAdapter<TItem>(gridConfigurationProvider, tableDataAdapterProvider, mainTableDataAdapter);

            return this;
        }

        public MasterTableDataAdapterBuilder<TItem> MasterTableDataAdapter(LazyLoadedTableDataAdapter<TItem> mainTableDataAdapter)
        {
            masterTableDataAdapter = new MasterTableDataAdapter<TItem>(gridConfigurationProvider, tableDataAdapterProvider, mainTableDataAdapter);

            return this;
        }

        public MasterTableDataAdapterBuilder<TItem> WithDetailTableDataAdapter(ITableDataAdapter tableDataAdapter)
        {
            masterTableDataAdapter.AddDetailTableSet(tableDataAdapter);

            return this;
        }

        public MasterTableDataAdapter<TItem> Build()
        {
            if (masterTableDataAdapter is null)
            {
                throw new InvalidOperationException($"Before build you must first call {nameof(MasterTableDataAdapterBuilder<TItem>.MasterTableDataAdapter)}");
            }

            return masterTableDataAdapter;
        }
    }
}
