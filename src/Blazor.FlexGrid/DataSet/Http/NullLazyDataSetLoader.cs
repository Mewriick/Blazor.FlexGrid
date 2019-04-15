using Blazor.FlexGrid.Filters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blazor.FlexGrid.DataSet.Http
{
    public class NullLazyDataSetLoader<TItem> : ILazyDataSetLoader<TItem> where TItem : class
    {
        public Task<LazyLoadingDataSetResult<GroupItem<TItem>>> GetGroupedTablePageData(
            RequestOptions requestOptions,
            IReadOnlyCollection<IFilterDefinition> filterDefinitions = null)
                => Task.FromResult(new LazyLoadingDataSetResult<GroupItem<TItem>>()
                {
                    Items = new List<GroupItem<TItem>>(),
                    TotalCount = 0
                });

        public Task<LazyLoadingDataSetResult<TItem>> GetTablePageData(
            RequestOptions requestOptions,
            IReadOnlyCollection<IFilterDefinition> filterDefinitions = null)
                => Task.FromResult(new LazyLoadingDataSetResult<TItem>()
                {
                    Items = new List<TItem>(),
                    TotalCount = 0
                });
    }
}
