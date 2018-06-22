using System;

namespace Blazor.FlexGrid.Components.Configuration
{
    public class PropertyBuilder<TProperty>
    {
        private InternalPropertyBuilder Builder { get; }

        public PropertyBuilder(InternalPropertyBuilder internalPropertyBuilder)
        {
            Builder = internalPropertyBuilder ?? throw new ArgumentNullException(nameof(internalPropertyBuilder));
        }

        public PropertyBuilder<TProperty> HasCaption(string caption)
        {
            Builder.HasCaption(caption);

            return this;
        }

        public PropertyBuilder<TProperty> IsVisible(bool isVisible)
        {
            Builder.IsVisible(isVisible);

            return this;
        }

        public PropertyBuilder<TProperty> HasOrder(int order)
        {
            Builder.HasOrder(order);

            return this;
        }
    }
}
