using Blazor.FlexGrid.DataSet.Options;
using Blazor.FlexGrid.Filters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blazor.FlexGrid.DataSet
{
    public interface ILazyDataSetLoader<TItem> where TItem : class
    {
        Task<LazyLoadingDataSetResult<TItem>> GetTablePageData(
            ILazyLoadingOptions lazyLoadingOptions,
            IPagingOptions pageableOptions,
            ISortingOptions sortingOptions,
            IReadOnlyCollection<IFilterDefinition> filterDefinitions = null);
    }
}
