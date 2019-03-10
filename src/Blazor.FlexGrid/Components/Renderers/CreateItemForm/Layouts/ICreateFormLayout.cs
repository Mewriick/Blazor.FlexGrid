using Blazor.FlexGrid.Components.Renderers.FormInputs;
using System;

namespace Blazor.FlexGrid.Components.Renderers.CreateItemForm.Layouts
{
    public interface ICreateFormLayout<TItem> where TItem : class
    {
        Action<IRendererTreeBuilder> BuildBodyRendererTree(CreateItemRendererContext<TItem> createItemRendererContext, IFormInputRendererTreeProvider formInputRendererTreeProvider);

        Action<IRendererTreeBuilder> BuildFooterRendererTree(CreateItemRendererContext<TItem> createItemRendererContext);
    }
}
