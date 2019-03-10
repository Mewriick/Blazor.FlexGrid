using Blazor.FlexGrid.Components.Renderers.CreateItemForm.Layouts;
using Blazor.FlexGrid.Components.Renderers.FormInputs;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.RenderTree;
using System;

namespace Blazor.FlexGrid.Components.Renderers.CreateItemForm
{
    public class CreateItemFormRenderer<TItem> where TItem : class
    {
        private readonly IFormInputRendererTreeProvider formInputRendererTreeProvider;

        public CreateItemFormRenderer(IFormInputRendererTreeProvider formInputRendererTreeProvider)
        {
            this.formInputRendererTreeProvider = formInputRendererTreeProvider ?? throw new ArgumentNullException(nameof(formInputRendererTreeProvider));
        }

        public void BuildRendererTree(
            ICreateFormLayout<TItem> createFormLayout,
            CreateItemRendererContext<TItem> createItemRendererContext,
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
                .AddAttribute(RenderTreeBuilder.ChildContent, formBody)
                .CloseComponent();

            createItemRendererContext.ViewModel.ValidateModel();
        }
    }
}
