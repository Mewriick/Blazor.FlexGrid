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

        public bool HasPageSize(int pageSize)
            => HasAnnotation(GridViewAnnotationNames.DetailTabPageSize, pageSize);

        public bool HasCaption(string caption)
            => HasAnnotation(GridViewAnnotationNames.DetailTabPageCaption, caption);
    }
}
