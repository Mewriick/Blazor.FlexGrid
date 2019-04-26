using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Linq.Expressions;

namespace Blazor.FlexGrid.Components.Renderers.FormInputs
{
    public class DateInputBuilder : IFormInputRendererBuilder
    {
        private readonly EventCallbackFactory eventCallbackFactory;

        public DateInputBuilder()
        {
            this.eventCallbackFactory = new EventCallbackFactory();
        }

        public Action<IRendererTreeBuilder> BuildRendererTree<TItem>(IActualItemContext<TItem> actualItemContext, FormField field) where TItem : class
        {
            var localColumnName = actualItemContext.ActualColumnName;
            var value = actualItemContext.GetActualItemColumnValue(localColumnName);

            var valueExpression = GetValueExpression(actualItemContext.ActualItem, field);
            var convertedValue = ConvertToDateTime(value);
            actualItemContext.SetActualItemColumnValue(localColumnName, convertedValue);

            return builder =>
            {
                builder
                    .OpenElement(HtmlTagNames.Div, "form-field-wrapper")
                    .OpenComponent(typeof(InputDate<>).MakeGenericType(field.Type))
                    .AddAttribute("Id", $"create-form-{localColumnName}")
                    .AddAttribute("Class", "edit-text-field")
                    .AddAttribute("Value", convertedValue)
                    .AddAttribute("ValueExpression", valueExpression);

                if (field.UnderlyneType == typeof(DateTime))
                {
                    if (field.IsNullable)
                    {
                        builder
                        .AddAttribute("ValueChanged", eventCallbackFactory.Create<DateTime?>(this, v => actualItemContext.SetActualItemColumnValue(localColumnName, v)))
                        .CloseComponent()
                        .AddValidationMessage<DateTime?>(valueExpression);
                    }
                    else
                    {
                        builder
                            .AddAttribute("ValueChanged", eventCallbackFactory.Create<DateTime>(this, v => actualItemContext.SetActualItemColumnValue(localColumnName, v)))
                            .CloseComponent()
                            .AddValidationMessage<DateTime>(valueExpression);
                    }
                }
                else
                {
                    if (field.IsNullable)
                    {
                        builder
                        .AddAttribute("ValueChanged", eventCallbackFactory.Create<DateTimeOffset?>(this, v => actualItemContext.SetActualItemColumnValue(localColumnName, v)))
                        .CloseComponent()
                        .AddValidationMessage<DateTimeOffset?>(valueExpression);
                    }
                    else
                    {
                        builder
                            .AddAttribute("ValueChanged", eventCallbackFactory.Create<DateTimeOffset>(this, v => actualItemContext.SetActualItemColumnValue(localColumnName, v)))
                            .CloseComponent()
                            .AddValidationMessage<DateTimeOffset>(valueExpression);
                    }
                }

                builder.CloseElement();
            };
        }

        public bool IsSupportedDateType(Type type)
             => type == typeof(DateTime) || type == typeof(DateTimeOffset);

        private LambdaExpression GetValueExpression(object actualItem, FormField field)
        {
            if (field.UnderlyneType == typeof(DateTime))
            {
                if (field.IsNullable)
                {
                    return Expression.Lambda<Func<DateTime?>>(
                         Expression.Property(
                             Expression.Constant(actualItem),
                            field.Info));
                }
                else
                {
                    return Expression.Lambda<Func<DateTime>>(
                         Expression.Property(
                             Expression.Constant(actualItem),
                            field.Info));
                }
            }
            else if (field.UnderlyneType == typeof(DateTimeOffset))
            {
                if (field.IsNullable)
                {
                    return Expression.Lambda<Func<DateTimeOffset?>>(
                         Expression.Property(
                             Expression.Constant(actualItem),
                            field.Info));
                }
                else

                {
                    return Expression.Lambda<Func<DateTimeOffset>>(
                         Expression.Property(
                             Expression.Constant(actualItem),
                            field.Info));
                }
            }

            throw new ArgumentException($"{nameof(DateInputBuilder)} does not support type {field.UnderlyneType}");
        }

        private DateTime ConvertToDateTime(object value)
        {
            if (value is DateTimeOffset dateTimeOffset)
            {
                return dateTimeOffset.DateTime == DateTime.MinValue ? DateTime.Now : dateTimeOffset.DateTime;
            }

            if (value is DateTime dateTime)
            {
                return dateTime == DateTime.MinValue ? DateTime.Now : dateTime;
            }

            return DateTime.Now;
        }
    }
}
