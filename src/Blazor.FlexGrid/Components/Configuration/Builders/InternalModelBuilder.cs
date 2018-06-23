using Blazor.FlexGrid.Components.Configuration.MetaData;
using System;

namespace Blazor.FlexGrid.Components.Configuration.Builders
{
    public class InternalModelBuilder : InternalMetadataBuilder<Model>
    {
        public override InternalModelBuilder ModelBuilder => this;

        public InternalModelBuilder(Model metadata)
            : base(metadata)
        {
        }

        public InternalEntityTypeBuilder Entity(Type type)
        {
            var entityType = Metadata.AddEntityType(type);

            return new InternalEntityTypeBuilder(entityType, new InternalModelBuilder(entityType.Model));
        }
    }
}
