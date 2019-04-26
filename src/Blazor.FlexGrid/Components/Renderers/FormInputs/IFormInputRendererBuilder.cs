using System;

namespace Blazor.FlexGrid.Components.Renderers.FormInputs
{
    public interface IFormInputRendererBuilder
    {
        bool IsSupportedDateType(Type type);

        Action<IRendererTreeBuilder> BuildRendererTree<TItem>(IActualItemContext<TItem> actualItemContext, FormField field) where TItem : class;
    }
}
