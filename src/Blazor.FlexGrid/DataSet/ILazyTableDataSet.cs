using Blazor.FlexGrid.DataSet.Options;

namespace Blazor.FlexGrid.DataSet
{
    /// <summary>
    /// Represents a collection of Items with lazy loading pagination
    /// </summary>
    interface ILazyTableDataSet : ITableDataSet
    {
        ILazyLoadingOptions LazyLoadingOptions { get; set; }
    }
}
