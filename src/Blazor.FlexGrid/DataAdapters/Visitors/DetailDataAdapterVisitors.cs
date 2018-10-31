using Blazor.FlexGrid.Components.Configuration;
using Blazor.FlexGrid.Components.Configuration.ValueFormatters;
using Blazor.FlexGrid.DataSet.Options;
using System;
using System.Collections.Generic;

namespace Blazor.FlexGrid.DataAdapters.Visitors
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
        {
            var selectedItemType = masterDetailRowArguments.SelectedItem.GetType();
            var detailAdapterItemType = masterDetailRowArguments.DataAdapter.UnderlyingTypeOfItem;
            var masterDetailConfiguration = gridConfigurationProvider
                .GetGridConfigurationByType(selectedItemType)
                .FindRelationshipConfiguration(detailAdapterItemType);

            if (masterDetailRowArguments.DataAdapter is ILazyLoadedTableDataAdapter)
            {
                return new List<IDataTableAdapterVisitor>
               {
                   new LazyLoadingRouteParamVisitor(masterDetailConfiguration, masterDetailRowArguments, propertyValueAccessorCache)
               };
            }

            var filterVisitorType = typeof(FilterVisitor<>).MakeGenericType(detailAdapterItemType);
            var filterVisitor = Activator.CreateInstance(filterVisitorType,
                new object[]
                {
                    masterDetailConfiguration,
                    masterDetailRowArguments,
                    propertyValueAccessorCache
                }) as IDataTableAdapterVisitor;

            return new List<IDataTableAdapterVisitor>
            {
                filterVisitor
            };
        }
    }
}
