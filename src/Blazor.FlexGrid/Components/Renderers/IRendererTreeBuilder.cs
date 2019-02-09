using Microsoft.AspNetCore.Components;
using System;

namespace Blazor.FlexGrid.Components.Renderers
{
    public interface IRendererTreeBuilder
    {
        IRendererTreeBuilder OpenElement(string elementName);

        IRendererTreeBuilder CloseElement();

        IRendererTreeBuilder AddAttribute(string name, object value);

        IRendererTreeBuilder AddAttribute(string name, bool value);

        IRendererTreeBuilder AddAttribute(string name, string value);

        IRendererTreeBuilder AddAttribute(string name, MulticastDelegate value);

        IRendererTreeBuilder AddAttribute(string name, Action<UIEventArgs> value);

        IRendererTreeBuilder AddContent(string textContent);

        IRendererTreeBuilder AddContent(RenderFragment fragment);

        IRendererTreeBuilder AddContent(MarkupString markupContent);

        IRendererTreeBuilder OpenComponent(Type componentType);

        IRendererTreeBuilder CloseComponent();
    }
}
