using Blazor.FlexGrid.Filters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blazor.FlexGrid.DataSet
{
    public interface IFilterableDataSet : IBaseTableDataSet
    {
        bool FilterIsApplied { get; }

        Task ApplyFilters(IReadOnlyCollection<IFilterDefinition> filters, bool goToFirstPage = true);
    }
}
