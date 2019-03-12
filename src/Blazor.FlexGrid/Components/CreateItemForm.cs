using Blazor.FlexGrid.Components.Configuration.ValueFormatters;
using Blazor.FlexGrid.Components.Renderers;
using Blazor.FlexGrid.Components.Renderers.CreateItemForm;
using Blazor.FlexGrid.Components.Renderers.CreateItemForm.Layouts;
using Blazor.FlexGrid.DataSet.Http;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.RenderTree;
using System;
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
        private ICreateItemHandle<TModel, TOutputDto> CreateItemHandle { get; set; }

        [CascadingParameter] CreateItemContext CascadeCreateItemContext { get; set; }

        protected CreateItemContext CreateItemContext { get; set; }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);

            Console.WriteLine($"Render create form");

            CreatetemFormRenderer.BuildRendererTree(
                new SignleColumnLayout<TModel>(),
                new CreateItemRendererContext<TModel>(createItemFormViewModel, PropertyValueAccessorCache),
                new BlazorRendererTreeBuilder(builder));
        }

        protected override void OnParametersSet()
        {
            Console.WriteLine("OnParametersSet");

            if (CreateItemContext is null)
            {
                if (CascadeCreateItemContext is null)
                {
                    Console.WriteLine("NULL");

                    //throw new InvalidOperationException($"CreateItemForm requires a {nameof(CascadeCreateItemContext)} parameter");
                    return;
                }

                if (createItemFormViewModel is null)
                {
                    createItemFormViewModel = new CreateItemFormViewModel<TModel>(CascadeCreateItemContext.CreateItemOptions);
                    CreateItemContext = CascadeCreateItemContext;
                    createItemFormViewModel.SaveAction = async model =>
                    {
                        Console.WriteLine($"Invoking save item. Model {model.ToString()}");

                        var dto = await CreateItemHandle.CreateItem(model, CascadeCreateItemContext.CreateItemOptions, CancellationToken.None);
                        CascadeCreateItemContext.NotifyItemCreated(dto);
                    };
                }
            }
            else if (CascadeCreateItemContext != CreateItemContext)
            {
                throw new InvalidOperationException($"{GetType()} does not support changing the {nameof(CreateItemContext)} dynamically.");
            }
        }
    }
}
