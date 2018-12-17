using Microsoft.AspNetCore.Blazor;
using Microsoft.AspNetCore.Blazor.Components;

namespace Blazor.FlexGrid.Components.Renderers
{
    public class GridActionButtonsRenderer : GridPartRenderer
    {
        public override bool CanRender(GridRendererContext rendererContext)
            => rendererContext.IsLastColumn && rendererContext.GridConfiguration.InlineEditIsAllowed;

        protected override void RenderInternal(GridRendererContext rendererContext)
        {
            var localActualItem = rendererContext.ActualItem;
            rendererContext.OpenElement(HtmlTagNames.TableColumn, rendererContext.CssClasses.TableCell);
            rendererContext.AddOnClickEvent(() =>
                BindMethods.GetEventHandlerValue((UIMouseEventArgs e) =>
                    rendererContext.TableDataSet.EditItem(localActualItem))
            );

            rendererContext.AddContent(string.Empty);
            rendererContext.OpenElement(HtmlTagNames.I, "fas fa-edit");
            rendererContext.CloseElement();
            rendererContext.CloseElement();
        }
    }
}
