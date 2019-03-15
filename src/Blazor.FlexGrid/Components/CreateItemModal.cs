using Blazor.FlexGrid.Components.Configuration;
using Blazor.FlexGrid.Components.Events;
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

        [Parameter] CreateFormCssClasses CreateFormCssClasses { get; set; }

        [Parameter] Action<ItemCreatedArgs> NewItemCreated { get; set; }


        [Inject]
        private FlexGridInterop FlexGridInterop { get; set; }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            if (!CreateItemOptions.IsCreateItemAllowed ||
                !PermissionContext.HasCreateItemPermission)
            {
                return;
            }

            base.BuildRenderTree(builder);

            var internalBuilder = new BlazorRendererTreeBuilder(builder);

            var createItemContext = new CreateItemContext(CreateItemOptions, CreateFormCssClasses);
            createItemContext.OnItemCreated += (object sender, ItemCreatedArgs e) => NewItemCreated?.Invoke(e);

            internalBuilder
                .OpenElement(HtmlTagNames.Div, "modal")
                .AddAttribute(HtmlAttributes.Id, CreateItemOptions.CreateItemModalName)
                .AddAttribute("role", "dialog")
                .OpenElement(HtmlTagNames.Div, $"modal-dialog modal-dialog-centered {CreateFormCssClasses.ModalSize}")
                .AddAttribute(HtmlAttributes.Id, CreateItemOptions.CreateItemModalSizeDiv)
                .OpenElement(HtmlTagNames.Div, "modal-content")
                .OpenElement(HtmlTagNames.Div, CreateFormCssClasses.ModalHeader)
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
                .OpenElement(HtmlTagNames.Div, CreateFormCssClasses.ModalBody)
                .OpenComponent(typeof(CreateItemForm<,>)
                    .MakeGenericType(CreateItemOptions.ModelType, CreateItemOptions.OutputDtoType))
                .AddAttribute(nameof(CreateItemContext), createItemContext)
                .CloseComponent()
                .CloseElement()
                .CloseElement()
                .CloseElement()
                .CloseElement();
        }
    }
}
