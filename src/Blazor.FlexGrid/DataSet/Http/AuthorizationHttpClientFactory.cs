using Blazor.FlexGrid.Permission;
using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Blazor.FlexGrid.DataSet.Http
{
    public class AuthorizationHttpClientFactory : IHttpClientFactory
    {
        private readonly HttpClient httpClient;
        private readonly IAuthorizationService authorizationService;

        public AuthorizationHttpClientFactory(HttpClient httpClient, IAuthorizationService authorizationService)
        {
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            this.authorizationService = authorizationService ?? throw new ArgumentNullException(nameof(authorizationService));
        }

        public HttpClient Create()
        {
            if (httpClient.DefaultRequestHeaders.Authorization is null)
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorizationService.UserToken);
            }

            return httpClient;
        }
    }
}
