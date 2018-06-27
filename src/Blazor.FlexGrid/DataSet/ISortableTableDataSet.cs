using Blazor.FlexGrid.DataSet.Options;

namespace Blazor.FlexGrid.DataSet
{
    /// <summary>
    /// Define contract for DataSet supporting sorting
    /// </summary>
    public interface ISortableTableDataSet : IBaseTableDataSet
    {
        ISortingOptions SortingOptions { get; }

        void SetSortExpression(string expression);
    }
}
