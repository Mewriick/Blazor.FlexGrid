using Blazor.FlexGrid.DataSet.Http;
using Blazor.FlexGrid.Filters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blazor.FlexGrid.DataSet
{
    public interface ILazyDataSetLoader<TItem> where TItem : class
    {
        Task<LazyLoadingDataSetResult<TItem>> GetTablePageData(RequestOptions requestOptions, IReadOnlyCollection<IFilterDefinition> filterDefinitions = null);

        Task<LazyLoadingDataSetResult<GroupItem<TItem>>> GetGroupedTablePageData(RequestOptions requestOptions, IReadOnlyCollection<IFilterDefinition> filterDefinitions = null);
    }
}
