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
