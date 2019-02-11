using Blazor.FlexGrid.Permission;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;

namespace Blazor.FlexGrid.Components.Renderers
{
    public class GridMesurablePartRenderer : GridCompositeRenderer
    {
        private readonly IGridRendererTreeBuilder gridPartRenderer;
        private readonly ILogger<GridMesurablePartRenderer> logger;
        private readonly Stopwatch stopwatch;


        public GridMesurablePartRenderer(IGridRendererTreeBuilder gridPartRenderer, ILogger<GridMesurablePartRenderer> logger)
        {
            this.gridPartRenderer = gridPartRenderer ?? throw new ArgumentNullException(nameof(gridPartRenderer));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.stopwatch = new Stopwatch();
        }

        public override IGridRendererTreeBuilder AddRenderer(IGridRendererTreeBuilder gridPartRenderer, RendererType rendererPosition = RendererType.InsideTag)
            => this.gridPartRenderer.AddRenderer(gridPartRenderer, rendererPosition);

        public override bool CanRender(GridRendererContext rendererContext)
            => gridPartRenderer.CanRender(rendererContext);

        protected override void BuildRenderTreeInternal(GridRendererContext rendererContext, PermissionContext permissionContext)
        {
            stopwatch.Restart();

            gridPartRenderer.BuildRendererTree(rendererContext, permissionContext);

            logger.LogInformation($"Rendering time [{gridPartRenderer.GetType().FullName}]: {stopwatch.ElapsedMilliseconds} ms");
        }
    }
}
