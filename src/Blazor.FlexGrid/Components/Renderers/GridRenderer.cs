using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Blazor.FlexGrid.Components.Renderers
{
    /// <summary>
    /// The 'Composite Renderer' 
    /// </summary>
    public class GridRenderer : IGridRenderer
    {
        private readonly ILogger<GridRenderer> logger;
        private readonly List<IGridRenderer> gridPartRenderers;
        private readonly Stopwatch stopwatch;

        public GridRenderer(ILogger<GridRenderer> logger)
        {
            this.gridPartRenderers = new List<IGridRenderer>();
            this.stopwatch = new Stopwatch();
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void AddRenderer(IGridRenderer gridPartRenderer)
        {
            gridPartRenderers.Add(gridPartRenderer);
        }

        public void Render(GridRendererContext rendererContext)
        {
            stopwatch.Restart();

            rendererContext.RenderTreeBuilder.OpenElement(rendererContext.Sequence, "table");
            rendererContext.RenderTreeBuilder.AddAttribute(++rendererContext.Sequence, "class", "table");

            foreach (var renderer in gridPartRenderers)
                renderer.Render(rendererContext);

            rendererContext.RenderTreeBuilder.CloseElement();

            stopwatch.Stop();
            logger.LogInformation($"Rendering time: {stopwatch.ElapsedMilliseconds} ms");
        }
    }
}
