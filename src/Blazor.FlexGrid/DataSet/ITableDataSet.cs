namespace Blazor.FlexGrid.DataSet
{

    /// <summary>
    /// Represents a collection of Items with paging, sorting and 
    /// </summary>
    public interface ITableDataSet : IPageableTableDataSet, ISortableTableDataSet, ISelectableDataSet
    {
    }
}
