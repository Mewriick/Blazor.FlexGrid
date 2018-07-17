using Microsoft.AspNetCore.Blazor;
using Microsoft.AspNetCore.Blazor.Components;

namespace Blazor.FlexGrid.Components.Renderers
{
    public class GridCellMasterActionRenderer : GridPartRenderer
    {
        public override void Render(GridRendererContext rendererContext)
        {
            if (!rendererContext.IsFirstColumn || !rendererContext.GridConfiguration.IsMasterTable)
            {
                return;
            }

            var localActualItem = rendererContext.ActualItem;
            rendererContext.OpenElement(HtmlTagNames.TableColumn, rendererContext.CssClasses.TableCell);
            rendererContext.AddOnClickEvent(() => BindMethods.GetEventHandlerValue((UIMouseEventArgs async) =>
            {
                rendererContext.TableDataSet.ToggleRowItem(localActualItem);
            }));

            rendererContext.AddContent(">");
            rendererContext.CloseElement();
        }
    }
}
