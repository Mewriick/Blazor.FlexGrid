using Blazor.FlexGrid.Components.Configuration;
using Blazor.FlexGrid.Components.Configuration.ValueFormatters;
using Blazor.FlexGrid.DataSet.Options;
using System;
using System.Collections;
using System.Reflection;

namespace Blazor.FlexGrid.DataAdapters
{
    public class RunTimeTableDataAdapterProvider : ITableDataAdapterProvider
    {
        private readonly IGridConfigurationProvider gridConfigurationProvider;
        private readonly IDetailDataAdapterVisitors detailDataAdapterVisitors;
        private readonly IPropertyValueAccessorCache propertyValueAccessorCache;

        public RunTimeTableDataAdapterProvider(
            IGridConfigurationProvider gridConfigurationProvider,
            IPropertyValueAccessorCache propertyValueAccessorCache,
            IDetailDataAdapterVisitors detailDataAdapterVisitors)
        {
            this.gridConfigurationProvider = gridConfigurationProvider ?? throw new ArgumentNullException(nameof(gridConfigurationProvider));
            this.detailDataAdapterVisitors = detailDataAdapterVisitors ?? throw new ArgumentNullException(nameof(detailDataAdapterVisitors));
            this.propertyValueAccessorCache = propertyValueAccessorCache ?? throw new ArgumentNullException(nameof(propertyValueAccessorCache));
        }

        public ITableDataAdapter ConvertToDetailTableDataAdapter(ITableDataAdapter tableDataAdapter, object selectedItem)
        {
            var detailAdapterType = typeof(DetailTableDataAdapter<>).MakeGenericType(tableDataAdapter.UnderlyingTypeOfItem);

            return Activator.CreateInstance(detailAdapterType,
                new object[] { gridConfigurationProvider, detailDataAdapterVisitors, new MasterDetailRowArguments(tableDataAdapter, selectedItem) }) as ITableDataAdapter;
        }

        public ITableDataAdapter CreateCollectionTableDataAdapter(object selectedItem, PropertyInfo propertyInfo)
        {
            var propertyType = propertyInfo.PropertyType.GenericTypeArguments[0];
            var propertyValueGetter = propertyValueAccessorCache.GetPropertyAccesor(selectedItem.GetType());
            var collectionValue = propertyValueGetter.GetValue(selectedItem, propertyInfo.Name) as ICollection;

            var dataAdapterType = typeof(CollectionTableDataAdapter<>).MakeGenericType(propertyType);
            var dataAdapter = Activator.CreateInstance(dataAdapterType,
                new object[] { collectionValue }) as ITableDataAdapter;

            return dataAdapter;
        }
    }
}
