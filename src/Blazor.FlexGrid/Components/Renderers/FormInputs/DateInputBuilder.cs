using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Blazor.FlexGrid.Components.Renderers.FormInputs
{
    public class DateInputBuilder : IFormInputRendererBuilder
    {
        private readonly EventCallbackFactory eventCallbackFactory;

        public DateInputBuilder()
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
                    .OpenElement(HtmlTagNames.Div, "form-field-wrapper")
                    .OpenComponent(typeof(InputDate<>).MakeGenericType(field.GetMemberType()))
                    .AddAttribute("Id", $"create-form-{localColumnName}")
                    .AddAttribute("Class", "edit-text-field")
                    .AddAttribute("Value", ConvertToDateTime(value))
                    .AddAttribute("ValueExpression", valueExpression);

                if (value is DateTime)
                {
                    builder
                        .AddAttribute("ValueChanged", eventCallbackFactory.Create<DateTime>(this, v => actualItemContext.SetActualItemColumnValue(localColumnName, v)))
                        .CloseComponent()
                        .AddValidationMessage<DateTime>(valueExpression);
                }
                else
                {
                    builder
                        .AddAttribute("ValueChanged", eventCallbackFactory.Create<DateTimeOffset>(this, v => actualItemContext.SetActualItemColumnValue(localColumnName, v)))
                        .CloseComponent()
                        .AddValidationMessage<DateTimeOffset>(valueExpression);
                }

                builder.CloseElement();
            };
        }

        public bool IsSupportedDateType(Type type)
             => type == typeof(DateTime) || type == typeof(DateTimeOffset);

        private LambdaExpression GetValueExpression(object value, object actualItem, PropertyInfo field)
        {
            if (value is DateTime)
            {
                return Expression.Lambda<Func<DateTime>>(
                     Expression.Property(
                         Expression.Constant(actualItem),
                        field));
            }
            else if (value is DateTimeOffset)
            {
                return Expression.Lambda<Func<DateTimeOffset>>(
                     Expression.Property(
                         System.Linq.Expressions.Expression.Constant(actualItem),
                        field));
            }

            throw new ArgumentException($"{nameof(DateInputBuilder)} does not support type {value.GetType()}");
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
