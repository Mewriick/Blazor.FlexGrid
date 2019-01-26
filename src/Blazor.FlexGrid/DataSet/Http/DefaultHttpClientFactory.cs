using System;
using System.Net.Http;

namespace Blazor.FlexGrid.DataSet.Http
{
    public class DefaultHttpClientFactory : IHttpClientFactory
    {
        private readonly HttpClient httpClient;

        public DefaultHttpClientFactory(HttpClient httpClient)
        {
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public HttpClient Create()
            => httpClient;
    }
}
