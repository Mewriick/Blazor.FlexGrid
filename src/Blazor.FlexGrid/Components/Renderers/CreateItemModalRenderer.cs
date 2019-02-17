using Blazor.FlexGrid.Components.Configuration;
using Blazor.FlexGrid.Permission;

namespace Blazor.FlexGrid.Components.Renderers
{
    public class CreateItemModalRenderer : GridPartRenderer
    {
        public override bool CanRender(GridRendererContext rendererContext)
            => rendererContext.GridConfiguration.CreateItemOptions.IsCreateItemAllowed;

        protected override void BuildRendererTreeInternal(GridRendererContext rendererContext, PermissionContext permissionContext)
        {
            if (!permissionContext.HasCreateItemPermission)
            {
                return;
            }

            rendererContext.OpenElement(HtmlTagNames.Div, "modal fade");
            rendererContext.AddAttribute(HtmlAttributes.Id, CreateItemOptions.CreateItemModalName);
            rendererContext.AddAttribute("data-backdrop", "static");
            rendererContext.OpenElement(HtmlTagNames.Div, "modal-dialog");
            rendererContext.OpenElement(HtmlTagNames.Div, "modal-content");
            rendererContext.OpenElement(HtmlTagNames.Div, "modal-body");

            rendererContext.AddCreateItemComponent();

            rendererContext.CloseElement();
            rendererContext.CloseElement();
            rendererContext.CloseElement();
            rendererContext.CloseElement();
        }
    }
}
