using Blazor.FlexGrid.Components.Configuration;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Blazor.FlexGrid.DataSet.Http
{
    public class HttpCreateItemHandler<TModel, TOutputDto> : ICreateItemHandle<TModel, TOutputDto>
        where TModel : class
        where TOutputDto : class
    {
        private readonly HttpClient httpClient;
        private readonly ILogger<HttpCreateItemHandler<TModel, TOutputDto>> logger;

        public HttpCreateItemHandler(
            Http.IHttpClientFactory httpClientFactory,
            ILogger<HttpCreateItemHandler<TModel, TOutputDto>> logger)
        {
            this.httpClient = httpClientFactory?.Create() ?? throw new ArgumentNullException(nameof(httpClientFactory));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<TOutputDto> CreateItem(TModel model, CreateItemOptions createItemOptions, CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await httpClient.PostJsonAsync<TOutputDto>(createItemOptions.CreateUri, model);

                return response;
            }
            catch (Exception ex)
            {
                logger.LogError($"Error during creating item for [{createItemOptions.CreateUri}]. Ex: {ex}");

                throw;
            }
        }
    }
}
