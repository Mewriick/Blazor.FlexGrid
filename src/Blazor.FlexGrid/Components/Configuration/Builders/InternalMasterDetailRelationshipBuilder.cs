using Blazor.FlexGrid.Components.Configuration.MetaData;
using System;

namespace Blazor.FlexGrid.Components.Configuration.Builders
{
    public class InternalMasterDetailRelationshipBuilder : InternalMetadataBuilder<MasterDetailRelationship>
    {
        public override InternalModelBuilder ModelBuilder { get; }

        public InternalMasterDetailRelationshipBuilder(MasterDetailRelationship metadata, InternalModelBuilder internalModelBuilder)
            : base(metadata)
        {
            ModelBuilder = internalModelBuilder ?? throw new ArgumentNullException(nameof(internalModelBuilder));
        }
    }
}
