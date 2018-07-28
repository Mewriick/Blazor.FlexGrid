using System;
using System.Collections.Generic;

namespace Blazor.FlexGrid.Components.Configuration.ValueFormatters
{
    public class PropertyValueAccessorCache : IPropertyValueAccessorCache
    {
        private readonly Dictionary<Type, IPropertyValueAccessor> propertyAccessors;

        public PropertyValueAccessorCache()
        {
            this.propertyAccessors = new Dictionary<Type, IPropertyValueAccessor>();
        }

        public void AddPropertyAccessor(Type type, IPropertyValueAccessor propertyValueAccessor)
        {
            if (type is null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (propertyValueAccessor is null)
            {
                throw new ArgumentNullException(nameof(propertyValueAccessor));
            }

            if (propertyAccessors.ContainsKey(type))
            {
                return;
            }

            propertyAccessors.Add(type, propertyValueAccessor);
        }

        public IPropertyValueAccessor GetPropertyAccesor(Type type)
        {
            if (type is null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (propertyAccessors.TryGetValue(type, out var propertyValueAccessor))
            {
                return propertyValueAccessor;
            }

            propertyValueAccessor = new TypeWrapper(type);
            propertyAccessors.Add(type, propertyValueAccessor);

            return propertyValueAccessor;
        }
    }
}
