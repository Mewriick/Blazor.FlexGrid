using Blazor.FlexGrid.Components.Renderers.FormInputs;
using System;

namespace Blazor.FlexGrid.Components.Renderers.CreateItemForm.Layouts
{
    public class SignleColumnLayout<TItem> : BaseCreateItemFormLayout<TItem> where TItem : class
    {
        public override Action<IRendererTreeBuilder> BuildBodyRendererTree(
            CreateItemRendererContext<TItem> createItemRendererContext,
            IFormInputRendererTreeProvider formInputRendererTreeProvider)
        {
            return builder =>
            {
                builder.OpenElement(HtmlTagNames.Div, "center-block");

                foreach (var field in createItemRendererContext.GetModelFields())
                {
                    BuildFormFieldRendererTree(field, createItemRendererContext, formInputRendererTreeProvider)?.Invoke(builder);
                }

                builder.CloseElement();
            };
        }
    }
}
