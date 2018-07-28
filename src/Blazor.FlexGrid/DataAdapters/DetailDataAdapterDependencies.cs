using Blazor.FlexGrid.Components.Configuration;
using Blazor.FlexGrid.Components.Configuration.ValueFormatters;
using System;

namespace Blazor.FlexGrid.DataAdapters
{
    public class DetailDataAdapterDependencies : IDetailDataAdapterDependencies
    {
        public IPropertyValueAccessorCache PropertyValueAccessorCache { get; }

        public IGridConfigurationProvider GridConfigurationProvider { get; }


        public DetailDataAdapterDependencies(IPropertyValueAccessorCache propertyValueAccessorCache, IGridConfigurationProvider gridConfigurationProvider)
        {
            PropertyValueAccessorCache = propertyValueAccessorCache ?? throw new ArgumentNullException(nameof(propertyValueAccessorCache));
            GridConfigurationProvider = gridConfigurationProvider ?? throw new ArgumentNullException(nameof(gridConfigurationProvider));
        }
    }
}
