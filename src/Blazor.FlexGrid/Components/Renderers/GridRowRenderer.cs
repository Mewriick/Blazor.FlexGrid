namespace Blazor.FlexGrid.Components.Renderers
{
    public class GridRowRenderer : GridCompositeRenderer
    {
        public override void Render(GridRendererContext rendererContext)
        {
            rendererContext.RenderTreeBuilder.OpenElement(++rendererContext.Sequence, HtmlTagNames.TableRow);
            rendererContext.RenderTreeBuilder.AddAttribute(++rendererContext.Sequence, HtmlAttributes.Class, "table-row");

            foreach (var property in rendererContext.GridItemProperties)
            {
                rendererContext.ActualColumnName = property.Name;

                foreach (var renderer in gridPartRenderers)
                    renderer.Render(rendererContext);
            }

            rendererContext.RenderTreeBuilder.CloseElement();
        }
    }
}
