using Microsoft.AspNetCore.Components;
using System;

namespace Blazor.FlexGrid.Components.Renderers.EditInputs
{
    public class SelectInputRenderer : AbstractEditInputRenderer
    {
        public override void RenderInput(IRendererTreeBuilder rendererTreeBuilder, IActualItemContext actualItemContext, Action<string, object> onChangeAction)
        {
            var localColumnName = actualItemContext.ActualColumnName;
            var value = actualItemContext.GetActualItemColumnValue(localColumnName);
            if (value is Enum enumTypeValue)
            {
                var actualStringValue = enumTypeValue.ToString();

                rendererTreeBuilder
                    .OpenElement(HtmlTagNames.Div, "edit-field-wrapper")
                    .OpenElement(HtmlTagNames.Select, "edit-text-field")
                    .AddAttribute(HtmlJSEvents.OnChange, BindMethods.SetValueHandler(delegate (string __value)
                        {
                            var parsedValue = Enum.Parse(value.GetType(), __value);
                            onChangeAction?.Invoke(localColumnName, parsedValue);
                        }, value.ToString())
                    );

                foreach (var enumValue in Enum.GetValues(enumTypeValue.GetType()))
                {
                    var enumStringValue = enumValue.ToString();

                    rendererTreeBuilder.OpenElement(HtmlTagNames.Option);
                    if (enumStringValue == actualStringValue)
                    {
                        rendererTreeBuilder.AddAttribute(HtmlAttributes.Selected, true);
                    }

                    rendererTreeBuilder
                        .AddAttribute(HtmlAttributes.Value, enumStringValue)
                        .AddContent(enumStringValue)
                        .CloseElement();
                }

                rendererTreeBuilder
                    .CloseElement()
                    .CloseElement();
            }
            else
            {
                successor.RenderInput(rendererTreeBuilder, actualItemContext, onChangeAction);
            }
        }
    }
}
