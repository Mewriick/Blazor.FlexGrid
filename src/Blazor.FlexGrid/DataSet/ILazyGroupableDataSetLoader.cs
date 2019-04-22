using Blazor.FlexGrid.DataSet.Http;
using Blazor.FlexGrid.Filters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blazor.FlexGrid.DataSet
{
    public interface ILazyGroupableDataSetLoader<TItem>
    {
        Task<LazyLoadingDataSetResult<GroupItem<TItem>>> GetGroupedTablePageData(
            RequestOptions requestOptions,
            IReadOnlyCollection<IFilterDefinition> filterDefinitions = null);
    }
}
