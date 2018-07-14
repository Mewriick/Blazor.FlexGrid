using Microsoft.AspNetCore.Blazor;
using Microsoft.AspNetCore.Blazor.Components;

namespace Blazor.FlexGrid.Components.Renderers
{
    public class GridCellRenderer : GridPartRenderer
    {
        public override void Render(GridRendererContext rendererContext)
        {
            rendererContext.OpenElement(HtmlTagNames.TableColumn, rendererContext.CssClasses.TableCell);
            rendererContext.AddOnClickEvent(() => BindMethods.GetEventHandlerValue((UIMouseEventArgs async) =>
                    rendererContext.TableDataSet.ToggleRowItem(rendererContext.ActualItem))
                );

            rendererContext.AddActualColumnValue();
            rendererContext.CloseElement();
        }
    }
}
