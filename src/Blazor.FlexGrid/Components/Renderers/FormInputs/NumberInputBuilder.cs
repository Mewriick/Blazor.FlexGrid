using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Linq.Expressions;

namespace Blazor.FlexGrid.Components.Renderers.FormInputs
{
    public class NumberInputBuilder : IFormInputRendererBuilder
    {
        private readonly EventCallbackFactory eventCallbackFactory;

        public NumberInputBuilder()
        {
            this.eventCallbackFactory = new EventCallbackFactory();
        }

        public Action<IRendererTreeBuilder> BuildRendererTree<TItem>(IActualItemContext<TItem> actualItemContext, FormField field) where TItem : class
        {
            var localColumnName = actualItemContext.ActualColumnName;
            var value = actualItemContext.GetActualItemColumnValue(localColumnName);

            var valueExpression = GetValueExpression(actualItemContext.ActualItem, field);

            return builder =>
            {
                builder
                    .OpenElement(HtmlTagNames.Div, "form-field-wrapper")
                    .OpenComponent(typeof(InputNumber<>).MakeGenericType(field.Type))
                    .AddAttribute("Id", $"create-form-{localColumnName}")
                    .AddAttribute("Class", "edit-text-field")
                    .AddAttribute("Value", value)
                    .AddAttribute("ValueExpression", valueExpression);

                if (field.UnderlyneType == typeof(int))
                {
                    if (field.IsNullable)
                    {
                        builder
                          .AddAttribute("ValueChanged", eventCallbackFactory.Create<int?>(this, v => actualItemContext.SetActualItemColumnValue(localColumnName, v)))
                          .CloseComponent()
                          .AddValidationMessage<int?>(valueExpression);
                    }
                    else
                    {
                        builder
                            .AddAttribute("ValueChanged", eventCallbackFactory.Create<int>(this, v => actualItemContext.SetActualItemColumnValue(localColumnName, v)))
                            .CloseComponent()
                            .AddValidationMessage<int>(valueExpression);
                    }

                }
                else if (field.UnderlyneType == typeof(long))
                {
                    if (field.IsNullable)
                    {
                        builder
                            .AddAttribute("ValueChanged", eventCallbackFactory.Create<long?>(this, v => actualItemContext.SetActualItemColumnValue(localColumnName, v)))
                            .CloseComponent()
                            .AddValidationMessage<long?>(valueExpression);
                    }
                    else
                    {
                        builder
                           .AddAttribute("ValueChanged", eventCallbackFactory.Create<long>(this, v => actualItemContext.SetActualItemColumnValue(localColumnName, v)))
                           .CloseComponent()
                           .AddValidationMessage<long>(valueExpression);
                    }

                }
                else if (field.UnderlyneType == typeof(decimal))
                {
                    if (field.IsNullable)
                    {
                        builder
                        .AddAttribute("ValueChanged", eventCallbackFactory.Create<decimal?>(this, v => actualItemContext.SetActualItemColumnValue(localColumnName, v)))
                        .CloseComponent()
                        .AddValidationMessage<decimal?>(valueExpression);
                    }
                    else
                    {
                        builder
                            .AddAttribute("ValueChanged", eventCallbackFactory.Create<decimal>(this, v => actualItemContext.SetActualItemColumnValue(localColumnName, v)))
                            .CloseComponent()
                            .AddValidationMessage<decimal>(valueExpression);
                    }
                }
                else if (field.UnderlyneType == typeof(double))
                {
                    if (field.IsNullable)
                    {
                        builder
                            .AddAttribute("ValueChanged", eventCallbackFactory.Create<double?>(this, v => actualItemContext.SetActualItemColumnValue(localColumnName, v)))
                            .CloseComponent()
                            .AddValidationMessage<double?>(valueExpression);
                    }
                    else
                    {
                        builder
                            .AddAttribute("ValueChanged", eventCallbackFactory.Create<double>(this, v => actualItemContext.SetActualItemColumnValue(localColumnName, v)))
                            .CloseComponent()
                            .AddValidationMessage<double>(valueExpression);
                    }

                }
                else if (field.UnderlyneType == typeof(float))
                {
                    if (field.IsNullable)
                    {
                        builder
                            .AddAttribute("ValueChanged", eventCallbackFactory.Create<float?>(this, v => actualItemContext.SetActualItemColumnValue(localColumnName, v)))
                            .CloseComponent()
                            .AddValidationMessage<float?>(valueExpression);
                    }
                    else
                    {
                        builder
                            .AddAttribute("ValueChanged", eventCallbackFactory.Create<float>(this, v => actualItemContext.SetActualItemColumnValue(localColumnName, v)))
                            .CloseComponent()
                            .AddValidationMessage<float>(valueExpression);
                    }
                }

                builder.CloseElement();
            };

        }

        public bool IsSupportedDateType(Type type)
            => type == typeof(int) || type == typeof(long) || type == typeof(decimal) || type == typeof(double) || type == typeof(float);

        private LambdaExpression GetValueExpression(object actualItem, FormField field)
        {
            if (field.UnderlyneType == typeof(int))
            {
                if (field.IsNullable)
                {
                    return Expression.Lambda<Func<int?>>(
                            Expression.Property(
                                Expression.Constant(actualItem),
                               field.Info));
                }
                else
                {

                    return Expression.Lambda<Func<int>>(
                         Expression.Property(
                             Expression.Constant(actualItem),
                            field.Info));
                }
            }
            else if (field.UnderlyneType == typeof(long))
            {
                if (field.IsNullable)
                {
                    return Expression.Lambda<Func<long?>>(
                       Expression.Property(
                           Expression.Constant(actualItem),
                          field.Info));
                }
                else
                {
                    return Expression.Lambda<Func<long>>(
                         Expression.Property(
                             Expression.Constant(actualItem),
                            field.Info));
                }
            }
            else if (field.UnderlyneType == typeof(decimal))
            {
                if (field.IsNullable)
                {
                    return Expression.Lambda<Func<decimal?>>(
                         Expression.Property(
                             Expression.Constant(actualItem),
                            field.Info));
                }
                else
                {
                    return Expression.Lambda<Func<decimal>>(
                         Expression.Property(
                             Expression.Constant(actualItem),
                            field.Info));
                }
            }
            else if (field.UnderlyneType == typeof(double))
            {
                if (field.IsNullable)
                {
                    return Expression.Lambda<Func<double?>>(
                       Expression.Property(
                           Expression.Constant(actualItem),
                          field.Info));
                }
                else
                {
                    return Expression.Lambda<Func<double>>(
                         Expression.Property(
                             Expression.Constant(actualItem),
                            field.Info));
                }
            }
            else if (field.UnderlyneType == typeof(float))
            {
                if (field.IsNullable)
                {
                    return Expression.Lambda<Func<float?>>(
                       Expression.Property(
                           Expression.Constant(actualItem),
                          field.Info));
                }
                else
                {
                    return Expression.Lambda<Func<float>>(
                         Expression.Property(
                             Expression.Constant(actualItem),
                            field.Info));
                }
            }

            throw new ArgumentException($"{nameof(NumberInputBuilder)} does not support type {field.UnderlyneType}");
        }
    }
}
