using Blazor.FlexGrid.DataAdapters;
using Blazor.FlexGrid.DataSet.Http;
using Blazor.FlexGrid.DataSet.Options;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http.Extensions;
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

        public HttpLazyDataSetLoader(IHttpClientFactory httpClientFactory, ILogger<HttpLazyDataSetLoader<TItem>> logger)
        {
            this.httpClient = httpClientFactory?.Create() ?? throw new ArgumentNullException(nameof(httpClientFactory));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }


        public Task<LazyLoadingDataSetResult<TItem>> GetTablePageData(
            ILazyLoadingOptions lazyLoadingOptions, IPagingOptions pageableOptions, 
            ISortingOptions sortingOptions)
        {
            if (string.IsNullOrWhiteSpace(lazyLoadingOptions.DataUri))
            {
                throw new ArgumentNullException($"When you using {nameof(LazyLoadedTableDataAdapter<TItem>)} you must specify " +
                    $"{nameof(LazyLoadingOptions.DataUri)} for lazy data retrieving. If you do not want use lazy loading feature use {nameof(CollectionTableDataAdapter<TItem>)} instead.");
            }

            var query = new QueryBuilder(lazyLoadingOptions.RequestParams);
            PagingParams(query, pageableOptions);
            SortingParams(query, sortingOptions);
            
            var dataUri = $"{lazyLoadingOptions.DataUri}{query.ToString()}";
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

        public Task<LazyLoadingDataSetResult<GroupItem<TItem>>> GetGroupedTablePageData(
            ILazyLoadingOptions lazyLoadingOptions, IPagingOptions pageableOptions, 
            ISortingOptions sortingOptions, IGroupingOptions groupingOptions)
        {
            throw new NotImplementedException();
        }



        private void PagingParams(QueryBuilder builder, IPagingOptions pagingOptions)
        {
            builder.Add("pagenumber", pagingOptions.CurrentPage.ToString());
            builder.Add("pagesize", pagingOptions.PageSize.ToString());
        }

        private void SortingParams(QueryBuilder builder, ISortingOptions sortingOptions)
        {
            if (!string.IsNullOrWhiteSpace(sortingOptions.SortExpression))
            {
                builder.Add("sortExpression", sortingOptions.SortExpression);
                builder.Add("sortDescending", sortingOptions.SortDescending.ToString());
            }
        }
    }
}
