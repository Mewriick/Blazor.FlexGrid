using Blazor.FlexGrid.Components.Configuration;
using System.Threading;
using System.Threading.Tasks;

namespace Blazor.FlexGrid.DataSet.Http
{
    public interface ICreateItemHandle<in TModel, TOutputDto>
        where TModel : class
        where TOutputDto : class
    {
        Task<TOutputDto> CreateItem(TModel model, CreateItemOptions createItemOptions, CancellationToken cancellationToken = default);
    }
}
