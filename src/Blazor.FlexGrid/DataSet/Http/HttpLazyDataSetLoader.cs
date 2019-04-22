using Blazor.FlexGrid.DataSet.Http;
using Blazor.FlexGrid.Filters;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Blazor.FlexGrid.DataSet
{
    public class HttpLazyDataSetLoader<TItem> : ILazyDataSetLoader<TItem> where TItem : class
    {
        private readonly HttpClient httpClient;
        private readonly ILogger<HttpLazyDataSetLoader<TItem>> logger;

        public HttpLazyDataSetLoader(IHttpClientFactory httpClientFactory, ILogger<HttpLazyDataSetLoader<TItem>> logger)
        {
            this.httpClient = httpClientFactory?.Create() ?? throw new ArgumentNullException(nameof(httpClientFactory));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Task<LazyLoadingDataSetResult<TItem>> GetTablePageData(
            RequestOptions requestOptions,
            IReadOnlyCollection<IFilterDefinition> filterDefinitions = null)
        {
            var dataUri = requestOptions.BuildUrl();
            try
            {
                if (filterDefinitions != null && filterDefinitions.Any())
                {
                    return httpClient.PostJsonAsync<LazyLoadingDataSetResult<TItem>>(dataUri, filterDefinitions);
                }
                else
                {
                    return httpClient.GetJsonAsync<LazyLoadingDataSetResult<TItem>>(dataUri);
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"Error during fetching data from [{dataUri}]. Ex: {ex}");

                var emptyResult = new LazyLoadingDataSetResult<TItem>
                {
                    Items = Enumerable.Empty<TItem>().ToList()
                };

                return Task.FromResult(emptyResult);
            }
        }
    }
}
