using Blazor.FlexGrid.Components.Renderers.CreateItemForm.Layouts;
using Blazor.FlexGrid.Components.Renderers.EditInputs;
using System;

namespace Blazor.FlexGrid.Components.Renderers.CreateItemForm
{
    public class CreateItemFormRenderer<TItem> where TItem : class
    {
        private readonly EditInputRendererTree editInputRendererTree;

        public CreateItemFormRenderer(EditInputRendererTree editInputRendererTree)
        {
            this.editInputRendererTree = editInputRendererTree ?? throw new ArgumentNullException(nameof(editInputRendererTree));
        }

        public void BuildRendererTree(
            ICreateFormLayout<TItem> createFormLayout,
            CreateItemRendererContext<TItem> createItemRendererContext,
            IRendererTreeBuilder rendererTreeBuilder)
        {
            createItemRendererContext.ViewModel.ValidateModel();

            var bodyAction = createFormLayout.BuildBodyRendererTree(createItemRendererContext, editInputRendererTree);
            var footerAction = createFormLayout.BuildFooterRendererTree(createItemRendererContext);

            rendererTreeBuilder.OpenElement(HtmlTagNames.Form);

            bodyAction?.Invoke(rendererTreeBuilder);
            footerAction?.Invoke(rendererTreeBuilder);

            rendererTreeBuilder.CloseElement();
        }
    }
}
