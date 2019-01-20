using Blazor.FlexGrid.DataSet.Options;
using System.Threading.Tasks;

namespace Blazor.FlexGrid.DataSet
{
    public interface ILazyDataSetItemManipulator<TItem> where TItem : class
    {
        Task<TItem> SaveItem(TItem item, ILazyLoadingOptions lazyLoadingOptions);

        Task<TItem> DeleteItem(TItem item, ILazyLoadingOptions lazyLoadingOptions);
    }
}
