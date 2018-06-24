using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;

namespace Blazor.FlexGrid.Components.Renderers
{
    public class GridMesurablePartRenderer : GridPartRenderer
    {
        private readonly IGridRenderer gridPartRenderer;
        private readonly ILogger<GridMesurablePartRenderer> logger;
        private readonly Stopwatch stopwatch;


        public GridMesurablePartRenderer(IGridRenderer gridPartRenderer, ILogger<GridMesurablePartRenderer> logger)
        {
            this.gridPartRenderer = gridPartRenderer ?? throw new ArgumentNullException(nameof(gridPartRenderer));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.stopwatch = new Stopwatch();
        }

        public override void Render(GridRendererContext rendererContext)
        {
            stopwatch.Restart();

            gridPartRenderer.Render(rendererContext);

            stopwatch.Stop();
            logger.LogInformation($"Rendering time [{gridPartRenderer.GetType().FullName}]: {stopwatch.ElapsedMilliseconds} ms");
        }
    }
}
