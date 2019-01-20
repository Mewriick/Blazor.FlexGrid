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

        public static async Task GoToFirstPage(this ITableDataSet tableDataSet)
        {
            if (!tableDataSet.PageableOptions.IsFirstPage)
            {
                await tableDataSet.GoToPage(0);
            }
        }

        public static async Task GoToLastPage(this ITableDataSet tableDataSet)
        {
            if (!tableDataSet.PageableOptions.IsLastPage)
            {
                await tableDataSet.GoToPage(tableDataSet.PageableOptions.PagesCount - 1);
            }
        }

        public static string PageInfoText(this ITableDataSet tableDataSet)
        {
            var from = tableDataSet.PageableOptions.CurrentPage * tableDataSet.PageableOptions.PageSize + 1;
            var to = from + tableDataSet.PageableOptions.PageSize - 1;

            return $"{from} - {to}";
        }

        public static bool IsItemEdited(this ITableDataSet tableDataSet, object item)
            => tableDataSet.RowEditOptions.ItemInEditMode == item;
    }
}