using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Blazor.FlexGrid.Components.Renderers.FormInputs
{
    public class NumberInputBuilder : IFormInputRendererBuilder
    {
        private readonly EventCallbackFactory eventCallbackFactory;

        public NumberInputBuilder()
        {
            this.eventCallbackFactory = new EventCallbackFactory();
        }

        public Action<IRendererTreeBuilder> BuildRendererTree<TItem>(IActualItemContext<TItem> actualItemContext, PropertyInfo field) where TItem : class
        {
            var localColumnName = actualItemContext.ActualColumnName;
            var value = actualItemContext.GetActualItemColumnValue(localColumnName);

            var valueExpression = GetValueExpression(value, actualItemContext.ActualItem, field);

            return builder =>
            {
                builder
                    .OpenElement(HtmlTagNames.Div, "edit-field-wrapper")
                    .OpenComponent(typeof(InputNumber<>).MakeGenericType(field.GetMemberType()))
                    .AddAttribute("Id", $"create-form-{localColumnName}")
                    .AddAttribute("Class", "edit-text-field")
                    .AddAttribute("Value", value)
                    .AddAttribute("ValueExpression", valueExpression);

                if (value is int)
                {
                    builder
                        .AddAttribute("ValueChanged", eventCallbackFactory.Create<int>(this, v => actualItemContext.SetActualItemColumnValue(localColumnName, v)))
                        .CloseComponent()
                        .AddValidationMessage<int>(valueExpression);

                }
                else if (value is long)
                {
                    builder
                        .AddAttribute("ValueChanged", eventCallbackFactory.Create<long>(this, v => actualItemContext.SetActualItemColumnValue(localColumnName, v)))
                        .CloseComponent()
                        .AddValidationMessage<long>(valueExpression);

                }
                else if (value is decimal)
                {
                    builder
                        .AddAttribute("ValueChanged", eventCallbackFactory.Create<decimal>(this, v => actualItemContext.SetActualItemColumnValue(localColumnName, v)))
                        .CloseComponent()
                        .AddValidationMessage<decimal>(valueExpression);

                }
                else if (value is double)
                {
                    builder
                        .AddAttribute("ValueChanged", eventCallbackFactory.Create<double>(this, v => actualItemContext.SetActualItemColumnValue(localColumnName, v)))
                        .CloseComponent()
                        .AddValidationMessage<double>(valueExpression);

                }
                else if (value is float)
                {
                    builder
                        .AddAttribute("ValueChanged", eventCallbackFactory.Create<float>(this, v => actualItemContext.SetActualItemColumnValue(localColumnName, v)))
                        .CloseComponent()
                        .AddValidationMessage<float>(valueExpression);
                }

                builder.CloseElement();
            };

        }

        public bool IsSupportedDateType(Type type)
            => type == typeof(int) || type == typeof(long) || type == typeof(decimal) || type == typeof(double) || type == typeof(float);

        private LambdaExpression GetValueExpression(object value, object actualItem, PropertyInfo field)
        {
            if (value is int)
            {
                return Expression.Lambda<Func<int>>(
                     Expression.Property(
                         Expression.Constant(actualItem),
                        field));
            }
            else if (value is long)
            {
                return Expression.Lambda<Func<long>>(
                     Expression.Property(
                         Expression.Constant(actualItem),
                        field));
            }
            else if (value is decimal)
            {
                return Expression.Lambda<Func<decimal>>(
                     Expression.Property(
                         Expression.Constant(actualItem),
                        field));
            }
            else if (value is double)
            {
                return Expression.Lambda<Func<double>>(
                     Expression.Property(
                         Expression.Constant(actualItem),
                        field));
            }
            else if (value is float)
            {
                return Expression.Lambda<Func<float>>(
                     Expression.Property(
                         Expression.Constant(actualItem),
                        field));
            }

            throw new ArgumentException($"{nameof(NumberInputBuilder)} does not support type {value.GetType()}");
        }
    }
}
