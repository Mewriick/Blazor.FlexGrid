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

        
        public MasterDetailOptions MasterDetailOptions
        {
            get
            {
                var masterDetailOptions = annotations[GridViewAnnotationNames.MasterDetailOptions];
                
                return (MasterDetailOptions)masterDetailOptions;
            }
        }

        public GridCssClasses CssClasses
        {
            get
            {
                var cssClasses = annotations[GridViewAnnotationNames.CssClasses];
                if (cssClasses is NullAnotationValue)
                {
                    return new DefaultGridCssClasses();
                }

                return cssClasses as GridCssClasses;
            }
        }

        public InlineEditOptions InlineEditOptions
        {
            get
            {
                var inlineEditOptions = annotations[GridViewAnnotationNames.InlineEditOptions];
                if (inlineEditOptions is NullAnotationValue)
                {
                    return NullInlineEditOptions.Instance;
                }

                return (InlineEditOptions)inlineEditOptions;
            }
        }

        public CreateItemOptions CreateItemOptions
        {
            get
            {

                var createItemOptions = annotations[GridViewAnnotationNames.CreateItemOptions];
                if (createItemOptions is NullAnotationValue)
                {
                    return NullCreateItemOptions.Instance;
                }

                return (CreateItemOptions)createItemOptions;
            }
        }

        

        public GridAnotations(IEntityType entityType)
        {
            this.entityTypeMetadata = entityType ?? throw new ArgumentNullException(nameof(entityType));
            annotations = entityType;
        }

        public IMasterDetailRelationship FindRelationshipConfiguration(Type detailType)
        {
            var masterDetailConnection = entityTypeMetadata.FindDetailRelationship(detailType);
            if (masterDetailConnection is null)
            {
                throw new InvalidOperationException($"If you want to use Master/Detail functionallity, you must configure relationship using method HasDetailRelationship");
            }

            return masterDetailConnection;
        }
    }
}
