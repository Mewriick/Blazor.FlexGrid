using Blazor.FlexGrid.Components.Renderers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.RenderTree;

namespace Blazor.FlexGrid.Components
{
    internal class GridViewTable : ComponentBase
    {
        [Parameter] public RenderFragment<ImutableGridRendererContext> ChildContent { get; set; }

        [Parameter] public ImutableGridRendererContext ImutableGridRendererContext { get; set; }

        [CascadingParameter] FlexGridContext CascadeFlexGridContext { get; set; }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);

            builder.OpenComponent<CascadingValue<ImutableGridRendererContext>>(0);
            builder.AddAttribute(1, "Value", ImutableGridRendererContext);
            builder.AddAttribute(2, BlazorRendererTreeBuilder.ChildContent, ChildContent?.Invoke(ImutableGridRendererContext));
            builder.CloseComponent();
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            CascadeFlexGridContext.SetRequestRendererNotification(StateHasChanged);
        }
    }
}
