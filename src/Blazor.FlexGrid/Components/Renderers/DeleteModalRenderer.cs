using Blazor.FlexGrid.Components.Configuration;
using Blazor.FlexGrid.Permission;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;

namespace Blazor.FlexGrid.Components.Renderers
{
    public class DeleteModalRenderer : GridPartRenderer
    {
        private readonly FlexGridInterop flexGridInterop;

        public DeleteModalRenderer(FlexGridInterop flexGridInterop)
        {
            this.flexGridInterop = flexGridInterop ?? throw new ArgumentNullException(nameof(flexGridInterop));
        }

        public override bool CanRender(GridRendererContext rendererContext)
        {
            return true;
        }

        protected override void BuildRendererTreeInternal(GridRendererContext rendererContext, PermissionContext permissionContext)
        {
            rendererContext
                .RendererTreeBuilder
                .OpenElement(HtmlTagNames.Div, "modal")
                .AddAttribute(HtmlAttributes.Id, DeleteItemOptions.DialogName)
                .AddAttribute("role", "dialog")
                .OpenElement(HtmlTagNames.Div, $"modal-dialog modal-dialog-centered")
                .AddAttribute(HtmlAttributes.Id, CreateItemOptions.CreateItemModalSizeDiv)
                .OpenElement(HtmlTagNames.Div, "modal-content")
                .OpenElement(HtmlTagNames.Div, "modal-header")
                .OpenElement(HtmlTagNames.H4, "modal-title")
                .AddContent("Confirm delete")
                .CloseElement()
                .CloseElement()
                .OpenElement(HtmlTagNames.Div, "modal-body")
                .AddContent("Are you sure you want to delete item?")
                .CloseElement()
                .OpenElement(HtmlTagNames.Div, "modal-footer")
                .OpenElement(HtmlTagNames.Button, rendererContext.CssClasses.DeleteDialogCssClasses.CancelButton)
                .AddAttribute(HtmlAttributes.Type, "button")
                .AddAttribute("data-dismiss", "modal")
                .AddAttribute(HtmlJSEvents.OnClick, EventCallback.Factory.Create(this, (MouseEventArgs e) => flexGridInterop.HideModal(DeleteItemOptions.DialogName)))
                .AddContent("Cancel")
                .CloseElement()
                .OpenElement(HtmlTagNames.Button, rendererContext.CssClasses.DeleteDialogCssClasses.DeleteButton)
                .AddAttribute(HtmlAttributes.Type, "button")
                .AddAttribute("data-dismiss", "modal")
                .AddAttribute(HtmlJSEvents.OnClick,
                    EventCallback.Factory.Create(this, (MouseEventArgs e) =>
                    {
                        rendererContext.TableDataSet.DeleteItem(rendererContext.FlexGridContext.SelectedItem);
                        rendererContext.FlexGridContext.RemoveSelection();
                        flexGridInterop.HideModal(DeleteItemOptions.DialogName);
                        rendererContext.RequestRerenderNotification?.Invoke();
                    }))
                .AddContent("Delete")
                .CloseElement()
                .CloseElement()
                .CloseElement()
                .CloseElement()
                .CloseElement();
        }
    }
}
