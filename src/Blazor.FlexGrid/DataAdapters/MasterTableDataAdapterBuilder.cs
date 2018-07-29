using System;

namespace Blazor.FlexGrid.DataAdapters
{
    public class MasterTableDataAdapterBuilder<TItem> where TItem : class
    {
        private readonly IDetailDataAdapterVisitors detailDataAdapterVisitors;
        private MasterTableDataAdapter<TItem> masterTableDataAdapter;

        public MasterTableDataAdapterBuilder(IDetailDataAdapterVisitors detailDataAdapterVisitors)
        {
            this.detailDataAdapterVisitors = detailDataAdapterVisitors ?? throw new ArgumentNullException(nameof(detailDataAdapterVisitors));
        }

        public MasterTableDataAdapterBuilder<TItem> MasterTableDataAdapter(CollectionTableDataAdapter<TItem> mainTableDataAdapter)
        {
            masterTableDataAdapter = new MasterTableDataAdapter<TItem>(mainTableDataAdapter, detailDataAdapterVisitors);

            return this;
        }

        public MasterTableDataAdapterBuilder<TItem> MasterTableDataAdapter(LazyLoadedTableDataAdapter<TItem> mainTableDataAdapter)
        {
            masterTableDataAdapter = new MasterTableDataAdapter<TItem>(mainTableDataAdapter, detailDataAdapterVisitors);

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
