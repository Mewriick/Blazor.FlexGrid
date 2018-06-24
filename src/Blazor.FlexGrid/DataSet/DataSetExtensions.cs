namespace Blazor.FlexGrid.DataSet
{
    public static class DataSetExtensions
    {
        public static bool HasItems(this ITableDataSet tableDataSet)
            => !(tableDataSet.Items == null || tableDataSet.Items.Count <= 0);

        public static void GoToNextPage(this ITableDataSet tableDataSet)
        {
            if (!tableDataSet.PageableOptions.IsLastPage)
            {
                tableDataSet.GoToPage(tableDataSet.PageableOptions.CurrentPage + 1);
            }
        }

        public static void GoToPreviousPage(this ITableDataSet tableDataSet)
        {
            if (!tableDataSet.PageableOptions.IsFirstPage)
            {
                tableDataSet.GoToPage(tableDataSet.PageableOptions.CurrentPage - 1);
            }
        }

        public static void GoToFirstPage(this ITableDataSet tableDataSet)
        {
            if (!tableDataSet.PageableOptions.IsFirstPage)
            {
                tableDataSet.GoToPage(0);
            }
        }

        public static void GoToLastPage(this ITableDataSet tableDataSet)
        {
            if (!tableDataSet.PageableOptions.IsLastPage)
            {
                tableDataSet.GoToPage(tableDataSet.PageableOptions.PagesCount - 1);
            }
        }
    }
}
