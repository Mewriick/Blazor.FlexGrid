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
    }
}
