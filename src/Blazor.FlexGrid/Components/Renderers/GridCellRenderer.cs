namespace Blazor.FlexGrid.Components.Renderers
{
    public class GridCellRenderer : GridPartRenderer
    {
        public override void Render(GridRendererContext rendererContext)
        {

            rendererContext.RenderTreeBuilder.OpenElement(++rendererContext.Sequence, HtmlTagNames.TableColumn);
            rendererContext.RenderTreeBuilder.AddAttribute(++rendererContext.Sequence, HtmlAttributes.Class, "table-cell");
            rendererContext.RenderTreeBuilder.AddContent(++rendererContext.Sequence, rendererContext.ValueFormatters[rendererContext.ActualColumnName]
                .FormatValue(rendererContext.GetPropertyValueAccessor.GetValue(rendererContext.ActualItem, rendererContext.ActualColumnName)));

            rendererContext.RenderTreeBuilder.CloseElement();
        }
    }
}
