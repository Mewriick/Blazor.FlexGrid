using Blazor.FlexGrid.DataSet.Options;
using System.Threading.Tasks;

namespace Blazor.FlexGrid.DataSet
{
    /// <summary>
    /// Define contract for DataSet supporting pagination
    /// </summary>
    public interface IPageableTableDataSet : IBaseTableDataSet
    {
        IPagingOptions PageableOptions { get; }

        Task GoToPage(int index);
    }
}
