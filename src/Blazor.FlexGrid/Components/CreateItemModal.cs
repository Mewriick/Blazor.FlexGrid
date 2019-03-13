using Blazor.FlexGrid.Components.Configuration;
using Blazor.FlexGrid.Components.Renderers;
using Blazor.FlexGrid.Permission;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.RenderTree;
using System;

namespace Blazor.FlexGrid.Components
{
    public class CreateItemModal : ComponentBase
    {
        [Parameter] CreateItemOptions CreateItemOptions { get; set; }

        [Parameter] PermissionContext PermissionContext { get; set; }

        [Inject]
        private FlexGridInterop FlexGridInterop { get; set; }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            Console.WriteLine(CreateItemOptions.IsCreateItemAllowed);

            if (!CreateItemOptions.IsCreateItemAllowed ||
                !PermissionContext.HasCreateItemPermission)
            {
                return;
            }

            base.BuildRenderTree(builder);

            var internalBuilder = new BlazorRendererTreeBuilder(builder);

            internalBuilder
                .OpenElement(HtmlTagNames.Div, "modal")
                .AddAttribute(HtmlAttributes.Id, CreateItemOptions.CreateItemModalName)
                .AddAttribute("role", "dialog")
                .OpenElement(HtmlTagNames.Div, "modal-dialog modal modal-dialog-centered")
                .OpenElement(HtmlTagNames.Div, "modal-content")
                .OpenElement(HtmlTagNames.Div, "modal-header")
                .OpenElement(HtmlTagNames.H4, "modal-title")
                .AddContent("Create Item")
                .CloseElement()
                .OpenElement(HtmlTagNames.Button, "close")
                .AddAttribute(HtmlJSEvents.OnClick, BindMethods.GetEventHandlerValue((UIMouseEventArgs e) => FlexGridInterop.HideModal(CreateItemOptions.CreateItemModalName)))
                .AddAttribute(HtmlAttributes.Type, "button")
                .AddAttribute("data-dismiss", "modal")
                .AddAttribute("aria-label", "Close")
                .OpenElement(HtmlTagNames.Span)
                .AddAttribute(HtmlAttributes.AriaHidden, "true")
                .AddContent(new MarkupString("&times;"))
                .CloseElement()
                .CloseElement()
                .CloseElement()
                .OpenElement(HtmlTagNames.Div, "modal-body")
                .OpenComponent(typeof(CreateItemForm<,>)
                    .MakeGenericType(CreateItemOptions.ModelType, CreateItemOptions.OutputDtoType))
                .AddAttribute(nameof(CreateItemContext), new CreateItemContext(CreateItemOptions))
                .CloseComponent()
                .CloseElement()
                .CloseElement()
                .CloseElement()
                .CloseElement();
        }
    }
}
