using System.Collections.Generic;

namespace Blazor.FlexGrid.DataSet.Http.Dto
{
    public class GroupedItemsDto<TItem>
    {
        public Dictionary<object, IEnumerable<TItem>> Items { get; set; }

        public int TotalCount { get; set; }
    }
}
