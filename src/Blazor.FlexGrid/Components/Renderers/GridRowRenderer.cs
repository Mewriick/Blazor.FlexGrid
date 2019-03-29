using Blazor.FlexGrid.Permission;
using System.Linq;

namespace Blazor.FlexGrid.Components.Renderers
{
    public class GridRowRenderer : GridCompositeRenderer
    {
        public override bool CanRender(GridRendererContext rendererContext)
            => !rendererContext.ActualItem.GetType().GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IGrouping<,>));
            

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
