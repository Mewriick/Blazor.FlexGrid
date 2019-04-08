using Blazor.FlexGrid.DataSet;
using Blazor.FlexGrid.Permission;

namespace Blazor.FlexGrid.Components.Renderers
{
    public class GridEmptyItemsRenderer : GridPartRenderer
    {
        public override bool CanRender(GridRendererContext rendererContext)
            => !rendererContext.TableDataSet.HasItems();

        protected override void BuildRendererTreeInternal(GridRendererContext rendererContext, PermissionContext permissionContext)
        {
            if (!rendererContext.TableDataSet.FilterIsApplied)
            {
                rendererContext.OpenElement(HtmlTagNames.Div, "table-info-text table-info-text-small");
                rendererContext.AddContent("Loading...");
                rendererContext.CloseElement();
            }
            else
            {
                rendererContext.OpenElement(HtmlTagNames.TableRow);
                rendererContext.OpenElement(HtmlTagNames.TableColumn);
                rendererContext.AddAttribute(HtmlAttributes.Colspan, rendererContext.GridItemProperties.Count);
                rendererContext.OpenElement(HtmlTagNames.Div, "table-info-text");
                rendererContext.OpenElement(HtmlTagNames.Span);
                rendererContext.OpenElement(HtmlTagNames.I, "fas fa-search");
                rendererContext.CloseElement();
                rendererContext.CloseElement();
                rendererContext.AddMarkupContent("\t No matching items found");
                rendererContext.CloseElement();
                rendererContext.CloseElement();
                rendererContext.CloseElement();
            }
        }
    }
}
