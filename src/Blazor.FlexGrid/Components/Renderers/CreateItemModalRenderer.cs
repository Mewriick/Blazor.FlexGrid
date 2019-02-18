using Blazor.FlexGrid.Components.Configuration;
using Blazor.FlexGrid.Permission;
using Microsoft.AspNetCore.Components;

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

            rendererContext.OpenElement(HtmlTagNames.Div, "modal");
            rendererContext.AddAttribute(HtmlAttributes.Id, CreateItemOptions.CreateItemModalName);
            rendererContext.AddAttribute("role", "dialog");

            rendererContext.OpenElement(HtmlTagNames.Div, "modal-dialog modal-lg modal-dialog-centered");
            rendererContext.OpenElement(HtmlTagNames.Div, "modal-content");

            rendererContext.OpenElement(HtmlTagNames.Div, "modal-header");
            rendererContext.OpenElement(HtmlTagNames.H4, "modal-title");
            rendererContext.AddContent("Create Item");
            rendererContext.CloseElement();
            rendererContext.OpenElement(HtmlTagNames.Button, "close");
            rendererContext.AddOnClickEvent(() =>
                BindMethods.GetEventHandlerValue((UIMouseEventArgs e) =>
                    FlexGridInterop.HideModal(CreateItemOptions.CreateItemModalName)));
            rendererContext.AddAttribute(HtmlAttributes.Type, "button");
            rendererContext.AddAttribute("data-dismiss", "modal");
            rendererContext.AddAttribute("aria-label", "Close");
            rendererContext.OpenElement(HtmlTagNames.Span);
            rendererContext.AddAttribute(HtmlAttributes.AriaHidden, "true");
            rendererContext.AddContent("&times;");
            rendererContext.CloseElement();
            rendererContext.CloseElement();
            rendererContext.CloseElement();

            rendererContext.OpenElement(HtmlTagNames.Div, "modal-body");
            rendererContext.AddCreateItemComponent();

            rendererContext.CloseElement();
            rendererContext.CloseElement();
            rendererContext.CloseElement();
            rendererContext.CloseElement();
        }
    }
}
