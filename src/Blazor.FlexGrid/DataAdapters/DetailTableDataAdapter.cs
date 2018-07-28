using Blazor.FlexGrid.DataSet;
using Blazor.FlexGrid.DataSet.Options;
using System;
using System.Linq.Expressions;

namespace Blazor.FlexGrid.DataAdapters
{
    public class DetailTableDataAdapter<TItem> : BaseTableDataAdapter where TItem : class
    {
        private readonly IDetailDataAdapterDependencies detailDataAdapterDependencies;
        private readonly IMasterDetailRowArguments masterDetailRowArguments;

        public DetailTableDataAdapter(IDetailDataAdapterDependencies detailDataAdapterDependencies, IMasterDetailRowArguments masterDetailRowArguments)
        {
            this.detailDataAdapterDependencies = detailDataAdapterDependencies ?? throw new ArgumentNullException(nameof(detailDataAdapterDependencies));
            this.masterDetailRowArguments = masterDetailRowArguments ?? throw new ArgumentNullException(nameof(masterDetailRowArguments));
        }

        public override ITableDataSet GetTableDataSet(Action<TableDataSetOptions> configureDataSet)
        {
            var selectedItemType = masterDetailRowArguments.SelectedItem.GetType();
            var detailAdapterItemType = masterDetailRowArguments.DataAdapter.GetUnderlyingType();

            var masterDetailConnection = detailDataAdapterDependencies
                .GridConfigurationProvider
                .GetGridConfigurationByType(selectedItemType)
                .FindRelationshipConfiguration(detailAdapterItemType);

            var constantValue = detailDataAdapterDependencies
                .PropertyValueAccessorCache
                .GetPropertyAccesor(selectedItemType)
                .GetValue(masterDetailRowArguments.SelectedItem, masterDetailConnection.MasterPropertyName);

            var parameter = Expression.Parameter(detailAdapterItemType, "x");
            var member = Expression.Property(parameter, masterDetailConnection.ForeignPropertyName);
            var constant = Expression.Constant(constantValue);
            var body = Expression.Equal(member, constant);

            var filterVisitorType = typeof(FilterVisitor<>).MakeGenericType(masterDetailRowArguments.DataAdapter.GetUnderlyingType());
            var filterVisitor = Activator.CreateInstance(filterVisitorType, new object[] { body, parameter }) as IDataTableAdapterVisitor;

            var clonedDataAdapter = masterDetailRowArguments.DataAdapter.Clone() as ITableDataAdapter;
            clonedDataAdapter.Accept(filterVisitor);

            return clonedDataAdapter.GetTableDataSet(configureDataSet);
        }

        public override object Clone()
            => new DetailTableDataAdapter<TItem>(detailDataAdapterDependencies, masterDetailRowArguments);
    }
}
