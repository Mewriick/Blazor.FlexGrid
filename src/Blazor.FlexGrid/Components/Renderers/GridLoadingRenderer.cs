using Blazor.FlexGrid.DataSet;

namespace Blazor.FlexGrid.Components.Renderers
{
    public class GridLoadingRenderer : GridPartRenderer
    {
        public override void Render(GridRendererContext rendererContext)
        {
            if (rendererContext.TableDataSet.HasItems())
            {
                return;
            }

            rendererContext.RenderTreeBuilder.AddContent(++rendererContext.Sequence, "    ");
            rendererContext.RenderTreeBuilder.OpenElement(++rendererContext.Sequence, "p");
            rendererContext.RenderTreeBuilder.OpenElement(++rendererContext.Sequence, "em");
            rendererContext.RenderTreeBuilder.AddContent(++rendererContext.Sequence, "Loading...");
            rendererContext.RenderTreeBuilder.CloseElement();
            rendererContext.RenderTreeBuilder.CloseElement();
            rendererContext.RenderTreeBuilder.AddContent(++rendererContext.Sequence, "\n");
        }
    }
}
