namespace Blazor.FlexGrid.DataSet.Options
{
    public class RowEditOptions : IRowEditOptions
    {
        public object ItemInEditMode { get; set; } = EmptyDataSetItem.Instance;
    }
}
