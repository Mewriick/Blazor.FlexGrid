using Blazor.FlexGrid.DataSet;
using Blazor.FlexGrid.Permission;
using Microsoft.Extensions.Logging;
using System;

namespace Blazor.FlexGrid.Components.Renderers
{
    public class GridBodySimpleRenderer : GridCompositeRenderer
    {
        private readonly ILogger<GridBodySimpleRenderer> logger;

        public GridBodySimpleRenderer(ILogger<GridBodySimpleRenderer> logger)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected override void BuildRenderTreeInternal(GridRendererContext rendererContext, PermissionContext permissionContext)
        {
            using (new MeasurableScope(sw => logger.LogInformation($"Grid body rendering duration {sw.ElapsedMilliseconds}ms")))
            {
                rendererContext.OpenElement(HtmlTagNames.TableBody, rendererContext.CssClasses.TableBody);

                try
                {
                    foreach (var item in rendererContext.TableDataSet.Items)
                    {
                        rendererContext.ActualItem = item;
                        foreach (var renderer in gridPartRenderers)
                            renderer.BuildRendererTree(rendererContext, permissionContext);
                    }

                }
                catch (Exception ex)
                {
                    logger.LogError($"Error occured during rendering grid view body. Ex: {ex}");
                }

                rendererContext.CloseElement();
            }
        }

        public override bool CanRender(GridRendererContext rendererContext)
            => rendererContext.TableDataSet.HasItems();
    }
}

