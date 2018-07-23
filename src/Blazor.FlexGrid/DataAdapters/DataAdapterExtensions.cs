using System;

namespace Blazor.FlexGrid.DataAdapters
{
    public static class DataAdapterExtensions
    {
        public static Type GetUnderlyingType(this ITableDataAdapter tableDataAdapter)
         => tableDataAdapter.GetType().GenericTypeArguments[0];
    }
}
