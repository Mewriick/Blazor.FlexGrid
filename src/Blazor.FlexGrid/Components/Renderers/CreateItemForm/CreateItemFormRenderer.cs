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
            CreateItemRendererContext<TItem> createItemRendererContext,
            IRendererTreeBuilder rendererTreeBuilder)
        {
            rendererTreeBuilder.OpenElement(HtmlTagNames.Div);

            foreach (var property in typeof(TItem).GetProperties())
            {
                createItemRendererContext.ActualColumnName = property.Name;

                editInputRendererTree.BuildInputRendererTree(
                    rendererTreeBuilder,
                    createItemRendererContext,
                    createItemRendererContext.SetActulItemColumnValue);
            }

            rendererTreeBuilder.CloseElement();
        }
    }
}
