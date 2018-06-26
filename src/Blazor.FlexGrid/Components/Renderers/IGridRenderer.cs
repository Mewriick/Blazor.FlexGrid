namespace Blazor.FlexGrid.Components.Renderers
{

    /// <summary>
    /// Contract which define 'Renderer component' 
    /// </summary>
    public interface IGridRenderer
    {
        void AddRenderer(IGridRenderer gridPartRenderer, RendererType rendererPosition);

        void Render(GridRendererContext rendererContext);
    }
}
