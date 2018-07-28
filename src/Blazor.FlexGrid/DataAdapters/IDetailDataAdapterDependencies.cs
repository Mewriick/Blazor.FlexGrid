using Blazor.FlexGrid.Components.Configuration;
using Blazor.FlexGrid.Components.Configuration.ValueFormatters;

namespace Blazor.FlexGrid.DataAdapters
{
    public interface IDetailDataAdapterDependencies
    {
        IPropertyValueAccessorCache PropertyValueAccessorCache { get; }

        IGridConfigurationProvider GridConfigurationProvider { get; }
    }
}
