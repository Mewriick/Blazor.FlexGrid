using Blazor.FlexGrid.Components.Configuration.MetaData;

namespace Blazor.FlexGrid.Components.Configuration
{
    public static class MetadataExtensions
    {
        public static IGridViewColumnAnnotations FindColumnConfiguration(this IEntityType entityType, string columnName)
        {
            var property = entityType.FindProperty(columnName);
            if (property is null)
            {
                return null;
            }

            return new GridColumnAnnotations(property);
        }
    }
}
