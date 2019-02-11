using Blazor.FlexGrid.Permission;
using Microsoft.Extensions.Logging;
using System;

namespace Blazor.FlexGrid.Components.Renderers
{
    /// <summary>
    /// The 'Composite Renderer' 
    /// </summary>
    public class GridRenderer : GridCompositeRenderer
    {
        private readonly ILogger<GridRenderer> logger;

        public GridRenderer(ILogger<GridRenderer> logger)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public override bool CanRender(GridRendererContext rendererContext)
            => true;

        protected override void BuildRenderTreeInternal(GridRendererContext rendererContext, PermissionContext permissionContext)
        {
            try
            {
                gridPartRenderersBefore.ForEach(renderer => renderer.BuildRendererTree(rendererContext, permissionContext));

                rendererContext.OpenElement(HtmlTagNames.Div, "table-wrapper");
                rendererContext.OpenElement(HtmlTagNames.Table, rendererContext.CssClasses.Table);

                gridPartRenderers.ForEach(renderer => renderer.BuildRendererTree(rendererContext, permissionContext));

                rendererContext.CloseElement(); // Close table

                gridPartRenderersAfter.ForEach(renderer => renderer.BuildRendererTree(rendererContext, permissionContext));

                rendererContext.CloseElement(); // Close table wrapper
            }
            catch (Exception ex)
            {
                logger.LogError($"Error raised during rendering GridView component. Ex: {ex}");
            }
        }
    }
}
