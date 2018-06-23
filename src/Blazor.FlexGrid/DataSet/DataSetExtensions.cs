namespace Blazor.FlexGrid.DataSet
{
    public static class DataSetExtensions
    {
        public static void GoToNextPage(this ITableDataSet tableDataSet)
        {
            if (!tableDataSet.PageableOptions.IsLastPage)
            {
                tableDataSet.GoToPage(tableDataSet.PageableOptions.CurrentPage + 1);
            }
        }
    }
}
