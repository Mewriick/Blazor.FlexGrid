using System;

namespace Blazor.FlexGrid.DataSet.Options
{
    public interface IPagingOptions
    {
        Action<int> PageChanged { get; set; }

        int PageSize { get; set; }

        int TotalItemsCount { get; set; }

        int PagesCount { get; }

        int CurrentPage { get; set; }

        bool IsFirstPage { get; }

        bool IsLastPage { get; }
    }
}
