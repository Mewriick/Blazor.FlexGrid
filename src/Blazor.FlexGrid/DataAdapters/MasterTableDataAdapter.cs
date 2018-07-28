using Blazor.FlexGrid.DataSet;
using Blazor.FlexGrid.DataSet.Options;
using System;
using System.Collections.Generic;

namespace Blazor.FlexGrid.DataAdapters
{
    public sealed class MasterTableDataAdapter<TItem> : BaseTableDataAdapter where TItem : class
    {
        private readonly ITableDataAdapter mainTableDataAdapter;
        private readonly IDetailDataAdapterDependencies detailDataAdapterDependencies;
        private readonly List<ITableDataAdapter> detailTableDataAdapters;

        public MasterTableDataAdapter(CollectionTableDataAdapter<TItem> mainTableDataAdapter, IDetailDataAdapterDependencies detailDataAdapterDependencies)
            : this(detailDataAdapterDependencies, mainTableDataAdapter)
        {
        }

        public MasterTableDataAdapter(LazyLoadedTableDataAdapter<TItem> mainTableDataAdapter, IDetailDataAdapterDependencies detailDataAdapterDependencies)
            : this(detailDataAdapterDependencies, mainTableDataAdapter)
        {

        }

        internal MasterTableDataAdapter(IDetailDataAdapterDependencies detailDataAdapterDependencies, ITableDataAdapter mainTableDataAdapter)
        {
            this.mainTableDataAdapter = mainTableDataAdapter ?? throw new ArgumentNullException(nameof(mainTableDataAdapter));
            this.detailDataAdapterDependencies = detailDataAdapterDependencies ?? throw new ArgumentNullException(nameof(detailDataAdapterDependencies));
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
            var masterTableDataSet = new MasterDetailTableDataSet<TItem>(mainTableDataSet, detailDataAdapterDependencies);

            detailTableDataAdapters.ForEach(dt => masterTableDataSet.AttachDetailDataSetAdapter(dt));

            return masterTableDataSet;
        }

        public override object Clone()
        {
            var masterTableAdapter = new MasterTableDataAdapter<TItem>(detailDataAdapterDependencies, mainTableDataAdapter);
            detailTableDataAdapters.ForEach(adapter => masterTableAdapter.AddDetailTableSet(adapter));

            return masterTableAdapter;
        }
    }
}
