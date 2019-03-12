using Blazor.FlexGrid.Components.Renderers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.RenderTree;

namespace Blazor.FlexGrid.Components
{
    internal class GridViewTable : ComponentBase
    {
        [Parameter] RenderFragment<ImutableGridRendererContext> ChildContent { get; set; }

        [Parameter] ImutableGridRendererContext ImutableGridRendererContext { get; set; }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);

            ImutableGridRendererContext.SetRequestRendererNotification(StateHasChanged);

            builder.OpenComponent<CascadingValue<ImutableGridRendererContext>>(0);
            builder.AddAttribute(1, "Value", ImutableGridRendererContext);
            builder.AddAttribute(2, RenderTreeBuilder.ChildContent, ChildContent?.Invoke(ImutableGridRendererContext));
            builder.CloseComponent();
        }
    }
}
