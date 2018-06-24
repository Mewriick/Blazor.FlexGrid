using Blazor.FlexGrid.DataSet.Options;
using Microsoft.AspNetCore.Blazor;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Blazor.FlexGrid.DataSet
{
    public class HttpLazyDataSetLoader<TItem> : ILazyDataSetLoader<TItem> where TItem : class
    {
        private readonly HttpClient httpClient;

        public HttpLazyDataSetLoader(HttpClient httpClient)
        {
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public Task<LazyLoadingDataSetResult<TItem>> GetTablePageData(ILazyLoadingOptions lazyLoadingOptions, IPageableOptions pageableOptions)
        {
            return httpClient.GetJsonAsync<LazyLoadingDataSetResult<TItem>>(lazyLoadingOptions.DataUri);
        }
    }
}
