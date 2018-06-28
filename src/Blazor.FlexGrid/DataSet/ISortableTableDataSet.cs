using Blazor.FlexGrid.DataSet.Options;
using System.Threading.Tasks;

namespace Blazor.FlexGrid.DataSet
{
    /// <summary>
    /// Define contract for DataSet supporting sorting
    /// </summary>
    public interface ISortableTableDataSet : IBaseTableDataSet
    {
        ISortingOptions SortingOptions { get; }

        Task SetSortExpression(string expression);
    }
}
