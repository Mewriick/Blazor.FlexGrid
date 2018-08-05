using Blazor.FlexGrid.DataAdapters;
using Blazor.FlexGrid.DataSet.Options;
using Microsoft.AspNetCore.Blazor;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Blazor.FlexGrid.DataSet
{
    public class HttpLazyDataSetLoader<TItem> : ILazyDataSetLoader<TItem> where TItem : class
    {
        private readonly HttpClient httpClient;
        private readonly ILogger<HttpLazyDataSetLoader<TItem>> logger;

        public HttpLazyDataSetLoader(HttpClient httpClient, ILogger<HttpLazyDataSetLoader<TItem>> logger)
        {
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Task<LazyLoadingDataSetResult<TItem>> GetTablePageData(
            ILazyLoadingOptions lazyLoadingOptions, IPagingOptions pageableOptions, ISortingOptions sortingOptions)
        {
            if (string.IsNullOrWhiteSpace(lazyLoadingOptions.DataUri))
            {
                throw new ArgumentNullException($"When you using {nameof(LazyLoadedTableDataAdapter<TItem>)} you must specify " +
                    $"{nameof(LazyLoadingOptions.DataUri)} for lazy data retrieving. If you do not want use lazy loading feature use {nameof(CollectionTableDataAdapter<TItem>)} instead.");
            }

            var dataUri = $"{lazyLoadingOptions.DataUri.TrimEnd('/')}?{PagingParams(pageableOptions)}{SortingParams(sortingOptions)}";
            try
            {
                return httpClient.GetJsonAsync<LazyLoadingDataSetResult<TItem>>(dataUri);
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

        private string PagingParams(IPagingOptions pagingOptions)
            => $"pagenumber={pagingOptions.CurrentPage}&pagesize={pagingOptions.PageSize}";

        private string SortingParams(ISortingOptions sortingOptions)
            => string.IsNullOrWhiteSpace(sortingOptions.SortExpression)
                ? string.Empty
                : $"&sortExpression={sortingOptions.SortExpression}&sortDescending={sortingOptions.SortDescending}";
    }
}
