using Microsoft.AspNetCore.Components;
using System;

namespace Blazor.FlexGrid.Components.Renderers.EditInputs
{
    public class SelectInputRenderer : AbstractEditInputRenderer
    {
        public override void RenderInput(GridRendererContext gridRendererContext)
        {
            var localColumnName = gridRendererContext.ActualColumnName;
            var value = gridRendererContext.PropertyValueAccessor.GetValue(gridRendererContext.ActualItem, localColumnName);
            if (value is Enum enumTypeValue)
            {
                var actualStringValue = enumTypeValue.ToString();

                gridRendererContext.OpenElement(HtmlTagNames.Div, "edit-field-wrapper");
                gridRendererContext.OpenElement(HtmlTagNames.Select, "edit-text-field");
                gridRendererContext.AddAttribute(HtmlJSEvents.OnChange, BindMethods.SetValueHandler(delegate (string __value)
                    {
                        var parsedValue = Enum.Parse(value.GetType(), __value);
                        gridRendererContext
                            .TableDataSet
                            .EditItemProperty(localColumnName, parsedValue);
                    }, value.ToString())
                );

                foreach (var enumValue in Enum.GetValues(enumTypeValue.GetType()))
                {
                    var enumStringValue = enumValue.ToString();

                    gridRendererContext.OpenElement(HtmlTagNames.Option);
                    if (enumStringValue == actualStringValue)
                    {
                        gridRendererContext.AddAttribute(HtmlAttributes.Selected, true);
                    }

                    gridRendererContext.AddAttribute(HtmlAttributes.Value, enumStringValue);
                    gridRendererContext.AddContent(enumStringValue);
                    gridRendererContext.CloseElement();
                }

                gridRendererContext.CloseElement();
                gridRendererContext.CloseElement();
            }
            else
            {
                successor.RenderInput(gridRendererContext);
            }
        }
    }
}
