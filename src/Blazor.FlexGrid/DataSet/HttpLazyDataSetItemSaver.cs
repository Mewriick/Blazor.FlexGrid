using Blazor.FlexGrid.DataSet.Options;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Blazor.FlexGrid.DataSet
{
    public class HttpLazyDataSetItemSaver<TItem> : ILazyDataSetItemSaver<TItem> where TItem : class
    {
        private readonly HttpClient httpClient;
        private readonly ILogger<HttpLazyDataSetItemSaver<TItem>> logger;

        public HttpLazyDataSetItemSaver(HttpClient httpClient, ILogger<HttpLazyDataSetItemSaver<TItem>> logger)
        {
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<bool> SaveItem(TItem item, ILazyLoadingOptions lazyLoadingOptions)
        {
            if (string.IsNullOrWhiteSpace(lazyLoadingOptions.PutDataUri))
            {
                throw new ArgumentNullException($"When you are using {nameof(LazyTableDataSet<TItem>)} you must specify url for saving updated item data");
            }

            try
            {
                var json = Microsoft.JSInterop.Json.Serialize(item);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await httpClient.PutAsync(lazyLoadingOptions.PutDataUri, content);

                return true;
            }
            catch (Exception ex)
            {
                logger.LogError($"Error during saving data for [{lazyLoadingOptions.PutDataUri}]. Ex: {ex}");

                return false;
            }
        }
    }
}
