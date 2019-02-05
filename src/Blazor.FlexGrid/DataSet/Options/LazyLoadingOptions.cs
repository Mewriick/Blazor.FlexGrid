using Microsoft.AspNetCore.Http.Extensions;

namespace Blazor.FlexGrid.DataSet.Options
{
    public class LazyLoadingOptions : ILazyLoadingOptions
    {
        public string DataUri { get; set; }

        public string PutDataUri { get; set; }

        public string DeleteUri { get; set; }

        public QueryBuilder RequestParams { get; } = new QueryBuilder();
    }
}
