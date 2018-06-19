using Blazor.FlexGrid.Components.Configuration.MetaData;

namespace Blazor.FlexGrid.Components.Configuration
{
    public abstract class InternalMetadataBuilder<TMetadata> : InternalMetadataBuilder where TMetadata : Annotatable
    {
        public new virtual TMetadata Metadata => base.Metadata as TMetadata;

        protected InternalMetadataBuilder(Annotatable metadata)
            : base(metadata)
        {
        }
    }

    public abstract class InternalMetadataBuilder
    {
        public virtual Annotatable Metadata { get; }

        public abstract InternalModelBuilder ModelBuilder { get; }

        protected InternalMetadataBuilder(Annotatable metadata)
        {
            Metadata = metadata;
        }
    }
}
