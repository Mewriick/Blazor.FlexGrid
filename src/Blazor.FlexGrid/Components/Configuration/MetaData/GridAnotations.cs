using System;

namespace Blazor.FlexGrid.Components.Configuration.MetaData
{
    public class GridAnotations : IGridViewAnotations
    {
        private readonly IEntityType entityTypeMetadata;
        private readonly IAnnotatable annotations;


        public bool IsMasterTable
        {
            get
            {
                var isMasterAnotation = annotations[GridViewAnnotationNames.IsMasterTable];
                if (isMasterAnotation is NullAnotationValue)
                {
                    return false;
                }

                return (bool)isMasterAnotation;
            }
        }

        public GridAnotations(IEntityType entityType)
        {
            this.entityTypeMetadata = entityType ?? throw new ArgumentNullException(nameof(entityType));
            annotations = entityType;
        }

        public IMasterDetailRelationship FindRelationshipConfiguration(Type detailType)
        {
            throw new NotImplementedException();
        }
    }
}
