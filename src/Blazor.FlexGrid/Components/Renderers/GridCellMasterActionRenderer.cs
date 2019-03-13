using Blazor.FlexGrid.Permission;
using Microsoft.AspNetCore.Components;

namespace Blazor.FlexGrid.Components.Renderers
{
    public class GridCellMasterActionRenderer : GridPartRenderer
    {
        protected override void BuildRendererTreeInternal(GridRendererContext rendererContext, PermissionContext permissionContext)
        {
            var localActualItem = rendererContext.ActualItem;
            var localActualItemIsSelected = rendererContext.TableDataSet.ItemIsSelected(localActualItem);

            rendererContext.OpenElement(HtmlTagNames.TableColumn, rendererContext.CssClasses.TableCell);
            rendererContext.AddOnClickEvent(() =>
                BindMethods.GetEventHandlerValue((UIMouseEventArgs e) =>
                {
                    rendererContext.TableDataSet.ToggleRowItem(localActualItem);
                    rendererContext.RequestRerenderNotification?.Invoke();
                })
            );

            rendererContext.AddContent(string.Empty);
            rendererContext.OpenElement(HtmlTagNames.Span, "pagination-button-arrow");
            rendererContext.OpenElement(HtmlTagNames.I, localActualItemIsSelected ? "fas fa-angle-down" : "fas fa-angle-right");
            rendererContext.CloseElement();
            rendererContext.CloseElement();
            rendererContext.CloseElement();
        }

        public override bool CanRender(GridRendererContext rendererContext)
            => rendererContext.IsFirstColumn && rendererContext.GridConfiguration.IsMasterTable;
    }
}
