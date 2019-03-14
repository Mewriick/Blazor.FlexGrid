using Blazor.FlexGrid.Components.Configuration;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Blazor.FlexGrid.DataSet.Http
{
    public class NullCreateItemHandler<TModel, TOutputDto> : ICreateItemHandle<TModel, TOutputDto>
        where TModel : class
        where TOutputDto : class
    {
        public Task<TOutputDto> CreateItem(TModel model, CreateItemOptions createItemOptions, CancellationToken cancellationToken = default)
            => Task.FromResult(Activator.CreateInstance<TOutputDto>());

    }
}
