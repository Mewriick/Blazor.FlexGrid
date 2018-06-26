namespace Blazor.FlexGrid.Components.Renderers
{
    public class GridCellRenderer : GridPartRenderer
    {
        public override void Render(GridRendererContext rendererContext)
        {
            rendererContext.OpenElement(HtmlTagNames.TableColumn, "table-cell");
            rendererContext.AddActualColumnValue();
            rendererContext.CloseElement();
        }
    }
}
