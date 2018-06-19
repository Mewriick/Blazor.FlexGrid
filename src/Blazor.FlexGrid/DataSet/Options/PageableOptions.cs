using System;

namespace Blazor.FlexGrid.DataSet.Options
{
    public class PageableOptions : IPageableOptions
    {
        public int PageSize { get; set; }

        public int TotalItemsCount { get; set; }

        public int CurrentPage { get; set; }

        public bool IsFirstPage => throw new NotImplementedException();

        public bool IsLastPage => throw new NotImplementedException();
    }
}
