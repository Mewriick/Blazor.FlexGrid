using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.RenderTree;
using System;

namespace Blazor.FlexGrid.Components.Renderers
{
    public class BlazorRendererTreeBuilder : IRendererTreeBuilder
    {
        public const string ChildContent = "ChildContent";

        private readonly RenderTreeBuilder renderTreeBuilder;
        private int sequence = 0;

        public BlazorRendererTreeBuilder(RenderTreeBuilder renderTreeBuilder)
        {
            this.renderTreeBuilder = renderTreeBuilder ?? throw new ArgumentNullException(nameof(renderTreeBuilder));
        }

        public IRendererTreeBuilder AddAttribute(string name, object value)
        {
            renderTreeBuilder.AddAttribute(++sequence, name, value);

            return this;
        }

        public IRendererTreeBuilder AddAttribute(string name, bool value)
        {
            renderTreeBuilder.AddAttribute(++sequence, name, value);

            return this;
        }

        public IRendererTreeBuilder AddAttribute(string name, string value)
        {
            renderTreeBuilder.AddAttribute(++sequence, name, value);

            return this;
        }

        public IRendererTreeBuilder AddAttribute(string name, MulticastDelegate value)
        {
            renderTreeBuilder.AddAttribute(++sequence, name, value);

            return this;
        }

        public IRendererTreeBuilder AddAttribute(string name, Action<UIEventArgs> value)
        {
            renderTreeBuilder.AddAttribute(++sequence, name, value);

            return this;
        }

        public IRendererTreeBuilder AddAttribute(string name, Func<MulticastDelegate> value)
        {
            renderTreeBuilder.AddAttribute(++sequence, name, value);

            return this;
        }

        public IRendererTreeBuilder AddContent(string textContent)
        {
            renderTreeBuilder.AddContent(++sequence, textContent);

            return this;
        }

        public IRendererTreeBuilder AddContent(RenderFragment fragment)
        {
            renderTreeBuilder.AddContent(++sequence, fragment);

            return this;
        }

        public IRendererTreeBuilder AddContent(MarkupString markupContent)
        {
            renderTreeBuilder.AddContent(++sequence, markupContent);

            return this;
        }

        public IRendererTreeBuilder CloseComponent()
        {
            renderTreeBuilder.CloseComponent();

            return this;
        }

        public IRendererTreeBuilder CloseElement()
        {
            renderTreeBuilder.CloseElement();

            return this;
        }

        public IRendererTreeBuilder OpenComponent(Type componentType)
        {
            renderTreeBuilder.OpenComponent(++sequence, componentType);

            return this;
        }

        public IRendererTreeBuilder OpenElement(string elementName)
        {
            renderTreeBuilder.OpenElement(++sequence, elementName);

            return this;
        }
    }
}
