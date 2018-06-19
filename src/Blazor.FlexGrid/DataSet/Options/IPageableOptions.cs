namespace Blazor.FlexGrid.DataSet.Options
{
    public interface IPageableOptions
    {
        int PageSize { get; set; }

        int TotalItemsCount { get; set; }

        int CurrentPage { get; set; }

        bool IsFirstPage { get; }

        bool IsLastPage { get; }
    }
}
