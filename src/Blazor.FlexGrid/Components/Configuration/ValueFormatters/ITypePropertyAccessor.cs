using System.Collections.Generic;
using System.Reflection;

namespace Blazor.FlexGrid.Components.Configuration.ValueFormatters
{
    public interface ITypePropertyAccessor : ITypeProperties
    {
        object GetValue(object @object, string name);

        void SetValue(object instance, string propertyName, object value);
    }

    public interface ITypeProperties
    {
        IEnumerable<PropertyInfo> Properties { get; }
    }
}
