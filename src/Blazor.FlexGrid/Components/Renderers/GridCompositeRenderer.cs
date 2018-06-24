using System.Collections.Generic;

namespace Blazor.FlexGrid.Components.Renderers
{
    public class GridCompositeRenderer : IGridRenderer
    {
        protected readonly List<IGridRenderer> gridPartRenderers;

        public GridCompositeRenderer()
        {
            this.gridPartRenderers = new List<IGridRenderer>();
        }

        public void AddRenderer(IGridRenderer gridPartRenderer)
        {
            gridPartRenderers.Add(gridPartRenderer);
        }

        public virtual void Render(GridRendererContext rendererContext)
        {
            foreach (var renderer in gridPartRenderers)
                renderer.Render(rendererContext);
        }
    }
}
