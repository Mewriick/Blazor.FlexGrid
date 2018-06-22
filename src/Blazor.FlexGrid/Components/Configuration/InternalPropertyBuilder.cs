using Blazor.FlexGrid.Components.Configuration.MetaData;
using System;

namespace Blazor.FlexGrid.Components.Configuration
{
    public class InternalPropertyBuilder : InternalMetadataBuilder<Property>
    {
        public override InternalModelBuilder ModelBuilder { get; }

        public InternalPropertyBuilder(Property metadata, InternalModelBuilder internalModelBuilder)
            : base(metadata)
        {
            ModelBuilder = internalModelBuilder ?? throw new ArgumentNullException(nameof(internalModelBuilder));
        }

        public bool HasCaption(string caption)
            => HasAnnotation(GridViewAnnotationNames.ColumnCaption, caption);

        public bool IsVisible(bool isVisible)
            => HasAnnotation(GridViewAnnotationNames.ColumnIsVisible, isVisible);

        public bool HasOrder(int order)
            => HasAnnotation(GridViewAnnotationNames.ColumnOrder, order);
    }
}
