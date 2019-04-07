using Blazor.FlexGrid.DataSet.Options;
using Blazor.FlexGrid.Filters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blazor.FlexGrid.DataSet.Http
{
    public class NullLazyDataSetLoader<TItem> : ILazyDataSetLoader<TItem> where TItem : class
    {
        public Task<LazyLoadingDataSetResult<TItem>> GetTablePageData(
            ILazyLoadingOptions lazyLoadingOptions,
            IPagingOptions pageableOptions,
            ISortingOptions sortingOptions,
            IReadOnlyCollection<IFilterDefinition> filters = null)
            => Task.FromResult(new LazyLoadingDataSetResult<TItem>()
            {
                Items = new List<TItem>(),
                TotalCount = 0
            });
    }
}
