using System;

namespace Blazor.FlexGrid.DataAdapters
{
    public class MasterTableDataAdapterBuilder<TItem> where TItem : class
    {
        private readonly IDetailDataAdapterDependencies detailDataAdapterDependencies;
        private MasterTableDataAdapter<TItem> masterTableDataAdapter;

        public MasterTableDataAdapterBuilder(IDetailDataAdapterDependencies detailDataAdapterDependencies)
        {
            this.detailDataAdapterDependencies = detailDataAdapterDependencies ?? throw new ArgumentNullException(nameof(detailDataAdapterDependencies));
        }

        public MasterTableDataAdapterBuilder<TItem> MasterTableDataAdapter(CollectionTableDataAdapter<TItem> mainTableDataAdapter)
        {
            masterTableDataAdapter = new MasterTableDataAdapter<TItem>(mainTableDataAdapter, detailDataAdapterDependencies);

            return this;
        }

        public MasterTableDataAdapterBuilder<TItem> MasterTableDataAdapter(LazyLoadedTableDataAdapter<TItem> mainTableDataAdapter)
        {
            masterTableDataAdapter = new MasterTableDataAdapter<TItem>(mainTableDataAdapter, detailDataAdapterDependencies);

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
