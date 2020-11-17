using System;

namespace Blazor.FlexGrid.DataSet.Options
{
    public class PageableOptions : IPagingOptions
    {
        private int currentPage = 0;

        public Action<int> PageChanged { get; set; }

        public int PageSize { get; set; } = 5;

        public int TotalItemsCount { get; set; }

        public int CurrentPage
        {
            get { return currentPage; }
            set
            {
                currentPage = value;
                PageChanged?.Invoke(currentPage);
            }
        }

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
