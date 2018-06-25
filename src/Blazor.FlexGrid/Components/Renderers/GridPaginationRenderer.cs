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
                 $"{rendererContext.TableDataSet.PageInfoText()} of {rendererContext.TableDataSet.PageableOptions.TotalItemsCount}");
            rendererContext.RenderTreeBuilder.CloseElement();

            rendererContext.RenderTreeBuilder.OpenElement(++rendererContext.Sequence, HtmlTagNames.Div);
            rendererContext.RenderTreeBuilder.AddAttribute(++rendererContext.Sequence, HtmlAttributes.Class, "pagination-buttons-wrapper");

            RenderButton(rendererContext, PaginationButtonType.First, previousButtonIsDisabled, "fas fa-angle-double-left");
            RenderButton(rendererContext, PaginationButtonType.Previous, previousButtonIsDisabled, "fas fa-angle-left");
            RenderButton(rendererContext, PaginationButtonType.Next, nextButtonIsDisabled, "fas fa-angle-right");
            RenderButton(rendererContext, PaginationButtonType.Last, nextButtonIsDisabled, "fas fa-angle-double-right");

            rendererContext.RenderTreeBuilder.CloseElement();
            rendererContext.RenderTreeBuilder.CloseElement();
            rendererContext.RenderTreeBuilder.CloseElement();
        }

        private void RenderButton(GridRendererContext rendererContext, PaginationButtonType buttonType, bool isDisabled, string buttonArrowClass)
        {
            rendererContext.RenderTreeBuilder.OpenElement(++rendererContext.Sequence, HtmlTagNames.Button);
            rendererContext.RenderTreeBuilder.AddAttribute(++rendererContext.Sequence, HtmlAttributes.Class, !isDisabled ? "pagination-button" : "pagination-button pagination-button-disabled");
            rendererContext.RenderTreeBuilder.AddAttribute(++rendererContext.Sequence, HtmlAttributes.Disabled, isDisabled);

            // This is not good but when click function is passed as parameter every button
            // Have seme on click events
            rendererContext.RenderTreeBuilder.AddAttribute(++rendererContext.Sequence, HtmlJSEvents.OnClick,
                BindMethods.GetEventHandlerValue((UIMouseEventArgs async) =>
                 buttonType == PaginationButtonType.Previous
                    ? rendererContext.TableDataSet.GoToPreviousPage()
                    : buttonType == PaginationButtonType.Next
                        ? rendererContext.TableDataSet.GoToNextPage()
                        : buttonType == PaginationButtonType.First
                            ? rendererContext.TableDataSet.GoToFirstPage()
                            : rendererContext.TableDataSet.GoToLastPage()));

            rendererContext.RenderTreeBuilder.OpenElement(++rendererContext.Sequence, HtmlTagNames.Span);
            rendererContext.RenderTreeBuilder.AddAttribute(++rendererContext.Sequence, HtmlAttributes.Class, "pagination-button-arrow");
            rendererContext.RenderTreeBuilder.OpenElement(++rendererContext.Sequence, HtmlTagNames.I);
            rendererContext.RenderTreeBuilder.AddAttribute(++rendererContext.Sequence, HtmlAttributes.Class, buttonArrowClass);
            rendererContext.RenderTreeBuilder.CloseElement();
            rendererContext.RenderTreeBuilder.CloseElement();
            rendererContext.RenderTreeBuilder.CloseElement();
        }
    }

    internal enum PaginationButtonType
    {
        Next = 0,
        Previous = 1,
        First = 2,
        Last = 3
    }
}
