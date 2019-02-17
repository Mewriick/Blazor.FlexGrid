using System;

namespace Blazor.FlexGrid.Components.Configuration.ValueFormatters
{
    public interface ITypePropertyAccessorCache
    {
        void AddPropertyAccessor(Type type, ITypePropertyAccessor propertyAccessor);

        ITypePropertyAccessor GetPropertyAccesor(Type type);
    }
}
