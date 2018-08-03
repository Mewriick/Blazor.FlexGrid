using Blazor.FlexGrid.Components.Configuration;
using Blazor.FlexGrid.DataAdapters;
using System;

namespace Blazor.FlexGrid.DataSet
{
    public class MasterDetailTableDataSetFactory : IMasterDetailTableDataSetFactory
    {
        private readonly IGridConfigurationProvider gridConfigurationProvider;
        private readonly ITableDataAdapterProvider tableDataAdapterProvider;

        public MasterDetailTableDataSetFactory(IGridConfigurationProvider gridConfigurationProvider, ITableDataAdapterProvider tableDataAdapterProvider)
        {
            this.gridConfigurationProvider = gridConfigurationProvider ?? throw new ArgumentNullException(nameof(gridConfigurationProvider));
            this.tableDataAdapterProvider = tableDataAdapterProvider ?? throw new ArgumentNullException(nameof(tableDataAdapterProvider));
        }

        public ITableDataSet ConvertToMasterTableIfIsRequired(ITableDataSet tableDataSet)
        {
            if (tableDataSet is IMasterTableDataSet masterTableDataSet)
            {
                return masterTableDataSet;
            }

            var entityConfiguration = gridConfigurationProvider.GetGridConfigurationByType(tableDataSet.UnderlyingTypeOfItem());
            if (!entityConfiguration.IsMasterTable)
            {
                return tableDataSet;
            }

            var masterDetailTableDataSetType = typeof(MasterDetailTableDataSet<>).MakeGenericType(tableDataSet.UnderlyingTypeOfItem());
            var masterDetailTableDataSet = Activator.CreateInstance(masterDetailTableDataSetType,
                new object[] { tableDataSet, gridConfigurationProvider, tableDataAdapterProvider }) as IMasterTableDataSet;

            return masterDetailTableDataSet;
        }
    }
}
