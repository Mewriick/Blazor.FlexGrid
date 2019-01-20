using Blazor.FlexGrid.DataSet;
using Blazor.FlexGrid.DataSet.Options;
using System.Threading.Tasks;

namespace Blazor.FlexGrid.DataAdapters
{
    public class NullLazyDataSetItemSaver<TItem> : ILazyDataSetItemManipulator<TItem> where TItem : class
    {
        public static NullLazyDataSetItemSaver<TItem> Instance = new NullLazyDataSetItemSaver<TItem>();

        public Task<TItem> DeleteItem(TItem item, ILazyLoadingOptions lazyLoadingOptions)
            => Task.FromResult(item);

        public Task<TItem> SaveItem(TItem item, ILazyLoadingOptions lazyLoadingOptions)
            => Task.FromResult(item);
    }
}
