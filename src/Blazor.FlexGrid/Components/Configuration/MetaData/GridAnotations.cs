﻿using System;

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
                if (masterDetailOptions is NullAnotationValue)
                {
                    return NullMasterDetailOptions.Instance;
                }

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

        public DeleteItemOptions DeleteItemOptions
        {
            get
            {
                var deleteItemOptions = annotations[GridViewAnnotationNames.DeleteItemOptions];
                if (deleteItemOptions is NullAnotationValue)
                {
                    return new DefaulDeleteItemOptions();
                }

                return (DeleteItemOptions)deleteItemOptions;
            }
        }

        public bool OnlyShowExplicitProperties
        {
            get
            {
                var onlyShowExplicitProperties = annotations[GridViewAnnotationNames.OnlyShowExplicitProperties];
                if (onlyShowExplicitProperties is NullAnotationValue)
                {
                    return false;
                }

                return (bool)onlyShowExplicitProperties;
            }
        }

        public GlobalGroupingOptions GroupingOptions
        {
            get
            {
                var groupingOptions = annotations[GridViewAnnotationNames.GroupingOptions];
                if (groupingOptions is NullAnotationValue)
                {
                    return NullGlobalGroupingOptions.Instance;
                }

                return (GlobalGroupingOptions)groupingOptions;
            }
        }

        public string EmptyItemsMessage
        {
            get
            {
                var emptyItemsMessage = annotations[GridViewAnnotationNames.EmptyItemsMessage];
                if (emptyItemsMessage is NullAnotationValue)
                {
                    return "No data to show here ...";
                }

                return emptyItemsMessage.ToString();
            }
        }

        public bool RenderHeaderWithEmtyItemsMessage
        {
            get
            {
                var renderHeaderWithEmtyItemsMessage = annotations[GridViewAnnotationNames.RenderHeaderWithEmtyItemsMessage];
                if (renderHeaderWithEmtyItemsMessage is NullAnotationValue)
                {
                    return false;
                }

                return (bool)renderHeaderWithEmtyItemsMessage;
            }
        }

        public bool PreserveFiltering
        {
            get
            {
                var preserveFiltering = annotations[GridViewAnnotationNames.PreserveFiltering];
                if (preserveFiltering is NullAnotationValue)
                {
                    return false;
                }

                return (bool)preserveFiltering;
            }
        }

        public bool PreservePagination
        {
            get
            {
                var preservePagination = annotations[GridViewAnnotationNames.PreservePagination];
                if (preservePagination is NullAnotationValue)
                {
                    return false;
                }

                return (bool)preservePagination;
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
