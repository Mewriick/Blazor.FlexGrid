using Blazor.FlexGrid.Filters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blazor.FlexGrid.DataSet.Http
{
    public class NullLazyGroupableDataSetLoader<TItem> : ILazyGroupableDataSetLoader<TItem> where TItem : class
    {
        public Task<LazyLoadingDataSetResult<GroupItem<TItem>>> GetGroupedTablePageData(
            RequestOptions requestOptions,
            IReadOnlyCollection<IFilterDefinition> filterDefinitions = null)
                => Task.FromResult(new LazyLoadingDataSetResult<GroupItem<TItem>>()
                {
                    Items = new List<GroupItem<TItem>>(),
                    TotalCount = 0
                });
    }
}
