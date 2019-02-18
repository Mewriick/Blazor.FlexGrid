using Blazor.FlexGrid.Components.Renderers.EditInputs;
using System;

namespace Blazor.FlexGrid.Components.Renderers.CreateItemForm.Layouts
{
    public interface ICreateFormLayout<TItem> where TItem : class
    {
        Action<IRendererTreeBuilder> BuildBodyRendererTree(CreateItemRendererContext<TItem> createItemRendererContext, AbstractEditInputRenderer editInputRenderer);

        Action<IRendererTreeBuilder> BuildFooterRendererTree(CreateItemRendererContext<TItem> createItemRendererContext);
    }
}
