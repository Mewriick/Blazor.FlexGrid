using Blazor.FlexGrid.Components.Configuration.ValueFormatters;
using Blazor.FlexGrid.Components.Renderers;
using Blazor.FlexGrid.Components.Renderers.CreateItemForm;
using Blazor.FlexGrid.Components.Renderers.CreateItemForm.Layouts;
using Blazor.FlexGrid.Validation;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.RenderTree;
using System.Threading.Tasks;

namespace Blazor.FlexGrid.Components
{
    public class CreateItemForm<TItem> : ComponentBase where TItem : class
    {
        private ICreateItemFormViewModel<TItem> createItemFormViewModel;

        [Inject]
        private CreateItemFormRenderer<TItem> CreatetemFormRenderer { get; set; }

        [Inject]
        private ITypePropertyAccessorCache PropertyValueAccessorCache { get; set; }

        [Inject]
        private IModelValidator ModelValidator { get; set; }


        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);

            CreatetemFormRenderer.BuildRendererTree(
                new SignleColumnLayout<TItem>(),
                new CreateItemRendererContext<TItem>(createItemFormViewModel, PropertyValueAccessorCache),
                new BlazorRendererTreeBuilder(builder));
        }

        protected override Task OnInitAsync()
        {
            createItemFormViewModel = new CreateItemFormViewModel<TItem>(ModelValidator);

            return Task.CompletedTask;
        }
    }
}
