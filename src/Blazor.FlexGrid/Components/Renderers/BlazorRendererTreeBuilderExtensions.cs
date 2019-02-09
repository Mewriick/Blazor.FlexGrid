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
    }
}
