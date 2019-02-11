using Blazor.FlexGrid.Permission;
using System;

namespace Blazor.FlexGrid.Components.Renderers
{
    /// <summary>
    /// The 'Leaf Renderer'
    /// </summary>
    public abstract class GridPartRenderer : IGridRendererTreeBuilder
    {
        public IGridRendererTreeBuilder AddRenderer(IGridRendererTreeBuilder gridPartRenderer, RendererType rendererPosition)
        {
            throw new NotImplementedException();
        }

        public void BuildRendererTree(GridRendererContext rendererContext, PermissionContext permissionContext)
        {
            if (!CanRender(rendererContext))
            {
                return;
            }

            BuildRendererTreeInternal(rendererContext, permissionContext);
        }

        public abstract bool CanRender(GridRendererContext rendererContext);

        protected abstract void BuildRendererTreeInternal(GridRendererContext rendererContext, PermissionContext permissionContext);
    }
}
