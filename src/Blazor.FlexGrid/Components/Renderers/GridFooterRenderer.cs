using Blazor.FlexGrid.DataSet;
using Blazor.FlexGrid.Permission;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace Blazor.FlexGrid.Components.Renderers
{
    public class GridFooterRenderer : GridPartRenderer
    {
        private readonly string groupingSelectId = "grouping-select";
        private readonly string noGroupingOptionText = "(no grouping)";
        private readonly string groupByPlaceholder = "Group by:";


        protected override void BuildRendererTreeInternal(GridRendererContext rendererContext, PermissionContext permissionContext)
        {
            var nextButtonIsDisabled = rendererContext.TableDataSet.PageableOptions.IsLastPage;
            var previousButtonIsDisabled = rendererContext.TableDataSet.PageableOptions.IsFirstPage;

            //rendererContext.OpenElement(HtmlTagNames.Div, "pagination-wrapper");
            rendererContext.OpenElement(HtmlTagNames.Div, rendererContext.CssClasses.FooterCssClasses.FooterWrapper);

            if (rendererContext.TableDataSet.GroupingOptions.IsGroupingEnabled
                && rendererContext.TableDataSet.GetType().GetGenericTypeDefinition() == typeof(TableDataSet<>))
                RenderGroupingFooterPart(rendererContext);

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
            //rendererContext.CloseElement();
        }

        public override bool CanRender(GridRendererContext rendererContext)
            => rendererContext.TableDataSet.HasItems();

        private void RenderGroupingFooterPart(GridRendererContext rendererContext)
        {
            rendererContext.OpenElement(HtmlTagNames.Div, rendererContext.CssClasses.FooterCssClasses.GroupingPartWrapper);
            rendererContext.OpenElement(HtmlTagNames.Select);
            rendererContext.AddAttribute(HtmlAttributes.Id, groupingSelectId);

            rendererContext.AddOnChangeEvent(() =>
                BindMethods.GetEventHandlerValue(async (UIChangeEventArgs e) =>
                {
                    string propertyName = e.Value.ToString();
                    if (propertyName == noGroupingOptionText)
                        rendererContext.TableDataSet.GroupingOptions.DeactivateGrouping();
                    else
                        rendererContext.TableDataSet.GroupingOptions.SetGroupedProperty(propertyName);
                    await rendererContext.TableDataSet.GoToPage(0);
                    rendererContext.RequestRerenderNotification?.Invoke();
                })
            );


            if (!rendererContext.TableDataSet.GroupingOptions.IsGroupingActive)
            {
                rendererContext.OpenElement(HtmlTagNames.Option);
                rendererContext.AddAttribute(HtmlAttributes.Disabled, true);
                rendererContext.AddContent(groupByPlaceholder);
                rendererContext.CloseElement();
            }
            else
            {
                rendererContext.OpenElement(HtmlTagNames.Option);
                rendererContext.AddAttribute(HtmlAttributes.Value, string.Empty);
                rendererContext.AddContent(noGroupingOptionText);
                rendererContext.CloseElement();
            }

            foreach (var groupableProperty in rendererContext.TableDataSet.GroupingOptions.GroupableProperties)
            {
                rendererContext.OpenElement(HtmlTagNames.Option);

                if (groupableProperty == rendererContext.TableDataSet.GroupingOptions.GroupedProperty)
                    rendererContext.AddAttribute(HtmlAttributes.Selected, true);

                rendererContext.AddAttribute(HtmlAttributes.Value, groupableProperty.Name);
                rendererContext.ActualColumnName = groupableProperty.Name;
                var displayedGroupablePropertyName = rendererContext.ActualColumnConfiguration?.Caption ?? groupableProperty.Name;
                rendererContext.AddContent(displayedGroupablePropertyName);
                rendererContext.CloseElement();
            }
            rendererContext.CloseElement();
            rendererContext.CloseElement();
        }



        private void RenderButton(GridRendererContext rendererContext, PaginationButtonType buttonType, bool disabled, string buttonArrowClass)
        {
            rendererContext.OpenElement(HtmlTagNames.Button, !disabled
                ? rendererContext.CssClasses.FooterCssClasses.PaginationButton
                : rendererContext.CssClasses.FooterCssClasses.PaginationButtonDisabled);
            rendererContext.AddDisabled(disabled);
            rendererContext.AddOnClickEvent(() =>
                BindMethods.GetEventHandlerValue(async (UIMouseEventArgs e) =>
                {
                    await GetPaginationTask(rendererContext, buttonType);
                    rendererContext.RequestRerenderNotification?.Invoke();
                })
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
