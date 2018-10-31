namespace Blazor.FlexGrid.DataSet.Options
{
    public class LazyLoadingOptions : ILazyLoadingOptions
    {
        public string DataUri { get; set; }

        public LazyRequestParams RequestParams { get; } = new LazyRequestParams();
    }
}
