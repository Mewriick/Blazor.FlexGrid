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
    }
}
