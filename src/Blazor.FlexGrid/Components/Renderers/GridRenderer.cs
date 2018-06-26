using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;

namespace Blazor.FlexGrid.Components.Renderers
{
    /// <summary>
    /// The 'Composite Renderer' 
    /// </summary>
    public class GridRenderer : GridCompositeRenderer
    {
        private readonly ILogger<GridRenderer> logger;
        private readonly Stopwatch stopwatch;

        public GridRenderer(ILogger<GridRenderer> logger)
        {
            this.stopwatch = new Stopwatch();
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public override void Render(GridRendererContext rendererContext)
        {
            stopwatch.Restart();

            gridPartRenderersBefore.ForEach(renderer => renderer.Render(rendererContext));

            rendererContext.OpenElement(HtmlTagNames.Div, "table-wrapper");
            rendererContext.OpenElement(HtmlTagNames.Table, "flex-table");

            gridPartRenderers.ForEach(renderer => renderer.Render(rendererContext));

            rendererContext.CloseElement(); // Close table

            gridPartRenderersAfter.ForEach(renderer => renderer.Render(rendererContext));

            rendererContext.CloseElement(); // Close table wrapper

            stopwatch.Stop();
            logger.LogInformation($"Rendering time: {stopwatch.ElapsedMilliseconds} ms");
        }
    }
}
