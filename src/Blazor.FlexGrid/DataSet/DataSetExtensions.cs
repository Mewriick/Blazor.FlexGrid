using System;
using System.Threading.Tasks;

namespace Blazor.FlexGrid.DataSet
{
    public static class DataSetExtensions
    {
        public static Type UnderlyingTypeOfItem(this ITableDataSet tableDataSet)
            => tableDataSet.GetType().GenericTypeArguments[0];

        public static bool HasItems(this ITableDataSet tableDataSet)
            => !(tableDataSet.Items == null || tableDataSet.Items.Count <= 0);

        public static Task GoToNextPage(this ITableDataSet tableDataSet)
        {
            if (!tableDataSet.PageableOptions.IsLastPage)
            {
                return tableDataSet.GoToPage(tableDataSet.PageableOptions.CurrentPage + 1);
            }

            return Task.FromResult(0);
        }

        public static Task GoToPreviousPage(this ITableDataSet tableDataSet)
        {
            if (!tableDataSet.PageableOptions.IsFirstPage)
            {
                return tableDataSet.GoToPage(tableDataSet.PageableOptions.CurrentPage - 1);
            }

            return Task.FromResult(0);
        }

        public static Task GoToFirstPage(this ITableDataSet tableDataSet)
        {
            if (!tableDataSet.PageableOptions.IsFirstPage)
            {
                tableDataSet.GoToPage(0);
            }

            return Task.FromResult(0);
        }

        public static Task GoToLastPage(this ITableDataSet tableDataSet)
        {
            if (!tableDataSet.PageableOptions.IsLastPage)
            {
                tableDataSet.GoToPage(tableDataSet.PageableOptions.PagesCount - 1);
            }

            return Task.FromResult(0);
        }

        public static string PageInfoText(this ITableDataSet tableDataSet)
        {
            var from = tableDataSet.PageableOptions.CurrentPage * tableDataSet.PageableOptions.PageSize + 1;
            var to = from + tableDataSet.PageableOptions.PageSize - 1;

            return $"{from} - {to}";
        }
    }
}