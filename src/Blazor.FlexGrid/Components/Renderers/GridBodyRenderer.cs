using Blazor.FlexGrid.DataSet;
using Blazor.FlexGrid.Permission;
using System;

namespace Blazor.FlexGrid.Components.Renderers
{
    public class GridBodyRenderer : GridPartRenderer
    {
        private readonly IGridRendererTreeBuilder simpleBodyRendererTreeBuilder;
        private readonly IGridRendererTreeBuilder groupedBodyRendererTreeBuilder;

        public GridBodyRenderer(
            IGridRendererTreeBuilder simpleBodyRendererTreeBuilder,
            IGridRendererTreeBuilder groupedBodyRendererTreeBuilder)
        {
            this.simpleBodyRendererTreeBuilder = simpleBodyRendererTreeBuilder ?? throw new ArgumentNullException(nameof(simpleBodyRendererTreeBuilder));
            this.groupedBodyRendererTreeBuilder = groupedBodyRendererTreeBuilder ?? throw new ArgumentNullException(nameof(groupedBodyRendererTreeBuilder));
        }

        public override bool CanRender(GridRendererContext rendererContext)
            => rendererContext.TableDataSet.HasItems();

        protected override void BuildRendererTreeInternal(GridRendererContext rendererContext, PermissionContext permissionContext)
        {
            if (rendererContext.TableDataSet.GroupingOptions.IsGroupingActive)
            {
                groupedBodyRendererTreeBuilder.BuildRendererTree(rendererContext, permissionContext);
            }
            else
            {
                simpleBodyRendererTreeBuilder.BuildRendererTree(rendererContext, permissionContext);
            }
        }
    }
}
