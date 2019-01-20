using Blazor.FlexGrid.Permission;
using System;

namespace Blazor.FlexGrid.Components.Renderers
{
    /// <summary>
    /// The 'Leaf Renderer'
    /// </summary>
    public abstract class GridPartRenderer : IGridRenderer
    {
        public IGridRenderer AddRenderer(IGridRenderer gridPartRenderer, RendererType rendererPosition)
        {
            throw new NotImplementedException();
        }

        public void Render(GridRendererContext rendererContext, PermissionContext permissionContext)
        {
            if (!CanRender(rendererContext))
            {
                return;
            }

            RenderInternal(rendererContext, permissionContext);
        }

        public abstract bool CanRender(GridRendererContext rendererContext);

        protected abstract void RenderInternal(GridRendererContext rendererContext, PermissionContext permissionContext);
    }
}
