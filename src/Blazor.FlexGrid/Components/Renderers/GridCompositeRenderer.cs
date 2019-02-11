using Blazor.FlexGrid.Permission;
using System.Collections.Generic;

namespace Blazor.FlexGrid.Components.Renderers
{
    public abstract class GridCompositeRenderer : IGridRendererTreeBuilder
    {
        protected readonly List<IGridRendererTreeBuilder> gridPartRenderers;
        protected readonly List<IGridRendererTreeBuilder> gridPartRenderersBefore;
        protected readonly List<IGridRendererTreeBuilder> gridPartRenderersAfter;

        public GridCompositeRenderer()
        {
            this.gridPartRenderers = new List<IGridRendererTreeBuilder>();
            this.gridPartRenderersBefore = new List<IGridRendererTreeBuilder>();
            this.gridPartRenderersAfter = new List<IGridRendererTreeBuilder>();
        }

        public virtual IGridRendererTreeBuilder AddRenderer(IGridRendererTreeBuilder gridPartRenderer, RendererType rendererPosition = RendererType.InsideTag)
        {
            switch (rendererPosition)
            {
                case RendererType.AfterTag:
                    gridPartRenderersAfter.Add(gridPartRenderer);
                    break;
                case RendererType.BeforeTag:
                    gridPartRenderersBefore.Add(gridPartRenderer);
                    break;
                case RendererType.InsideTag:
                    gridPartRenderers.Add(gridPartRenderer);
                    break;
            }

            return this;
        }

        public void BuildRendererTree(GridRendererContext rendererContext, PermissionContext permissionContext)
        {
            if (!CanRender(rendererContext))
            {
                return;
            }

            BuildRenderTreeInternal(rendererContext, permissionContext);
        }

        public abstract bool CanRender(GridRendererContext rendererContext);

        protected abstract void BuildRenderTreeInternal(GridRendererContext rendererContext, PermissionContext permissionContext);
    }
}
