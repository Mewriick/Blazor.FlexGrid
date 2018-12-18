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
        }

        private void RenderEditButton(GridRendererContext rendererContext)
        {
            var localActualItem = rendererContext.ActualItem;

            rendererContext.OpenElement(HtmlTagNames.I, "fas fa-edit");
            rendererContext.AddOnClickEvent(() =>
                BindMethods.GetEventHandlerValue((UIMouseEventArgs e) =>
                    rendererContext.TableDataSet.StartEditItem(localActualItem))
            );

            rendererContext.CloseElement();
        }

        private void RenderSaveButton(GridRendererContext rendererContext)
        {
            rendererContext.OpenElement(HtmlTagNames.I, "fas fa-save");
            rendererContext.AddOnClickEvent(() =>
                BindMethods.GetEventHandlerValue((UIMouseEventArgs e) =>
                    rendererContext.TableDataSet.SaveItem(rendererContext.PropertyValueAccessor))
            );

            rendererContext.CloseElement();
        }

        private void RenderDiscardButton(GridRendererContext rendererContext)
        {
            rendererContext.OpenElement(HtmlTagNames.I, "fas fa-times");
            rendererContext.AddOnClickEvent(() =>
                BindMethods.GetEventHandlerValue((UIMouseEventArgs e) =>
                    rendererContext.TableDataSet.CancelEditation())
            );

            rendererContext.CloseElement();
        }
    }
}
