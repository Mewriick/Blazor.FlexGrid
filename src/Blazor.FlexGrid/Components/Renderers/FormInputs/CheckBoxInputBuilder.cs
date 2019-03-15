using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Blazor.FlexGrid.Components.Renderers.FormInputs
{
    public class CheckBoxInputBuilder : IFormInputRendererBuilder
    {
        private readonly EventCallbackFactory eventCallbackFactory;

        public CheckBoxInputBuilder()
        {
            this.eventCallbackFactory = new EventCallbackFactory();
        }

        public Action<IRendererTreeBuilder> BuildRendererTree<TItem>(IActualItemContext<TItem> actualItemContext, PropertyInfo field) where TItem : class
        {
            var localColumnName = actualItemContext.ActualColumnName;
            var value = actualItemContext.GetActualItemColumnValue(localColumnName);

            var valueExpression = Expression.Lambda<Func<bool>>(
                 Expression.Property(
                     Expression.Constant(actualItemContext.ActualItem),
                    field));

            return builder =>
            {
                builder
                    .OpenElement(HtmlTagNames.Div, "checkbox")
                    .OpenElement(HtmlTagNames.Label)
                    .OpenComponent(typeof(InputCheckbox))
                    .AddAttribute("Id", $"create-form-{localColumnName}")
                    .AddAttribute("Class", string.Empty)
                    .AddAttribute("Value", value)
                    .AddAttribute("ValueExpression", valueExpression)
                    .AddAttribute("ValueChanged", eventCallbackFactory.Create<bool>(this, v => actualItemContext.SetActualItemColumnValue(localColumnName, v)))
                    .CloseComponent()
                    .OpenElement(HtmlTagNames.Span, "cr")
                    .OpenElement(HtmlTagNames.I, "cr-icon fa fa-check")
                    .CloseElement()
                    .CloseElement()
                    .CloseElement()
                    .CloseElement()
                    .AddValidationMessage<bool>(valueExpression);
            };
        }

        public bool IsSupportedDateType(Type type)
            => type == typeof(bool);
    }
}
