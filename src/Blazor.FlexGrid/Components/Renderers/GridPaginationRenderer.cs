using Blazor.FlexGrid.DataSet;
using Microsoft.AspNetCore.Blazor;
using Microsoft.AspNetCore.Blazor.Components;

namespace Blazor.FlexGrid.Components.Renderers
{
    public class GridPaginationRenderer : GridPartRenderer
    {
        public override void Render(GridRendererContext rendererContext)
        {
            if (!rendererContext.TableDataSet.HasItems())
            {
                return;
            }

            var nextButtonIsDisabled = rendererContext.TableDataSet.PageableOptions.IsLastPage;
            var previousButtonIsDisabled = rendererContext.TableDataSet.PageableOptions.IsFirstPage;

            rendererContext.RenderTreeBuilder.OpenElement(++rendererContext.Sequence, HtmlTagNames.Div);
            rendererContext.RenderTreeBuilder.AddAttribute(++rendererContext.Sequence, HtmlAttributes.Class, "pagination-wrapper");

            rendererContext.RenderTreeBuilder.OpenElement(++rendererContext.Sequence, HtmlTagNames.Div);
            rendererContext.RenderTreeBuilder.AddAttribute(++rendererContext.Sequence, HtmlAttributes.Class, "pagination-wrapper-inner");

            rendererContext.RenderTreeBuilder.OpenElement(++rendererContext.Sequence, HtmlTagNames.Div);
            rendererContext.RenderTreeBuilder.AddAttribute(++rendererContext.Sequence, HtmlAttributes.Class, "pagination-right");
            rendererContext.RenderTreeBuilder.CloseElement();

            rendererContext.RenderTreeBuilder.OpenElement(++rendererContext.Sequence, HtmlTagNames.Span);
            rendererContext.RenderTreeBuilder.AddAttribute(++rendererContext.Sequence, HtmlAttributes.Class, "pagination-page-status");
            rendererContext.RenderTreeBuilder.AddContent(++rendererContext.Sequence,
                 $"{rendererContext.TableDataSet.PageableOptions.CurrentPage + 1} / {rendererContext.TableDataSet.PageableOptions.PagesCount}");
            rendererContext.RenderTreeBuilder.CloseElement();

            rendererContext.RenderTreeBuilder.OpenElement(++rendererContext.Sequence, HtmlTagNames.Div);
            rendererContext.RenderTreeBuilder.AddAttribute(++rendererContext.Sequence, HtmlAttributes.Class, "pagination-buttons-wrapper");

            rendererContext.RenderTreeBuilder.OpenElement(++rendererContext.Sequence, HtmlTagNames.Button);
            rendererContext.RenderTreeBuilder.AddAttribute(++rendererContext.Sequence, HtmlAttributes.Class, !previousButtonIsDisabled ? "pagination-button" : "pagination-button pagination-button-disabled");
            rendererContext.RenderTreeBuilder.AddAttribute(++rendererContext.Sequence, HtmlAttributes.Disabled, previousButtonIsDisabled);
            rendererContext.RenderTreeBuilder.AddAttribute(++rendererContext.Sequence, HtmlJSEvents.OnClick,
                BindMethods.GetEventHandlerValue((UIMouseEventArgs async) => rendererContext.TableDataSet.GoToPreviousPage()));

            rendererContext.RenderTreeBuilder.OpenElement(++rendererContext.Sequence, HtmlTagNames.Span);
            rendererContext.RenderTreeBuilder.AddAttribute(++rendererContext.Sequence, HtmlAttributes.Class, "pagination-button-arrow");
            rendererContext.RenderTreeBuilder.OpenElement(++rendererContext.Sequence, HtmlTagNames.I);
            rendererContext.RenderTreeBuilder.AddAttribute(++rendererContext.Sequence, HtmlAttributes.Class, "fas fa-angle-left");
            rendererContext.RenderTreeBuilder.CloseElement();
            rendererContext.RenderTreeBuilder.CloseElement();

            if (!previousButtonIsDisabled)
            {
                rendererContext.RenderTreeBuilder.OpenElement(++rendererContext.Sequence, HtmlTagNames.Span);
                rendererContext.RenderTreeBuilder.AddAttribute(++rendererContext.Sequence, HtmlAttributes.Class, "pagination-button-arrow-space");
                rendererContext.RenderTreeBuilder.CloseElement();
            }

            rendererContext.RenderTreeBuilder.CloseElement();

            rendererContext.RenderTreeBuilder.OpenElement(++rendererContext.Sequence, HtmlTagNames.Button);
            rendererContext.RenderTreeBuilder.AddAttribute(++rendererContext.Sequence, HtmlAttributes.Class, !nextButtonIsDisabled ? "pagination-button" : "pagination-button pagination-button-disabled");
            rendererContext.RenderTreeBuilder.AddAttribute(++rendererContext.Sequence, HtmlAttributes.Disabled, nextButtonIsDisabled);
            rendererContext.RenderTreeBuilder.AddAttribute(++rendererContext.Sequence, HtmlJSEvents.OnClick,
                BindMethods.GetEventHandlerValue((UIMouseEventArgs async) => rendererContext.TableDataSet.GoToNextPage()));

            rendererContext.RenderTreeBuilder.OpenElement(++rendererContext.Sequence, HtmlTagNames.Span);
            rendererContext.RenderTreeBuilder.AddAttribute(++rendererContext.Sequence, HtmlAttributes.Class, "pagination-button-arrow");
            rendererContext.RenderTreeBuilder.OpenElement(++rendererContext.Sequence, HtmlTagNames.I);
            rendererContext.RenderTreeBuilder.AddAttribute(++rendererContext.Sequence, HtmlAttributes.Class, "fas fa-angle-right");
            rendererContext.RenderTreeBuilder.CloseElement();
            rendererContext.RenderTreeBuilder.CloseElement();

            if (!nextButtonIsDisabled)
            {
                rendererContext.RenderTreeBuilder.OpenElement(++rendererContext.Sequence, HtmlTagNames.Span);
                rendererContext.RenderTreeBuilder.AddAttribute(++rendererContext.Sequence, HtmlAttributes.Class, "pagination-button-arrow-space");
                rendererContext.RenderTreeBuilder.CloseElement();
            }

            rendererContext.RenderTreeBuilder.CloseElement();

            rendererContext.RenderTreeBuilder.CloseElement();
            rendererContext.RenderTreeBuilder.CloseElement();
            rendererContext.RenderTreeBuilder.CloseElement();
        }
    }
}
