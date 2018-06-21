using Blazor.FlexGrid.Components.Configuration.MetaData;
using System.Reflection;

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

        public InternalPropertyBuilder Property(MemberInfo memberInfo)
        {
            var property = Metadata.AddProperty(memberInfo);

            return new InternalPropertyBuilder(property, new InternalModelBuilder(Metadata.Model));
        }
    }
}
