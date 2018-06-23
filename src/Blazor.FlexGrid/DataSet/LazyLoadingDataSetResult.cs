using System.Collections.Generic;

namespace Blazor.FlexGrid.DataSet
{
    public class LazyLoadingDataSetResult<TItem> where TItem : class
    {
        public int TotalCount { get; set; }

        public IList<TItem> Items { get; set; }
    }
}
