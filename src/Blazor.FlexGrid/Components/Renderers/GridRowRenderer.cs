using Blazor.FlexGrid.Permission;

namespace Blazor.FlexGrid.Components.Renderers
{
    public class GridRowRenderer : GridCompositeRenderer
    {
        public override bool CanRender(GridRendererContext rendererContext)
            => true;

        protected override void BuildRenderTreeInternal(GridRendererContext rendererContext, PermissionContext permissionContext)
        {
            rendererContext.OpenElement(HtmlTagNames.TableRow, rendererContext.CssClasses.TableRow);

            foreach (var property in rendererContext.GridItemProperties)
            {
                rendererContext.ActualColumnName = property.Name;
                rendererContext.ActualColumnPropertyCanBeEdited = property.CanWrite;

                gridPartRenderers.ForEach(renderer => renderer.BuildRendererTree(rendererContext, permissionContext));
            }

            rendererContext.CloseElement();

            gridPartRenderersAfter.ForEach(renderer => renderer.BuildRendererTree(rendererContext, permissionContext));
        }
    }
}
