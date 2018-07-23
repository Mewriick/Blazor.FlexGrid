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

        public MasterTableDataAdapter(CollectionTableDataAdapter<TItem> mainTableDataAdapter)
        {
            this.mainTableDataAdapter = mainTableDataAdapter ?? throw new ArgumentNullException(nameof(mainTableDataAdapter));
            this.detailTableDataAdapters = new List<ITableDataAdapter>();
        }

        public MasterTableDataAdapter(LazyLoadedTableDataAdapter<TItem> mainTableDataAdapter)
        {
            this.mainTableDataAdapter = mainTableDataAdapter ?? throw new ArgumentNullException(nameof(mainTableDataAdapter));
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
            var masterTableDataSet = new MasterDetailDataSet<TItem>(mainTableDataSet);

            detailTableDataAdapters.ForEach(dt => masterTableDataSet.AttachDetailDataSetAdapter(dt));

            return masterTableDataSet;
        }
    }
}
