namespace Blazor.FlexGrid.Components.Renderers
{
    public class GridRowRenderer : GridCompositeRenderer
    {
        public override void Render(GridRendererContext rendererContext)
        {
            rendererContext.OpenElement(HtmlTagNames.TableRow, "table-row");

            foreach (var property in rendererContext.GridItemProperties)
            {
                rendererContext.ActualColumnName = property.Name;
                gridPartRenderers.ForEach(renderer => renderer.Render(rendererContext));
            }

            rendererContext.CloseElement();
        }
    }
}
