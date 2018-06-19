using Blazor.FlexGrid.Components.Configuration.MetaData;
using System;

namespace Blazor.FlexGrid.Components.Configuration
{
    public class InternalEntityTypeBuilder : InternalMetadataBuilder<EntityType>
    {
        public override InternalModelBuilder ModelBuilder { get; }

        public InternalEntityTypeBuilder(EntityType metadata, InternalModelBuilder modelBuilder)
            : base(metadata)
        {
            ModelBuilder = modelBuilder;
        }

        public InternalPropertyBuilder Property(string name, Type propertyType)
        {
            var property = Metadata.AddProperty(name, propertyType);

            return new InternalPropertyBuilder(property, new InternalModelBuilder(Metadata.Model));
        }
    }
}
