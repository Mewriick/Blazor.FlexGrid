using Blazor.FlexGrid.DataSet;
using Blazor.FlexGrid.DataSet.Options;
using System.Threading.Tasks;

namespace Blazor.FlexGrid.DataAdapters
{
    public class NullLazyDataSetItemSaver<TItem> : ILazyDataSetItemSaver<TItem> where TItem : class
    {
        public static NullLazyDataSetItemSaver<TItem> Instance = new NullLazyDataSetItemSaver<TItem>();

        public Task<bool> SaveItem(TItem item, ILazyLoadingOptions lazyLoadingOptions)
            => Task.FromResult(true);
    }
}
