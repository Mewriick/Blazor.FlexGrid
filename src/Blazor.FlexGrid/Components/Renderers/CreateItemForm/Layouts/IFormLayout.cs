using Blazor.FlexGrid.Components.Renderers.FormInputs;
using System;

namespace Blazor.FlexGrid.Components.Renderers.CreateItemForm.Layouts
{
    public interface IFormLayout<TModel> where TModel : class
    {
        Action<IRendererTreeBuilder> BuildBodyRendererTree(CreateItemRendererContext<TModel> createItemRendererContext, IFormInputRendererTreeProvider formInputRendererTreeProvider);

        Action<IRendererTreeBuilder> BuildFooterRendererTree(CreateItemRendererContext<TModel> createItemRendererContext);
    }
}
