using Blazor.FlexGrid.DataSet;
using Blazor.FlexGrid.DataSet.Options;
using System;
using System.Collections.Generic;

namespace Blazor.FlexGrid.DataAdapters
{
    public class MasterTableDataAdapter<TItem> : BaseTableDataAdapter where TItem : class
    {
        private readonly ITableDataAdapter mainTableDataAdapter;
        private readonly List<ITableDataAdapter> detailTableDataAdapters;

        public MasterTableDataAdapter(ITableDataAdapter mainTableDataAdapter)
        {
            this.mainTableDataAdapter = mainTableDataAdapter ?? throw new ArgumentNullException(nameof(mainTableDataAdapter));
            if (mainTableDataAdapter.GetType() == typeof(MasterDetailDataSet<>))
            {
                throw new InvalidOperationException($"The type of {nameof(mainTableDataAdapter)} cannot be {mainTableDataAdapter.GetType()}." +
                    $"{nameof(ITableDataAdapter)} must provide collection of items e.g {nameof(CollectionTableDataAdapter<TItem>)}");
            }

            this.detailTableDataAdapters = new List<ITableDataAdapter>();
        }

        public void AddDetailTableSet(ITableDataAdapter tableDataAdapter)
            => detailTableDataAdapters.Add(tableDataAdapter);

        public override ITableDataSet GetTableDataSet(Action<TableDataSetOptions> configureDataSet)
        {
            var mainTableDataSet = mainTableDataAdapter.GetTableDataSet(configureDataSet);
            var masterTableDataSet = new MasterDetailDataSet<TItem>(mainTableDataSet);

            detailTableDataAdapters.ForEach(dt => masterTableDataSet.AttachDetailDataSetAdapter(dt));

            return masterTableDataSet;
        }
    }
}
