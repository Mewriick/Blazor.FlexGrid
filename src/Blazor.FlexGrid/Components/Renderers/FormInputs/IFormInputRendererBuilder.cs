using System;
using System.Reflection;

namespace Blazor.FlexGrid.Components.Renderers.FormInputs
{
    public interface IFormInputRendererBuilder
    {
        bool IsSupportedDateType(Type type);

        Action<IRendererTreeBuilder> BuildRendererTree<TItem>(IActualItemContext<TItem> actualItemContext, PropertyInfo field) where TItem : class;
    }
}
