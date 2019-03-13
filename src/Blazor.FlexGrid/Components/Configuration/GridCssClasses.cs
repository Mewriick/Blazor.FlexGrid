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

        public GridFooterClasses FooterCssClasses { get; set; } = new GridFooterClasses();

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

        internal void AppendDefaultFooterCssClasses(DefaultFooterClasses defaultFooterClasses)
        {
            FooterCssClasses = new GridFooterClasses
            {
                FooterWrapper = $"{defaultFooterClasses.FooterWrapper} {FooterCssClasses.FooterWrapper}".TrimEnd(),
                PaginationButton = $"{defaultFooterClasses.PaginationButton} {FooterCssClasses.PaginationButton}".TrimEnd(),
                PaginationButtonDisabled = $"{defaultFooterClasses.PaginationButtonDisabled} {FooterCssClasses.PaginationButtonDisabled}".TrimEnd()
            };
        }
    }

    public class GridFooterClasses
    {
        public string FooterWrapper { get; set; } = string.Empty;

        public string PaginationButton { get; set; } = string.Empty;

        public string PaginationButtonDisabled { get; set; } = string.Empty;
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
            FooterCssClasses = new DefaultFooterClasses();
        }
    }

    public class DefaultFooterClasses : GridFooterClasses
    {
        public DefaultFooterClasses()
        {
            FooterWrapper = "pagination-wrapper-inner";
            PaginationButton = "pagination-button";
            PaginationButtonDisabled = "pagination-button pagination-button-disabled";
        }
    }
}
