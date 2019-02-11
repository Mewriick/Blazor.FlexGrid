using Blazor.FlexGrid.Components.Configuration.ValueFormatters;
using Blazor.FlexGrid.Components.Renderers;
using Blazor.FlexGrid.Components.Renderers.CreateItemForm;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.RenderTree;

namespace Blazor.FlexGrid.Components
{
    public class AddItemForm<TItem> : ComponentBase where TItem : class
    {
        [Inject]
        private CreateItemFormRenderer<TItem> CreatetemFormRenderer { get; set; }

        [Inject]
        private IPropertyValueAccessorCache PropertyValueAccessorCache { get; set; }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);

            CreatetemFormRenderer.BuildRendererTree(
                new CreateItemRendererContext<TItem>(PropertyValueAccessorCache),
                new BlazorRendererTreeBuilder(builder));
        }
    }
}
