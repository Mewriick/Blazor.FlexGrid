namespace Blazor.FlexGrid.DataSet.Options
{
    public interface ILazyLoadingOptions
    {
        string DataUri { get; set; }

        string PutDataUri { get; set; }

        string DeleteUri { get; set; }

        LazyRequestParams RequestParams { get; }
    }
}
