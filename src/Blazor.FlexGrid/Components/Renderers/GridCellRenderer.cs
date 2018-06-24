namespace Blazor.FlexGrid.Components.Renderers
{
    public class GridCellRenderer : GridPartRenderer
    {
        public override void Render(GridRendererContext rendererContext)
        {
            var builder = rendererContext.RenderTreeBuilder;
            var seq = rendererContext.Sequence;

            builder.OpenElement(++seq, HtmlTagNames.TableColumn);
            builder.AddContent(++seq, rendererContext.ValueFormatters[rendererContext.ActualColumnName]
                .FormatValue(rendererContext.GetPropertyValueAccessor.GetValue(rendererContext.ActualItem, rendererContext.ActualColumnName)));

            builder.CloseElement();
        }
    }
}
