using Blazor.FlexGrid.Permission;
using Microsoft.AspNetCore.Components;

namespace Blazor.FlexGrid.Components.Renderers
{
    public class GridActionButtonsRenderer : GridPartRenderer
    {
        public override bool CanRender(GridRendererContext rendererContext)
            => rendererContext.IsLastColumn && rendererContext.GridConfiguration.InlineEditOptions.InlineEditIsAllowed;

        protected override void BuildRendererTreeInternal(GridRendererContext rendererContext, PermissionContext permissionContext)
        {
            rendererContext.OpenElement(HtmlTagNames.TableColumn, rendererContext.CssClasses.TableCell);
            rendererContext.OpenElement(HtmlTagNames.Div, "action-buttons-wrapper");
            rendererContext.OpenElement(HtmlTagNames.Div, "action-buttons-wrapper-inner");

            if (rendererContext.IsActualItemEdited)
            {
                RenderSaveButton(rendererContext);
                RenderDiscardButton(rendererContext);
            }
            else
            {
                RenderEditButton(rendererContext);
                RenderDeleteButton(rendererContext, permissionContext);
            }

            rendererContext.CloseElement();
            rendererContext.CloseElement();
            rendererContext.CloseElement();
        }

        private void RenderEditButton(GridRendererContext rendererContext)
        {
            var localActualItem = rendererContext.ActualItem;

            rendererContext.OpenElement(HtmlTagNames.Button, "action-button");
            rendererContext.AddOnClickEvent(
                EventCallback.Factory.Create(this, (UIMouseEventArgs e) =>
                {
                    rendererContext.TableDataSet.StartEditItem(localActualItem);
                    rendererContext.RequestRerenderNotification?.Invoke();
                })
            );

            rendererContext.OpenElement(HtmlTagNames.Span, "action-button-span");
            rendererContext.OpenElement(HtmlTagNames.I, "fas fa-edit");
            rendererContext.CloseElement();
            rendererContext.CloseElement();
            rendererContext.CloseElement();
        }

        private void RenderSaveButton(GridRendererContext rendererContext)
        {
            rendererContext.OpenElement(HtmlTagNames.Button, "action-button");
            rendererContext.AddOnClickEvent(
                EventCallback.Factory.Create(this, (UIMouseEventArgs e) =>
                {
                    rendererContext.TableDataSet.SaveItem(rendererContext.PropertyValueAccessor);
                    rendererContext.RequestRerenderNotification?.Invoke();
                })
            );

            rendererContext.OpenElement(HtmlTagNames.Span, "action-button-span");
            rendererContext.OpenElement(HtmlTagNames.I, "fas fa-save");
            rendererContext.CloseElement();
            rendererContext.CloseElement();
            rendererContext.CloseElement();
        }

        private void RenderDeleteButton(GridRendererContext rendererContext, PermissionContext permissionContext)
        {
            if (!permissionContext.HasDeleteItemPermission ||
                !rendererContext.GridConfiguration.InlineEditOptions.AllowDeleting)
            {
                return;
            }

            var localActualItem = rendererContext.ActualItem;

            rendererContext.OpenElement(HtmlTagNames.Button, "action-button");
            rendererContext.AddOnClickEvent(
                EventCallback.Factory.Create(this, (UIMouseEventArgs e) =>
                {
                    rendererContext.TableDataSet.DeleteItem(localActualItem);
                    rendererContext.RequestRerenderNotification?.Invoke();
                })
            );

            rendererContext.OpenElement(HtmlTagNames.Span, "action-button-span");
            rendererContext.OpenElement(HtmlTagNames.I, "fas fa-trash-alt");
            rendererContext.CloseElement();
            rendererContext.CloseElement();
            rendererContext.CloseElement();
        }

        private void RenderDiscardButton(GridRendererContext rendererContext)
        {
            rendererContext.OpenElement(HtmlTagNames.Button, "action-button");
            rendererContext.AddOnClickEvent(
                EventCallback.Factory.Create(this, (UIMouseEventArgs e) =>
                {
                    rendererContext.TableDataSet.CancelEditation();
                    rendererContext.RequestRerenderNotification?.Invoke();
                })
            );

            rendererContext.OpenElement(HtmlTagNames.Span, "action-button-span");
            rendererContext.OpenElement(HtmlTagNames.I, "fas fa-times");
            rendererContext.CloseElement();
            rendererContext.CloseElement();
            rendererContext.CloseElement();
        }
    }
}
