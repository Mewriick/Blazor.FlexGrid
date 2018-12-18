namespace Blazor.FlexGrid.DataSet.Options
{
    public interface ILazyLoadingOptions
    {
        string DataUri { get; set; }

        string PutDataUri { get; set; }

        LazyRequestParams RequestParams { get; }
    }
}
