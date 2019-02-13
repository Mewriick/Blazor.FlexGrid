using Blazor.FlexGrid.Components.Configuration.ValueFormatters;
using Blazor.FlexGrid.Components.Renderers;
using Blazor.FlexGrid.Components.Renderers.CreateItemForm;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.RenderTree;
using System.Threading.Tasks;

namespace Blazor.FlexGrid.Components
{
    public class AddItemForm<TItem> : ComponentBase where TItem : class
    {
        private ICreateItemFormViewModel<TItem> createItemFormViewModel;

        [Inject]
        private CreateItemFormRenderer<TItem> CreatetemFormRenderer { get; set; }

        [Inject]
        private IPropertyValueAccessorCache PropertyValueAccessorCache { get; set; }


        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);

            CreatetemFormRenderer.BuildRendererTree(
                new CreateItemRendererContext<TItem>(createItemFormViewModel, PropertyValueAccessorCache),
                new BlazorRendererTreeBuilder(builder));
        }

        protected override Task OnInitAsync()
        {
            createItemFormViewModel = new CreateItemFormViewModel<TItem>();

            return Task.FromResult(0);
        }
    }
}
