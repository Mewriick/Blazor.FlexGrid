using Microsoft.AspNetCore.Components.Forms;
using System.Linq.Expressions;

namespace Blazor.FlexGrid.Components.Renderers
{
    public static class BlazorRendererTreeBuilderExtensions
    {
        public static IRendererTreeBuilder OpenElement(this IRendererTreeBuilder rendererTreeBuilder, string elementName, string className)
        {
            rendererTreeBuilder
                .OpenElement(elementName)
                .AddCssClass(className);

            return rendererTreeBuilder;
        }

        public static IRendererTreeBuilder AddCssClass(this IRendererTreeBuilder rendererTreeBuilder, string className)
        {
            rendererTreeBuilder.AddAttribute(HtmlAttributes.Class, className);

            return rendererTreeBuilder;
        }

        public static IRendererTreeBuilder AddValidationMessage<T>(this IRendererTreeBuilder rendererTreeBuilder, LambdaExpression lambdaExpression)
        {
            rendererTreeBuilder
                .OpenComponent(typeof(ValidationMessage<>).MakeGenericType(typeof(T)))
                .AddAttribute("For", lambdaExpression)
                .CloseComponent();

            return rendererTreeBuilder;
        }
    }
}
