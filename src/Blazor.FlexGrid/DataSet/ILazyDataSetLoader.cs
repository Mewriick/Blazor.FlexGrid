using Blazor.FlexGrid.DataSet.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blazor.FlexGrid.DataSet
{
    public interface ILazyDataSetLoader<TItem> where TItem : class
    {
        Task<IList<TItem>> GetTablePageData(ILazyLoadingOptions lazyLoadingOptions, IPageableOptions pageableOptions);
    }
}
