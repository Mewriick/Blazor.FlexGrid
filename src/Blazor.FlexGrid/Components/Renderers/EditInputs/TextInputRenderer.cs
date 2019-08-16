using Microsoft.AspNetCore.Components;
using System;

namespace Blazor.FlexGrid.Components.Renderers.EditInputs
{
    public class TextInputRenderer : AbstractEditInputRenderer
    {
        private const string InputTypeText = "text";
        private const string InputTypeEmail = "email";

        public override void BuildInputRendererTree(IRendererTreeBuilder rendererTreeBuilder, IActualItemContext<object> actualItemContext, Action<string, object> onChangeAction)
        {
            var localColumnName = actualItemContext.ActualColumnName;
            var value = actualItemContext.GetActualItemColumnValue(localColumnName);

            rendererTreeBuilder
                .OpenElement(HtmlTagNames.Div, "edit-field-wrapper")
                .OpenElement(HtmlTagNames.Input, "edit-text-field")
                .AddAttribute(HtmlAttributes.Type, GetInputType(value?.ToString() ?? InputTypeText))
                .AddAttribute(HtmlAttributes.Value, BindConverter.FormatValue(value))
                .AddAttribute(HtmlJSEvents.OnChange, EventCallback.Factory.Create(this,
                    (UIChangeEventArgs e) => onChangeAction?.Invoke(localColumnName, BindConverterExtensions.ConvertTo(e.Value, string.Empty)))
                )
                .CloseElement()
                .CloseElement();
        }

        private string GetInputType(string value)
        {
            if (value.Contains("@"))
            {
                return InputTypeEmail;
            }

            return InputTypeText;
        }
    }
}
