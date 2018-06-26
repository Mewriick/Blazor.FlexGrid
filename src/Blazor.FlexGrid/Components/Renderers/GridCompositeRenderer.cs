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

        public void AddRenderer(IGridRenderer gridPartRenderer, RendererType rendererPosition = RendererType.InsideTag)
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
        }

        public abstract void Render(GridRendererContext rendererContext);
    }
}
