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
    }
}
