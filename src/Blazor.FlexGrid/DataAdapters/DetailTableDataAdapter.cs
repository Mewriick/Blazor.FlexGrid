using Blazor.FlexGrid.DataSet;
using Blazor.FlexGrid.DataSet.Options;
using System;

namespace Blazor.FlexGrid.DataAdapters
{
    internal class DetailTableDataAdapter<TItem> : BaseTableDataAdapter where TItem : class
    {
        private readonly IDetailDataAdapterVisitors detailDataAdapterVisitors;
        private readonly IMasterDetailRowArguments masterDetailRowArguments;

        public override Type UnderlyingTypeOfItem => typeof(TItem);

        public DetailTableDataAdapter(IDetailDataAdapterVisitors detailDataAdapterVisitors, IMasterDetailRowArguments masterDetailRowArguments)
        {
            this.detailDataAdapterVisitors = detailDataAdapterVisitors ?? throw new ArgumentNullException(nameof(detailDataAdapterVisitors));
            this.masterDetailRowArguments = masterDetailRowArguments ?? throw new ArgumentNullException(nameof(masterDetailRowArguments));
        }

        public override ITableDataSet GetTableDataSet(Action<TableDataSetOptions> configureDataSet)
        {
            var clonedDataAdapter = masterDetailRowArguments.DataAdapter.Clone() as ITableDataAdapter;
            foreach (var visitor in detailDataAdapterVisitors.GetVisitors(masterDetailRowArguments))
            {
                clonedDataAdapter.Accept(visitor);
            }

            return clonedDataAdapter.GetTableDataSet(configureDataSet);
        }

        public override object Clone()
            => new DetailTableDataAdapter<TItem>(detailDataAdapterVisitors, masterDetailRowArguments);
    }
}
