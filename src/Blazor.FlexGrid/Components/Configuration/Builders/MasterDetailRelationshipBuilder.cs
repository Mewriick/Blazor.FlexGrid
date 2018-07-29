using System;

namespace Blazor.FlexGrid.Components.Configuration.Builders
{
    public class MasterDetailRelationshipBuilder
    {
        private InternalMasterDetailRelationshipBuilder Builder { get; }

        public MasterDetailRelationshipBuilder(InternalMasterDetailRelationshipBuilder builder)
        {
            Builder = builder ?? throw new ArgumentNullException(nameof(builder));
        }

        public MasterDetailRelationshipBuilder HasPageSize(int pageSize)
        {
            Builder.HasPageSize(pageSize);

            return this;
        }

        public MasterDetailRelationshipBuilder HasCaption(string caption)
        {
            if (string.IsNullOrWhiteSpace(caption))
            {
                throw new ArgumentNullException(nameof(caption));
            }

            Builder.HasCaption(caption);

            return this;
        }
    }
}
