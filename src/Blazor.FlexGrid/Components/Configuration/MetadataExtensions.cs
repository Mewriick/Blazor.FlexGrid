using Blazor.FlexGrid.Components.Configuration.MetaData;
using Blazor.FlexGrid.DataAdapters;
using Blazor.FlexGrid.DataSet;
using System;

namespace Blazor.FlexGrid.Components.Configuration
{
    public static class MetadataExtensions
    {
        public static IGridViewColumnAnotations FindColumnConfiguration(this IEntityType entityType, string columnName)
        {
            var property = entityType.FindProperty(columnName);
            if (property is null)
            {
                return null;
            }

            return new GridColumnAnotations(property);
        }

        public static bool IsConfiguredAsMasterTable(this IEntityType entityType)
        {
            var masterTableAnnotationValue = entityType[GridViewAnnotationNames.IsMasterTable];
            if (masterTableAnnotationValue is NullAnotationValue)
            {
                return false;
            }

            return (bool)masterTableAnnotationValue;
        }

        public static int DetailGridViewPageSize(this IMasterDetailRelationship masterDetailRelationship, ITableDataSet masterTableDataSet)
        {
            if (masterTableDataSet == null)
            {
                throw new ArgumentNullException(nameof(masterTableDataSet));
            }

            var pageSizeAnnotationValue = masterDetailRelationship[GridViewAnnotationNames.DetailTabPageSize];
            if (pageSizeAnnotationValue is NullAnotationValue)
            {
                return masterTableDataSet.PageableOptions.PageSize;
            }

            return (int)pageSizeAnnotationValue;
        }

        public static string DetailGridViewPageCaption(this IMasterDetailRelationship masterDetailRelationship, ITableDataAdapter tableDataAdapter)
        {
            if (tableDataAdapter == null)
            {
                throw new ArgumentNullException(nameof(tableDataAdapter));
            }

            var tabCaptionAnnotationValue = masterDetailRelationship[GridViewAnnotationNames.DetailTabPageCaption];
            if (tabCaptionAnnotationValue is NullAnotationValue)
            {
                return tableDataAdapter.DefaultTitle();
            }

            return tabCaptionAnnotationValue.ToString();
        }

        public static string DetailGridLazyLoadingUrl(this IMasterDetailRelationship masterDetailRelationship)
        {
            var lazyLoadingUrl = masterDetailRelationship[GridViewAnnotationNames.DetailLazyLoadingUrl];
            if (lazyLoadingUrl is NullAnotationValue)
            {
                return string.Empty;
            }

            return lazyLoadingUrl.ToString();
        }

        public static string DetailGridUpdateUrl(this IMasterDetailRelationship masterDetailRelationship)
        {
            var updateUrl = masterDetailRelationship[GridViewAnnotationNames.DetailUpdateUrl];
            if (updateUrl is NullAnotationValue)
            {
                return string.Empty;
            }

            return updateUrl.ToString();
        }

        public static string DetailGridDeleteUrl(this IMasterDetailRelationship masterDetailRelationship)
        {
            var deleteUrl = masterDetailRelationship[GridViewAnnotationNames.DetailDeleteUrl];
            if (deleteUrl is NullAnotationValue)
            {
                return string.Empty;
            }

            return deleteUrl.ToString();
        }
    }
}
