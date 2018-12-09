namespace Blazor.FlexGrid.Components.Renderers
{

    /// <summary>
    /// Contract which define 'Renderer component' 
    /// </summary>
    public interface IGridRenderer
    {
        bool CanRender(GridRendererContext rendererContext);

        IGridRenderer AddRenderer(IGridRenderer gridPartRenderer, RendererType rendererPosition = RendererType.InsideTag);

        void Render(GridRendererContext rendererContext);
    }
}
