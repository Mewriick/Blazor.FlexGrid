using Blazor.FlexGrid.Components.Configuration.MetaData;

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

        public static GridCssClasses CssClasses(this IEntityType entityType)
        {
            var cssClasses = entityType[GridViewAnnotationNames.CssClasses];
            if (cssClasses is NullAnotationValue)
            {
                return new DefaultGridCssClasses();
            }

            return cssClasses as GridCssClasses;
        }

        public static string CreateColumnUniqueName(this IEntityType entityType, string columnName)
        {
            return $"{entityType.Name}_{columnName}";
        }
    }
}
