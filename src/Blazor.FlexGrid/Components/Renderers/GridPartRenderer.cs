using System;

namespace Blazor.FlexGrid.Components.Renderers
{
    /// <summary>
    /// The 'Leaf Renderer'
    /// </summary>
    public abstract class GridPartRenderer : IGridRenderer
    {
        public void AddRenderer(IGridRenderer gridPartRenderer, RendererType rendererPosition)
        {
            throw new NotImplementedException();
        }

        public abstract void Render(GridRendererContext rendererContext);
    }
}
