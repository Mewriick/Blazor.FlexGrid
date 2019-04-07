namespace Blazor.FlexGrid.Demo.Shared
{
    public class FilterDefinition
    {
        public string ColumnName { get; set; }

        public object Value { get; set; }

        public FilterOperation FilterOperation { get; set; }
    }
}
