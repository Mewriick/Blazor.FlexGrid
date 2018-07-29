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
            var localActualItemIsSelected = rendererContext.TableDataSet.ItemIsSelected(localActualItem);

            rendererContext.OpenElement(HtmlTagNames.TableColumn, rendererContext.CssClasses.TableCell);
            rendererContext.AddOnClickEvent(() =>
                BindMethods.GetEventHandlerValue((UIMouseEventArgs e) =>
                    rendererContext.TableDataSet.ToggleRowItem(localActualItem))
            );

            rendererContext.AddContent(string.Empty);
            rendererContext.OpenElement(HtmlTagNames.Span, "pagination-button-arrow");
            rendererContext.OpenElement(HtmlTagNames.I, localActualItemIsSelected ? "fas fa-angle-down" : "fas fa-angle-right");
            rendererContext.CloseElement();
            rendererContext.CloseElement();
            rendererContext.CloseElement();
        }
    }
}
