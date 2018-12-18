using Microsoft.AspNetCore.Blazor;
using Microsoft.AspNetCore.Blazor.Components;

namespace Blazor.FlexGrid.Components.Renderers
{
    public class GridActionButtonsRenderer : GridPartRenderer
    {
        public override bool CanRender(GridRendererContext rendererContext)
            => rendererContext.IsLastColumn && rendererContext.GridConfiguration.InlineEditIsAllowed;

        protected override void RenderInternal(GridRendererContext rendererContext)
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
            }

            rendererContext.CloseElement();
            rendererContext.CloseElement();
            rendererContext.CloseElement();
        }

        private void RenderEditButton(GridRendererContext rendererContext)
        {
            var localActualItem = rendererContext.ActualItem;

            rendererContext.OpenElement(HtmlTagNames.Button, "action-button");
            rendererContext.AddOnClickEvent(() =>
                BindMethods.GetEventHandlerValue((UIMouseEventArgs e) =>
                    rendererContext.TableDataSet.StartEditItem(localActualItem))
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
            rendererContext.AddOnClickEvent(() =>
                BindMethods.GetEventHandlerValue((UIMouseEventArgs e) =>
                    rendererContext.TableDataSet.SaveItem(rendererContext.PropertyValueAccessor))
            );

            rendererContext.OpenElement(HtmlTagNames.Span, "action-button-span");
            rendererContext.OpenElement(HtmlTagNames.I, "fas fa-save");
            rendererContext.CloseElement();
            rendererContext.CloseElement();
            rendererContext.CloseElement();
        }

        private void RenderDiscardButton(GridRendererContext rendererContext)
        {
            rendererContext.OpenElement(HtmlTagNames.Button, "action-button");
            rendererContext.AddOnClickEvent(() =>
                BindMethods.GetEventHandlerValue((UIMouseEventArgs e) =>
                    rendererContext.TableDataSet.CancelEditation())
            );

            rendererContext.OpenElement(HtmlTagNames.Span, "action-button-span");
            rendererContext.OpenElement(HtmlTagNames.I, "fas fa-times");
            rendererContext.CloseElement();
            rendererContext.CloseElement();
            rendererContext.CloseElement();
        }
    }
}
