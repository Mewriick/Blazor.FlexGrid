using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Linq.Expressions;

namespace Blazor.FlexGrid.Components.Renderers.FormInputs
{
    public class CheckBoxInputBuilder : IFormInputRendererBuilder
    {
        private readonly EventCallbackFactory eventCallbackFactory;

        public CheckBoxInputBuilder()
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
                    .OpenElement(HtmlTagNames.Div, "checkbox")
                    .OpenElement(HtmlTagNames.Label)
                    .OpenComponent(typeof(InputCheckbox))
                    .AddAttribute("Id", $"create-form-{localColumnName}")
                    .AddAttribute("Class", string.Empty)
                    .AddAttribute("Value", value)
                    .AddAttribute("ValueExpression", valueExpression);

                if (field.IsNullable)
                {
                    builder.AddAttribute("ValueChanged", eventCallbackFactory.Create<bool?>(this, v => actualItemContext.SetActualItemColumnValue(localColumnName, v)));
                }
                else
                {
                    builder.AddAttribute("ValueChanged", eventCallbackFactory.Create<bool>(this, v => actualItemContext.SetActualItemColumnValue(localColumnName, v)));
                }

                builder
                    .CloseComponent()
                    .OpenElement(HtmlTagNames.Span, "cr")
                    .OpenElement(HtmlTagNames.I, "cr-icon fa fa-check")
                    .CloseElement()
                    .CloseElement()
                    .CloseElement()
                    .CloseElement();

                if (field.IsNullable)
                {
                    builder.AddValidationMessage<bool?>(valueExpression);
                }
                else
                {
                    builder.AddValidationMessage<bool>(valueExpression);
                }
            };
        }

        public bool IsSupportedDateType(Type type)
            => type == typeof(bool);

        private LambdaExpression GetValueExpression(object actualItem, FormField field)
        {
            if (field.IsNullable)
            {
                return Expression.Lambda<Func<bool?>>(
                 Expression.Property(
                     Expression.Constant(actualItem),
                    field.Info));
            }

            return Expression.Lambda<Func<bool>>(
                 Expression.Property(
                     Expression.Constant(actualItem),
                    field.Info));
        }
    }
}
