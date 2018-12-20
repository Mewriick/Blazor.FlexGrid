using Microsoft.Extensions.Logging;
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

        private readonly Dictionary<string, Action<object, object>> setters
            = new Dictionary<string, Action<object, object>>();

        private readonly ILogger logger;

        public TypeWrapper(Type clrType, ILogger logger)
        {
            foreach (var property in clrType.GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                var wrappedObjectParameter = Expression.Parameter(typeof(object));
                var valueParameter = Expression.Parameter(typeof(object));

                var getExpression = Expression.Lambda<Func<object, object>>(
                    Expression.Convert(
                        Expression.Property(
                            Expression.Convert(wrappedObjectParameter, clrType), property),
                        typeof(object)),
                    wrappedObjectParameter);

                this.getters.Add(property.Name, getExpression.Compile());

                if (property.CanWrite)
                {

                    var setExpression = Expression.Lambda<Action<object, object>>(
                         Expression.Assign(
                             Expression.Property(
                                 Expression.Convert(wrappedObjectParameter, clrType), property),
                             Expression.Convert(valueParameter, property.PropertyType)),
                         wrappedObjectParameter, valueParameter);

                    this.setters.Add(property.Name, setExpression.Compile());
                }
            }

            this.logger = logger;
        }

        public object GetValue(object @object, string name)
        {
            try
            {
                var get = getters[name];
                return get(@object);
            }
            catch (Exception ex)
            {
                logger.LogError($"TypeWrapper:GetValue. Ex: {ex}");

                throw;
            }
        }

        public void SetValue(object instance, string propertyName, object value)
        {
            try
            {
                var set = setters[propertyName];
                set(instance, value);
            }

            catch (Exception ex)
            {
                logger.LogError($"TypeWrapper:SetValue. Ex: {ex}");

                throw;
            }
        }
    }
}
