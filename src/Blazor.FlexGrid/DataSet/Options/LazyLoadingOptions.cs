namespace Blazor.FlexGrid.DataSet.Options
{
    public class LazyLoadingOptions : ILazyLoadingOptions
    {
        public string DataUri { get; set; }

        public string PutDataUri { get; set; }

        public string DeleteUri { get; set; }

        public LazyRequestParams RequestParams { get; } = new LazyRequestParams();
    }
}
