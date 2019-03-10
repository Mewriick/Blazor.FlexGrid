using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Blazor.FlexGrid.Components.Renderers.FormInputs
{
    public class TextInputBuilder : IFormInputRendererBuilder
    {
        private readonly EventCallbackFactory eventCallbackFactory;

        public TextInputBuilder()
        {
            this.eventCallbackFactory = new EventCallbackFactory();
        }

        public Action<IRendererTreeBuilder> BuildRendererTree<TItem>(IActualItemContext<TItem> actualItemContext, PropertyInfo field) where TItem : class
        {
            var localColumnName = actualItemContext.ActualColumnName;
            var value = actualItemContext.GetActualItemColumnValue(localColumnName);

            var valueExpression = Expression.Lambda<Func<string>>(
                 Expression.Property(
                     Expression.Constant(actualItemContext.ActualItem),
                    field));

            return builder =>
            {
                builder
                    .OpenElement(HtmlTagNames.Div, "edit-field-wrapper")
                    .OpenComponent(typeof(InputText))
                    .AddAttribute("Id", $"create-form-{localColumnName}")
                    .AddAttribute("Class", "edit-text-field")
                    .AddAttribute("Value", value)
                    .AddAttribute("ValueExpression", valueExpression)
                    .AddAttribute("ValueChanged", eventCallbackFactory.Create<string>(this, v => actualItemContext.SetActualItemColumnValue(localColumnName, v)))
                    .CloseComponent()
                    .AddValidationMessage<string>(valueExpression)
                    .CloseElement();
            };
        }

        public bool IsSupportedDateType(Type type)
            => type == typeof(string);
    }
}
