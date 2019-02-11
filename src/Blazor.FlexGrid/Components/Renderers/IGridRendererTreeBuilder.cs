using Blazor.FlexGrid.Permission;

namespace Blazor.FlexGrid.Components.Renderers
{

    /// <summary>
    /// Contract which define 'Renderer component' 
    /// </summary>
    public interface IGridRendererTreeBuilder
    {
        bool CanRender(GridRendererContext rendererContext);

        IGridRendererTreeBuilder AddRenderer(IGridRendererTreeBuilder gridPartRenderer, RendererType rendererPosition = RendererType.InsideTag);

        void BuildRendererTree(GridRendererContext rendererContext, PermissionContext permissionContext);
    }
}
