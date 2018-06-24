using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Blazor.FlexGrid.Components.Configuration.ValueFormatters
{
    public class TypeWrapper : IPropertyValueAccessor
    {
        private readonly Dictionary<string, Func<object, object>> getters
            = new Dictionary<string, Func<object, object>>();

        public TypeWrapper(Type clrType)
        {
            foreach (var property in clrType.GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                var wrappedObjectParameter = Expression.Parameter(typeof(object));
                var getExpression = Expression.Lambda<Func<object, object>>(
                    Expression.Convert(
                        Expression.Property(
                            Expression.Convert(wrappedObjectParameter, clrType), property),
                        typeof(object)),
                    wrappedObjectParameter);

                this.getters.Add(property.Name, getExpression.Compile());
            }
        }

        public object GetValue(object @object, string name)
        {
            var get = getters[name];
            return get(@object);
        }
    }
}
