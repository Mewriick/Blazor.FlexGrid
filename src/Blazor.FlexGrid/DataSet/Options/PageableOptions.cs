using System;

namespace Blazor.FlexGrid.DataSet.Options
{
    public class PageableOptions : IPagingOptions
    {
        public int PageSize { get; set; }

        public int TotalItemsCount { get; set; }

        public int CurrentPage { get; set; }

        public bool IsFirstPage => CurrentPage == 0;

        public bool IsLastPage => CurrentPage == PagesCount - 1;

        public int PagesCount
        {
            get
            {
                if (TotalItemsCount == 0 || PageSize == 0)
                {
                    return 1;
                }
                return (int)Math.Ceiling((double)TotalItemsCount / PageSize);
            }
        }
    }
}
