using Blazor.FlexGrid.DataSet;
using Microsoft.AspNetCore.Blazor;
using Microsoft.AspNetCore.Blazor.Components;
using System.Threading.Tasks;

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

            rendererContext.OpenElement(HtmlTagNames.Div, "pagination-wrapper");
            rendererContext.OpenElement(HtmlTagNames.Div, "pagination-wrapper-inner");
            rendererContext.OpenElement(HtmlTagNames.Div, "pagination-right");
            rendererContext.CloseElement();
            rendererContext.OpenElement(HtmlTagNames.Span, "pagination-page-status");
            rendererContext.AddContent($"{rendererContext.TableDataSet.PageInfoText()} of {rendererContext.TableDataSet.PageableOptions.TotalItemsCount}");
            rendererContext.CloseElement();
            rendererContext.OpenElement(HtmlTagNames.Div, "pagination-buttons-wrapper");

            RenderButton(rendererContext, PaginationButtonType.First, previousButtonIsDisabled, "fas fa-angle-double-left");
            RenderButton(rendererContext, PaginationButtonType.Previous, previousButtonIsDisabled, "fas fa-angle-left");
            RenderButton(rendererContext, PaginationButtonType.Next, nextButtonIsDisabled, "fas fa-angle-right");
            RenderButton(rendererContext, PaginationButtonType.Last, nextButtonIsDisabled, "fas fa-angle-double-right");

            rendererContext.CloseElement();
            rendererContext.CloseElement();
            rendererContext.CloseElement();
        }

        private void RenderButton(GridRendererContext rendererContext, PaginationButtonType buttonType, bool disabled, string buttonArrowClass)
        {
            rendererContext.OpenElement(HtmlTagNames.Button, !disabled ? "pagination-button" : "pagination-button pagination-button-disabled");
            rendererContext.AddDisabled(disabled);
            rendererContext.AddOnClickEvent(() =>
                BindMethods.GetEventHandlerValue(async (UIMouseEventArgs e) =>
                    await GetPaginationTask(rendererContext, buttonType))
            );

            rendererContext.OpenElement(HtmlTagNames.Span, "pagination-button-arrow");
            rendererContext.OpenElement(HtmlTagNames.I, buttonArrowClass);
            rendererContext.CloseElement();
            rendererContext.CloseElement();
            rendererContext.CloseElement();
        }

        private Task GetPaginationTask(GridRendererContext rendererContext, PaginationButtonType paginationButtonType)
            =>
            paginationButtonType == PaginationButtonType.Previous
               ? rendererContext.TableDataSet.GoToPreviousPage()
               : paginationButtonType == PaginationButtonType.Next
                   ? rendererContext.TableDataSet.GoToNextPage()
                   : paginationButtonType == PaginationButtonType.First
                       ? rendererContext.TableDataSet.GoToFirstPage()
                       : rendererContext.TableDataSet.GoToLastPage();

    }

    internal enum PaginationButtonType
    {
        Next = 0,
        Previous = 1,
        First = 2,
        Last = 3
    }
}
