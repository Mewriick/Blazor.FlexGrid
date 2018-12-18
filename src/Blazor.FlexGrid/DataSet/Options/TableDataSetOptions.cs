namespace Blazor.FlexGrid.DataSet.Options
{
    public class TableDataSetOptions
    {
        public ILazyLoadingOptions LazyLoadingOptions { get; set; } = new LazyLoadingOptions();

        public PageableOptions PageableOptions { get; set; } = new PageableOptions();

        public SortingOptions SortingOptions { get; set; } = new SortingOptions();
    }
}
