using System;

namespace Blazor.FlexGrid.Components.Renderers.EditInputs
{
    public abstract class AbstractEditInputRenderer
    {
        protected AbstractEditInputRenderer successor;

        public void SetSuccessor(AbstractEditInputRenderer editInputRenderer)
            => successor = editInputRenderer ?? throw new ArgumentNullException(nameof(editInputRenderer));

        abstract public void BuildInputRendererTree(IRendererTreeBuilder rendererTreeBuilder, IActualItemContext actualItemContext, Action<string, object> onChangeAction);
    }
}
