using System.Net.Http;

namespace Blazor.FlexGrid.DataSet.Http
{
    public interface IHttpClientFactory
    {
        HttpClient Create();
    }
}
