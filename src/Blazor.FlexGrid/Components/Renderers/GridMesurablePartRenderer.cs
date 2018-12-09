using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;

namespace Blazor.FlexGrid.Components.Renderers
{
    public class GridMesurablePartRenderer : GridCompositeRenderer
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

        public override IGridRenderer AddRenderer(IGridRenderer gridPartRenderer, RendererType rendererPosition = RendererType.InsideTag)
            => this.gridPartRenderer.AddRenderer(gridPartRenderer, rendererPosition);

        public override bool CanRender(GridRendererContext rendererContext)
            => gridPartRenderer.CanRender(rendererContext);

        protected override void RenderInternal(GridRendererContext rendererContext)
        {
            stopwatch.Restart();

            gridPartRenderer.Render(rendererContext);

            logger.LogInformation($"Rendering time [{gridPartRenderer.GetType().FullName}]: {stopwatch.ElapsedMilliseconds} ms");
        }
    }
}
