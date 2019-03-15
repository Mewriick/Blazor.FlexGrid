using Blazor.FlexGrid.Components.Renderers.CreateItemForm.Layouts;
using Blazor.FlexGrid.Components.Renderers.FormInputs;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.RenderTree;
using System;

namespace Blazor.FlexGrid.Components.Renderers.CreateItemForm
{
    public class CreateItemFormRenderer<TModel> where TModel : class
    {
        private readonly IFormInputRendererTreeProvider formInputRendererTreeProvider;
        private readonly EventCallbackFactory eventCallbackFactory;


        public CreateItemFormRenderer(IFormInputRendererTreeProvider formInputRendererTreeProvider)
        {
            this.formInputRendererTreeProvider = formInputRendererTreeProvider ?? throw new ArgumentNullException(nameof(formInputRendererTreeProvider));
            this.eventCallbackFactory = new EventCallbackFactory();
        }

        public void BuildRendererTree(
            IFormLayout<TModel> createFormLayout,
            CreateItemRendererContext<TModel> createItemRendererContext,
            IRendererTreeBuilder rendererTreeBuilder)
        {
            var bodyAction = createFormLayout.BuildBodyRendererTree(createItemRendererContext, formInputRendererTreeProvider);
            var footerAction = createFormLayout.BuildFooterRendererTree(createItemRendererContext);

            RenderFragment<EditContext> formBody = (EditContext context) => delegate (RenderTreeBuilder builder)
            {
                var internalBuilder = new BlazorRendererTreeBuilder(builder);
                bodyAction?.Invoke(internalBuilder);
                footerAction?.Invoke(internalBuilder);

                internalBuilder
                .OpenComponent(typeof(DataAnnotationsValidator))
                .CloseComponent();
            };

            rendererTreeBuilder
                .OpenComponent(typeof(EditForm))
                .AddAttribute(nameof(EditContext), createItemRendererContext.ViewModel.EditContext)
                .AddAttribute("OnValidSubmit", eventCallbackFactory.Create<EditContext>(this,
                    context => createItemRendererContext.ViewModel.SaveAction.Invoke(createItemRendererContext.ViewModel.Model)))
                .AddAttribute(RenderTreeBuilder.ChildContent, formBody)
                .CloseComponent();

            createItemRendererContext.ViewModel.ValidateModel();
        }
    }
}
