using Blazor.FlexGrid.Components.Renderers;
using Blazor.FlexGrid.Filters;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.RenderTree;

namespace Blazor.FlexGrid.Components.Filters
{
    public class ColumnFilter<TValue> : ComponentBase
    {
        private bool filterDefinitionOpened = false;

        [CascadingParameter] FilterContext FilterContext { get; set; }

        [Parameter] string ColumnName { get; set; }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);

            var rendererBuilder = new BlazorRendererTreeBuilder(builder);

            rendererBuilder
                .OpenElement(HtmlTagNames.Button, "action-button action-button-small")
                .AddAttribute(HtmlJSEvents.OnClick,
                    BindMethods.GetEventHandlerValue((UIMouseEventArgs e) =>
                   {
                       filterDefinitionOpened = !filterDefinitionOpened;
                   })
                 )
                .OpenElement(HtmlTagNames.Span)
                .OpenElement(HtmlTagNames.I, "fas fa-filter")
                .CloseElement()
                .CloseElement()
                .CloseElement()
                .OpenElement(HtmlTagNames.Div, filterDefinitionOpened ? "filter-wrapper-open" : "filter-wrapper")
                .OpenElement(HtmlTagNames.Input)
                .AddAttribute(HtmlAttributes.Value, BindMethods.GetValue(string.Empty))
                .AddAttribute(HtmlJSEvents.OnChange, BindMethods.SetValueHandler(delegate (string __value)
                {
                    FilterContext.AddOrUpdateFilterDefinition(new ExpressionFilterDefinition(ColumnName, FilterOperation.Contains, __value));
                }, string.Empty))
                .CloseElement()
                .CloseElement();
        }
    }
}
