using Microsoft.AspNetCore.Blazor.Components;
using System;

namespace Blazor.FlexGrid.Components.Renderers.EditInputs
{
    public class DateTimeInputRenderer : AbstractEditInputRenderer
    {
        public override void RenderInput(GridRendererContext gridRendererContext)
        {
            var localItem = gridRendererContext.ActualItem;
            var localColumnName = gridRendererContext.ActualColumnName;
            var value = gridRendererContext.PropertyValueAccessor.GetValue(localItem, localColumnName);
            if (value is DateTime dateTimeValue)
            {
                gridRendererContext.OpenElement("input");
                gridRendererContext.AddAttribute("type", "date");
                gridRendererContext.AddAttribute("value", BindMethods.GetValue(dateTimeValue, "yyyy-MM-dd"));
                gridRendererContext.AddAttribute("onchange", BindMethods.SetValueHandler(delegate (DateTime __value)
                    {
                        gridRendererContext
                            .PropertyValueAccessor
                            .SetValue(localItem, localColumnName, __value);
                    }, dateTimeValue, "yyyy-MM-dd")
                );

                gridRendererContext.CloseElement();
            }
            else
            {
                successor.RenderInput(gridRendererContext);
            }
        }
    }
}
