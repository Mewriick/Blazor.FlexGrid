using Blazor.FlexGrid.DataSet.Options;
using System.Threading.Tasks;

namespace Blazor.FlexGrid.DataSet.Http
{
    public class NullLazyDataSetItemManipulator<TItem> : ILazyDataSetItemManipulator<TItem> where TItem : class
    {
        public Task<TItem> DeleteItem(TItem item, ILazyLoadingOptions lazyLoadingOptions)
            => Task.FromResult(item);

        public Task<TItem> SaveItem(TItem item, ILazyLoadingOptions lazyLoadingOptions)
            => Task.FromResult(item);
    }
}
