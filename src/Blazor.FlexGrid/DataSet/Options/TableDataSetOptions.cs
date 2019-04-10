using Blazor.FlexGrid.Components.Events;

namespace Blazor.FlexGrid.DataSet.Options
{
    public class TableDataSetOptions
    {
        public ILazyLoadingOptions LazyLoadingOptions { get; set; } = new LazyLoadingOptions();

        public PageableOptions PageableOptions { get; set; } = new PageableOptions();

        public SortingOptions SortingOptions { get; set; } = new SortingOptions();

        public GroupingOptions GroupingOptions { get; set; } = new GroupingOptions();

        public GridViewEvents GridViewEvents { get; set; }
    }
}
