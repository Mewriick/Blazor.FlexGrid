using Blazor.FlexGrid.Permission;

namespace Blazor.FlexGrid.Components.Renderers
{
    public class GridRowRenderer : GridCompositeRenderer
    {
        public override bool CanRender(GridRendererContext rendererContext)
            => true;

        protected override void RenderInternal(GridRendererContext rendererContext, PermissionContext permissionContext)
        {
            rendererContext.OpenElement(HtmlTagNames.TableRow, rendererContext.CssClasses.TableRow);

            foreach (var property in rendererContext.GridItemProperties)
            {
                rendererContext.ActualColumnName = property.Name;
                rendererContext.ActualColumnPropertyCanBeEdited = property.CanWrite;

                gridPartRenderers.ForEach(renderer => renderer.Render(rendererContext, permissionContext));
            }

            rendererContext.CloseElement();

            gridPartRenderersAfter.ForEach(renderer => renderer.Render(rendererContext, permissionContext));
        }
    }
}
