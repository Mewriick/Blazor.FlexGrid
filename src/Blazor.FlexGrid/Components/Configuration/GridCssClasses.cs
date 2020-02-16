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

        public string TableGroupRow { get; set; } = string.Empty;

        public string TableGroupRowCell { get; set; } = string.Empty;

        public GridFooterCssClasses FooterCssClasses { get; set; } = NullFooterCssClasses.Instance;

        public CreateFormCssClasses CreateFormCssClasses { get; set; } = NullCreateFormCssClasses.Instance;

        public DeleteDialogCssClasses DeleteDialogCssClasses { get; set; } = new DeleteDialogCssClasses();

        internal void AppendDefaultCssClasses(DefaultGridCssClasses defaultCssClasses)
        {
            Table = $"{defaultCssClasses.Table} {Table}".TrimEnd();
            TableBody = $"{defaultCssClasses.TableBody} {TableBody}".TrimEnd();
            TableCell = $"{defaultCssClasses.TableCell} {TableCell}".TrimEnd();
            TableRow = $"{defaultCssClasses.TableRow} {TableRow}".TrimEnd();
            TableHeaderCell = $"{defaultCssClasses.TableHeaderCell} {TableHeaderCell}".TrimEnd();
            TableHeaderRow = $"{defaultCssClasses.TableHeaderRow} {TableHeaderRow}".TrimEnd();
            TableHeader = $"{defaultCssClasses.TableHeader} {TableHeader}".TrimEnd();
            TableGroupRow = $"{defaultCssClasses.TableGroupRow} {TableGroupRow}".TrimEnd();
            TableGroupRowCell = $"{defaultCssClasses.TableGroupRowCell} {TableGroupRowCell}".TrimEnd();
        }

        internal void AppendDefaultFooterCssClasses(DefaultFooterCssClasses defaultFooterClasses)
        {
            FooterCssClasses = new GridFooterCssClasses
            {
                FooterWrapper = $"{defaultFooterClasses.FooterWrapper} {FooterCssClasses.FooterWrapper}".TrimEnd(),
                PaginationButton = $"{defaultFooterClasses.PaginationButton} {FooterCssClasses.PaginationButton}".TrimEnd(),
                PaginationButtonDisabled = $"{defaultFooterClasses.PaginationButtonDisabled} {FooterCssClasses.PaginationButtonDisabled}".TrimEnd(),
                GroupingPartWrapper = $"{defaultFooterClasses.GroupingPartWrapper} {FooterCssClasses.GroupingPartWrapper}".TrimEnd()
            };
        }

        internal void AppendDefaultCreateFormCssClasses(DefaultCreateFormCssClasses defaultCreateFormCssClasses)
        {
            CreateFormCssClasses = new CreateFormCssClasses
            {
                SubmitButton = $"{defaultCreateFormCssClasses.SubmitButton} {CreateFormCssClasses.SubmitButton}".TrimEnd(),
                FieldName = $"{defaultCreateFormCssClasses.FieldName} {CreateFormCssClasses.FieldName}".TrimEnd(),
                ModalHeader = $"{defaultCreateFormCssClasses.ModalHeader} {CreateFormCssClasses.ModalHeader}".TrimEnd(),
                ModalBody = $"{defaultCreateFormCssClasses.ModalBody} {CreateFormCssClasses.ModalBody}".TrimEnd(),
                ModalFooter = $"{defaultCreateFormCssClasses.ModalFooter} {CreateFormCssClasses.ModalFooter}".TrimEnd(),
                ModalSize = $"{defaultCreateFormCssClasses.ModalSize} {CreateFormCssClasses.ModalSize}".TrimEnd()
            };
        }

        internal void AppendDefaultDeleteDialogCssClasses(DefaultDeleteDialogCssClasses defaultDeleteDialogCssClasses)
        {
            DeleteDialogCssClasses = new DeleteDialogCssClasses
            {
                DeleteButton = $"{defaultDeleteDialogCssClasses.DeleteButton} {DeleteDialogCssClasses.DeleteButton}".TrimEnd(),
                CancelButton = $"{defaultDeleteDialogCssClasses.CancelButton} {DeleteDialogCssClasses.CancelButton}".TrimEnd()
            };
        }
    }

    public class GridFooterCssClasses
    {
        public string FooterWrapper { get; set; } = string.Empty;

        public string PaginationButton { get; set; } = string.Empty;

        public string PaginationButtonDisabled { get; set; } = string.Empty;

        public string GroupingPartWrapper { get; set; } = string.Empty;
    }

    public class CreateFormCssClasses
    {
        public string SubmitButton { get; set; } = string.Empty;

        public string FieldName { get; set; } = string.Empty;

        public string ModalSize { get; set; } = string.Empty;

        public string ModalHeader { get; set; } = string.Empty;

        public string ModalBody { get; set; } = string.Empty;

        public string ModalFooter { get; set; } = string.Empty;
    }

    public class DeleteDialogCssClasses
    {
        public string DeleteButton { get; set; } = string.Empty;

        public string CancelButton { get; set; } = string.Empty;
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
            TableGroupRow = "table-group-row";
            TableGroupRowCell = "table-group-row-cell";
            FooterCssClasses = new DefaultFooterCssClasses();
            CreateFormCssClasses = new DefaultCreateFormCssClasses();
            DeleteDialogCssClasses = new DefaultDeleteDialogCssClasses();
        }
    }

    public class DefaultFooterCssClasses : GridFooterCssClasses
    {
        public DefaultFooterCssClasses()
        {
            FooterWrapper = "pagination-wrapper-inner";
            PaginationButton = "pagination-button";
            PaginationButtonDisabled = "pagination-button pagination-button-disabled";
            GroupingPartWrapper = "grouping-part-wrapper";
        }
    }

    public class NullFooterCssClasses : GridFooterCssClasses
    {
        public static NullFooterCssClasses Instance = new NullFooterCssClasses();
    }

    public class DefaultCreateFormCssClasses : CreateFormCssClasses
    {
        public DefaultCreateFormCssClasses()
        {
            SubmitButton = "btn btn-primary";
            FieldName = "edit-field-name";
            ModalSize = "modal";
            ModalHeader = "modal-header";
            ModalBody = "modal-body";
            ModalFooter = "modal-footer";
        }
    }

    public class DefaultDeleteDialogCssClasses : DeleteDialogCssClasses
    {
        public DefaultDeleteDialogCssClasses()
        {
            CancelButton = "btn btn-light";
            DeleteButton = "btn btn-danger";
        }
    }

    public class NullCreateFormCssClasses : CreateFormCssClasses
    {
        public static NullCreateFormCssClasses Instance = new NullCreateFormCssClasses();
    }
}
