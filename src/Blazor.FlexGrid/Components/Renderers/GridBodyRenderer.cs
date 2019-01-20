using Blazor.FlexGrid.DataSet;
using Blazor.FlexGrid.Permission;
using Microsoft.Extensions.Logging;
using System;

namespace Blazor.FlexGrid.Components.Renderers
{
    public class GridBodyRenderer : GridCompositeRenderer
    {
        private readonly ILogger<GridBodyRenderer> logger;

        public GridBodyRenderer(ILogger<GridBodyRenderer> logger)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected override void RenderInternal(GridRendererContext rendererContext, PermissionContext permissionContext)
        {
            rendererContext.OpenElement(HtmlTagNames.TableBody, rendererContext.CssClasses.TableBody);
            try
            {
                foreach (var item in rendererContext.TableDataSet.Items)
                {
                    rendererContext.ActualItem = item;
                    foreach (var renderer in gridPartRenderers)
                        renderer.Render(rendererContext, permissionContext);
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"Error occured during rendering grid view body. Ex: {ex}");
            }

            rendererContext.CloseElement();
        }

        public override bool CanRender(GridRendererContext rendererContext)
            => rendererContext.TableDataSet.HasItems();
    }
}
