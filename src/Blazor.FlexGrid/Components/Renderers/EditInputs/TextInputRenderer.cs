using Microsoft.AspNetCore.Blazor.Components;

namespace Blazor.FlexGrid.Components.Renderers.EditInputs
{
    public class TextInputRenderer : AbstractEditInputRenderer
    {
        public override void RenderInput(GridRendererContext gridRendererContext)
        {
            var localColumnName = gridRendererContext.ActualColumnName;
            var value = gridRendererContext.PropertyValueAccessor.GetValue(gridRendererContext.ActualItem, gridRendererContext.ActualColumnName);

            gridRendererContext.OpenElement(HtmlTagNames.Div, "edit-field-wrapper");
            gridRendererContext.OpenElement(HtmlTagNames.Input, "edit-text-field");
            gridRendererContext.AddAttribute(HtmlAttributes.Type, GetInputType(value.ToString()));
            gridRendererContext.AddAttribute(HtmlAttributes.Value, BindMethods.GetValue(value));
            gridRendererContext.AddAttribute(HtmlJSEvents.OnChange, BindMethods.SetValueHandler(delegate (string __value)
                {
                    gridRendererContext
                        .TableDataSet
                        .EditItemProperty(localColumnName, __value);
                }, value.ToString())
            );

            gridRendererContext.CloseElement();
            gridRendererContext.CloseElement();
        }

        private string GetInputType(string value)
        {
            if (value.Contains("@"))
            {
                return "email";
            }

            return "text";
        }
    }
}
