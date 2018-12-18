using Blazor.FlexGrid.DataSet.Options;
using System.Threading.Tasks;

namespace Blazor.FlexGrid.DataSet
{
    public interface ILazyDataSetItemSaver<TItem> where TItem : class
    {
        Task<bool> SaveItem(TItem item, ILazyLoadingOptions lazyLoadingOptions);
    }
}
