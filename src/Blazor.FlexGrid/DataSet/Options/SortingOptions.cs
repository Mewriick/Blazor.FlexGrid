namespace Blazor.FlexGrid.DataSet.Options
{
    public class SortingOptions : ISortingOptions
    {
        public bool SortDescending { get; set; } = false;

        public string SortExpression { get; set; }
    }
}
