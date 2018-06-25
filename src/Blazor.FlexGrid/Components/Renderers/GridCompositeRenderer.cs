using System.Collections.Generic;

namespace Blazor.FlexGrid.Components.Renderers
{
    public abstract class GridCompositeRenderer : IGridRenderer
    {
        protected readonly List<IGridRenderer> gridPartRenderers;
        protected readonly List<IGridRenderer> gridPartRenderersBefore;
        protected readonly List<IGridRenderer> gridPartRenderersAfter;

        public GridCompositeRenderer()
        {
            this.gridPartRenderers = new List<IGridRenderer>();
            this.gridPartRenderersBefore = new List<IGridRenderer>();
            this.gridPartRenderersAfter = new List<IGridRenderer>();
        }

        public void AddRenderer(IGridRenderer gridPartRenderer, RendererPosition rendererPosition = RendererPosition.Default)
        {
            switch (rendererPosition)
            {
                case RendererPosition.After:
                    gridPartRenderersAfter.Add(gridPartRenderer);
                    break;
                case RendererPosition.Before:
                    gridPartRenderersBefore.Add(gridPartRenderer);
                    break;
                case RendererPosition.Default:
                    gridPartRenderers.Add(gridPartRenderer);
                    break;
            }
        }

        public abstract void Render(GridRendererContext rendererContext);
    }
}
