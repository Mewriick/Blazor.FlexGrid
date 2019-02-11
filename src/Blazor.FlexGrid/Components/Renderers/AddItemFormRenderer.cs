using Blazor.FlexGrid.Components.Renderers.EditInputs;
using System;

namespace Blazor.FlexGrid.Components.Renderers
{
    public class AddItemFormRenderer
    {
        private readonly IRendererTreeBuilder rendererTreeBuilder;
        private readonly EditInputRendererTree editInputRendererTree;

        public AddItemFormRenderer(
            IRendererTreeBuilder rendererTreeBuilder,
            EditInputRendererTree editInputRendererTree)
        {
            this.rendererTreeBuilder = rendererTreeBuilder ?? throw new ArgumentNullException(nameof(rendererTreeBuilder));
            this.editInputRendererTree = editInputRendererTree ?? throw new ArgumentNullException(nameof(editInputRendererTree));
        }

        public void BuildeRendererTree()
        {

        }
    }
}
