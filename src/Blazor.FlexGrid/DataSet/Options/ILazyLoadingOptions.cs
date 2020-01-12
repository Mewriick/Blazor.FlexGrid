using Microsoft.AspNetCore.Http.Extensions;

namespace Blazor.FlexGrid.DataSet.Options
{
    public interface ILazyLoadingOptions
    {
        string DataUri { get; set; }

        string PutDataUri { get; set; }

        string DeleteUri { get; set; }

        QueryBuilder RequestParams { get; }

        void Copy(ILazyLoadingOptions lazyLoadingOptions);
    }
}
