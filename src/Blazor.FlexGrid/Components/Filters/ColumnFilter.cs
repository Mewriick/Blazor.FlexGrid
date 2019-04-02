using Blazor.FlexGrid.Filters;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.RenderTree;

namespace Blazor.FlexGrid.Components.Filters
{
    public class ColumnFilter<TValue> : ComponentBase
    {
        [CascadingParameter] FilterContext FilterContext { get; set; }

        [Parameter] string ColumnName { get; set; }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);

            builder.OpenElement(0, "input");
            builder.AddAttribute(1, "value", BindMethods.GetValue(string.Empty));
            builder.AddAttribute(2, "onchange", BindMethods.SetValueHandler(delegate (string __value)
            {
                FilterContext.AddOrUpdateFilterDefinition(new ExpressionFilterDefinition(ColumnName, FilterOperation.Contains, __value));
            }, string.Empty));

            builder.CloseElement();
        }
    }
}
