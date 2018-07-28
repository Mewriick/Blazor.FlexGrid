using System;

namespace Blazor.FlexGrid.Components.Configuration.ValueFormatters
{
    public interface IPropertyValueAccessorCache
    {
        void AddPropertyAccessor(Type type, IPropertyValueAccessor propertyValueAccessor);

        IPropertyValueAccessor GetPropertyAccesor(Type type);
    }
}
