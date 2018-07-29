namespace Blazor.FlexGrid.DataAdapters
{
    public static class DataAdapterExtensions
    {
        public static string DefaultTitle(this ITableDataAdapter tableDataAdapter)
        {
            var name = tableDataAdapter.UnderlyingTypeOfItem.Name;
            var nameChars = name.ToCharArray();
            nameChars[0] = char.ToUpper(nameChars[0]);

            return new string(nameChars);
        }

        public static bool IsForSameUnderlyingType(this ITableDataAdapter tableDataAdapter, ITableDataAdapter otherTableDataAdapter)
        {
            var tableDataAdapterType = tableDataAdapter.UnderlyingTypeOfItem;
            var otherTableDataAdapterType = otherTableDataAdapter.UnderlyingTypeOfItem;

            return tableDataAdapterType.FullName == otherTableDataAdapterType.FullName;
        }
    }
}
