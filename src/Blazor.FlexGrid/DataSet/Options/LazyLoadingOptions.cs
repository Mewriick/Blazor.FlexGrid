using Microsoft.AspNetCore.Http.Extensions;

namespace Blazor.FlexGrid.DataSet.Options
{
    public class LazyLoadingOptions : ILazyLoadingOptions
    {
        public string DataUri { get; set; } = string.Empty;

        public string PutDataUri { get; set; } = string.Empty;

        public string DeleteUri { get; set; } = string.Empty;

        public QueryBuilder RequestParams { get; private set; } = new QueryBuilder();

        public void Copy(ILazyLoadingOptions lazyLoadingOptions)
        {
            DataUri = lazyLoadingOptions.DataUri;
            PutDataUri = lazyLoadingOptions.PutDataUri;
            DeleteUri = lazyLoadingOptions.DeleteUri;
            RequestParams = lazyLoadingOptions.RequestParams;
        }
    }
}
