using Blazor.FlexGrid.DataSet;

namespace Blazor.FlexGrid.Components.Renderers
{
    public class GridLoadingRenderer : GridPartRenderer
    {
        public override bool CanRender(GridRendererContext rendererContext)
            => !rendererContext.TableDataSet.HasItems();

        protected override void RenderInternal(GridRendererContext rendererContext)
        {
            rendererContext.AddContent("    ");
            rendererContext.OpenElement("p");
            rendererContext.OpenElement("em");
            rendererContext.AddContent("Loading...");
            rendererContext.CloseElement();
            rendererContext.CloseElement();
            rendererContext.AddContent("\n");
        }
    }
}
