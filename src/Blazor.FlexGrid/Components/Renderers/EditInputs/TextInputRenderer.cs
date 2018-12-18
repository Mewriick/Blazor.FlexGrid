using Microsoft.AspNetCore.Blazor.Components;

namespace Blazor.FlexGrid.Components.Renderers.EditInputs
{
    public class TextInputRenderer : AbstractEditInputRenderer
    {
        public override void RenderInput(GridRendererContext gridRendererContext)
        {
            var localColumnName = gridRendererContext.ActualColumnName;
            var value = gridRendererContext.PropertyValueAccessor.GetValue(gridRendererContext.ActualItem, gridRendererContext.ActualColumnName);

            gridRendererContext.OpenElement("input");
            gridRendererContext.AddAttribute("type", GetInputType(value.ToString()));
            gridRendererContext.AddAttribute("value", BindMethods.GetValue(value));
            gridRendererContext.AddAttribute("onchange", BindMethods.SetValueHandler(delegate (string __value)
                {
                    gridRendererContext
                        .TableDataSet
                        .EditItemProperty(localColumnName, __value);
                }, value.ToString())
            );

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
