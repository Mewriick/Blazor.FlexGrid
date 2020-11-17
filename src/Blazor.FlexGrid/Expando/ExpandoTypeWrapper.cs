using Blazor.FlexGrid.Components.Configuration.ValueFormatters;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;

namespace Blazor.FlexGrid.Expando
{
    public class ExpandoTypeWrapper : ITypePropertyAccessor
    {
        public IEnumerable<PropertyInfo> Properties { get; }

        public ExpandoTypeWrapper(ExpandoObject @object)
        {
            Properties = @object
                    .Select(o => new ExpandoPropertyInfo(o.Key, o.Value.GetType()))
                    .Cast<PropertyInfo>()
                    .ToList();
        }

        public object GetValue(object @object, string name)
        {
            if (@object is ExpandoObject expandoObject)
            {
                return ((IDictionary<string, object?>)expandoObject)[name];
            }

            return null;
        }

        public void SetValue(object instance, string propertyName, object value)
        {
            if (instance is ExpandoObject expandoObject)
            {
                ((IDictionary<string, object?>)expandoObject)[propertyName] = value;
            }
        }
    }
}
