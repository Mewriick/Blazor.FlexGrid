using Blazor.FlexGrid.Components.Configuration;
using Blazor.FlexGrid.Components.Configuration.ValueFormatters;
using Blazor.FlexGrid.DataSet.Options;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Blazor.FlexGrid.DataAdapters
{
    public class DetailDataAdapterVisitors : IDetailDataAdapterVisitors
    {
        private readonly IPropertyValueAccessorCache propertyValueAccessorCache;
        private readonly IGridConfigurationProvider gridConfigurationProvider;

        public DetailDataAdapterVisitors(IPropertyValueAccessorCache propertyValueAccessorCache, IGridConfigurationProvider gridConfigurationProvider)
        {
            this.propertyValueAccessorCache = propertyValueAccessorCache ?? throw new ArgumentNullException(nameof(propertyValueAccessorCache));
            this.gridConfigurationProvider = gridConfigurationProvider ?? throw new ArgumentNullException(nameof(gridConfigurationProvider));
        }

        public IEnumerable<IDataTableAdapterVisitor> GetVisitors(IMasterDetailRowArguments masterDetailRowArguments)
            => new List<IDataTableAdapterVisitor>
            {
                GetFilterVisitor(masterDetailRowArguments)
            };

        private IDataTableAdapterVisitor GetFilterVisitor(IMasterDetailRowArguments masterDetailRowArguments)
        {
            var selectedItemType = masterDetailRowArguments.SelectedItem.GetType();
            var detailAdapterItemType = masterDetailRowArguments.DataAdapter.UnderlyingTypeOfItem;

            var masterDetailConfiguration = gridConfigurationProvider
                .GetGridConfigurationByType(selectedItemType)
                .FindRelationshipConfiguration(detailAdapterItemType);

            var constantValue = propertyValueAccessorCache
                .GetPropertyAccesor(selectedItemType)
                .GetValue(masterDetailRowArguments.SelectedItem, masterDetailConfiguration.MasterDetailConnection.MasterPropertyName);

            var parameter = Expression.Parameter(detailAdapterItemType, "x");
            var member = Expression.Property(parameter, masterDetailConfiguration.MasterDetailConnection.ForeignPropertyName);
            var constant = Expression.Constant(constantValue);
            var body = Expression.Equal(member, constant);

            var filterVisitorType = typeof(FilterVisitor<>).MakeGenericType(detailAdapterItemType);
            var filterVisitor = Activator.CreateInstance(filterVisitorType, new object[] { body, parameter }) as IDataTableAdapterVisitor;

            return filterVisitor;
        }
    }
}
