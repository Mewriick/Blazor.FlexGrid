namespace Blazor.FlexGrid.Components.Renderers
{
    public class GridLoadingRenderer : GridPartRenderer
    {
        public override void Render(GridRendererContext rendererContext)
        {
            var seq = rendererContext.Sequence;
            var builder = rendererContext.RenderTreeBuilder;

            if (rendererContext.TableDataSet?.Items == null ||
                rendererContext.TableDataSet.Items.Count <= 0)
            {
                builder.AddContent(++seq, "    ");
                builder.OpenElement(++seq, "p");
                builder.OpenElement(++seq, "em");
                builder.AddContent(++seq, "Loading...");
                builder.CloseElement();
                builder.CloseElement();
                builder.AddContent(++seq, "\n");
            }

            rendererContext.Sequence = seq;
        }
    }
}
