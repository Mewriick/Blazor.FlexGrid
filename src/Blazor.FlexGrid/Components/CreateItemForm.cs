using Blazor.FlexGrid.Components.Configuration;
using Blazor.FlexGrid.Components.Configuration.ValueFormatters;
using Blazor.FlexGrid.Components.Renderers;
using Blazor.FlexGrid.Components.Renderers.CreateItemForm;
using Blazor.FlexGrid.Components.Renderers.CreateItemForm.Layouts;
using Blazor.FlexGrid.DataSet.Http;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.RenderTree;
using System.Threading;

namespace Blazor.FlexGrid.Components
{
    public class CreateItemForm<TModel, TOutputDto> : ComponentBase
        where TModel : class
        where TOutputDto : class
    {
        private ICreateItemFormViewModel<TModel> createItemFormViewModel;

        [Inject]
        private CreateItemFormRenderer<TModel> CreatetemFormRenderer { get; set; }

        [Inject]
        private ITypePropertyAccessorCache PropertyValueAccessorCache { get; set; }

        [Inject]
        private IFormLayoutProvider<TModel> LayoutProvider { get; set; }

        [Inject]
        private ICreateItemHandle<TModel, TOutputDto> CreateItemHandle { get; set; }

        [Inject]
        private FlexGridInterop FlexGridInterop { get; set; }

        [Parameter] CreateItemContext CreateItemContext { get; set; }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);

            CreatetemFormRenderer.BuildRendererTree(
                LayoutProvider.GetLayoutBuilder() ?? new SignleColumnLayout<TModel>(),
                new CreateItemRendererContext<TModel>(createItemFormViewModel, PropertyValueAccessorCache, CreateItemContext.CreateFormCssClasses),
                new BlazorRendererTreeBuilder(builder));
        }

        protected override void OnParametersSet()
        {
            if (createItemFormViewModel is null)
            {
                createItemFormViewModel = new CreateItemFormViewModel<TModel>(CreateItemContext.CreateItemOptions);
                createItemFormViewModel.SaveAction = async model =>
                {
                    if (string.IsNullOrEmpty(CreateItemContext.CreateItemOptions.CreateUri))
                    {
                        CreateItemContext.NotifyItemCreated(model);
                    }
                    else
                    {
                        var dto = await CreateItemHandle.CreateItem(model, CreateItemContext.CreateItemOptions, CancellationToken.None);
                        CreateItemContext.NotifyItemCreated(dto);
                    }
                    createItemFormViewModel.ClearModel();

                    if (CreateItemContext.CreateItemOptions.CloseAfterSuccessfullySaved)
                    {
                        await FlexGridInterop.HideModal(CreateItemOptions.CreateItemModalName);
                    }

                    StateHasChanged();
                };
            }
        }
    }
}
