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

            rendererContext.RenderTreeBuilder.OpenElement(rendererContext.Sequence, HtmlTagNames.Table);
            rendererContext.RenderTreeBuilder.AddAttribute(++rendererContext.Sequence, "class", "table");

            foreach (var renderer in gridPartRenderers)
                renderer.Render(rendererContext);

            rendererContext.RenderTreeBuilder.CloseElement();

            stopwatch.Stop();
            logger.LogInformation($"Rendering time: {stopwatch.ElapsedMilliseconds} ms");
        }
    }
}
