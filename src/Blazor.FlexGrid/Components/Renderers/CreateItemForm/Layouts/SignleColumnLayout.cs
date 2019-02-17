using Blazor.FlexGrid.Components.Renderers.EditInputs;
using System;

namespace Blazor.FlexGrid.Components.Renderers.CreateItemForm.Layouts
{
    public class SignleColumnLayout<TItem> : BaseCreateItemFormLayout<TItem> where TItem : class
    {
        public override Action<IRendererTreeBuilder> BuildBodyRendererTree(
            CreateItemRendererContext<TItem> createItemRendererContext,
            AbstractEditInputRenderer editInputRenderer)
        {
            return builder =>
            {
                foreach (var field in createItemRendererContext.GetModelFields())
                {
                    BuildFormFieldRendererTree(field, createItemRendererContext, editInputRenderer)?.Invoke(builder);
                }
            };
        }
    }
}
