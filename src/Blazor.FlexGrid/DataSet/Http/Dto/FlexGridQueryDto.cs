namespace Blazor.FlexGrid.DataSet.Http.Dto
{
    public class FlexGridQueryDto
    {
        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public bool SortDescending { get; set; } = false;

        public string SortExpression { get; set; } = string.Empty;

        public string GroupExpression { get; set; } = string.Empty;
    }
}
