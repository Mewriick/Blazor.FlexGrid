namespace Blazor.FlexGrid.Components.Configuration
{
    public class GridCssClasses
    {
        public string Table { get; set; } = string.Empty;

        public string TableBody { get; set; } = string.Empty;

        public string TableCell { get; set; } = string.Empty;

        public string TableRow { get; set; } = string.Empty;

        public string TableHeader { get; set; } = string.Empty;

        public string TableHeaderRow { get; set; } = string.Empty;

        public string TableHeaderCell { get; set; } = string.Empty;

        internal void AppendDefaultCssClasses(DefaultGridCssClasses defaultCssClasses)
        {
            Table = $"{defaultCssClasses.Table} {Table}".TrimEnd();
            TableBody = $"{defaultCssClasses.TableBody} {TableBody}".TrimEnd();
            TableCell = $"{defaultCssClasses.TableCell} {TableCell}".TrimEnd();
            TableRow = $"{defaultCssClasses.TableRow} {TableRow}".TrimEnd();
            TableHeaderCell = $"{defaultCssClasses.TableHeaderCell} {TableHeaderCell}".TrimEnd();
            TableHeaderRow = $"{defaultCssClasses.TableHeaderRow} {TableHeaderRow}".TrimEnd();
            TableHeader = $"{defaultCssClasses.TableHeader} {TableHeader}".TrimEnd();
        }
    }

    public class DefaultGridCssClasses : GridCssClasses
    {
        public DefaultGridCssClasses()
        {
            Table = "flex-table";
            TableBody = "table-body";
            TableCell = "table-cell";
            TableRow = "table-row";
            TableHeaderCell = "table-cell-head";
            TableHeaderRow = "table-head-row";
            TableHeader = "table-head";
        }
    }
}
