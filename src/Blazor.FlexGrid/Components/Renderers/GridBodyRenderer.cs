namespace Blazor.FlexGrid.Components.Renderers
{
    public class GridBodyRenderer : IGridRenderer
    {
        public void Render(GridRendererContext rendererContext)
        {
            var builder = rendererContext.RenderTreeBuilder;
            var seq = rendererContext.Sequence;

            builder.OpenElement(++seq, "tbody");
            foreach (var item in rendererContext.TableDataSet.Items)
            {
                builder.OpenElement(++seq, "tr");
                foreach (var property in rendererContext.GridItemProperties)
                {
                    builder.OpenElement(++seq, "td");
                    builder.AddContent(++seq, property.GetValue(item).ToString());
                    builder.CloseElement();
                }

                builder.CloseElement();
            }

            builder.CloseElement();

            rendererContext.Sequence = seq;
        }
    }
}
