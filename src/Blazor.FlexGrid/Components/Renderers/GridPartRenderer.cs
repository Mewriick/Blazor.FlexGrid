using System;

namespace Blazor.FlexGrid.Components.Renderers
{
    /// <summary>
    /// The 'Leaf Renderer'
    /// </summary>
    public abstract class GridPartRenderer : IGridRenderer
    {
        public void AddRenderer(IGridRenderer gridPartRenderer)
        {
            throw new InvalidOperationException("Cannot add to a leaf");
        }

        public abstract void Render(GridRendererContext rendererContext);
    }
}
