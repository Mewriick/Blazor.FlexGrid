using Blazor.FlexGrid.DataSet;
using System.Collections.Generic;
using System.Linq;

namespace Blazor.FlexGrid
{
    public static class ColletionExtensions
    {
        public static void PreserveCollapse(this IList<GroupItem> newGroupedItems, IList<GroupItem> oldGroupedItems)
        {
            if (oldGroupedItems == null)
            {
                return;
            }

            foreach (var group in newGroupedItems)
            {
                var oldGroup = oldGroupedItems.FirstOrDefault(g => g.Key == group.Key);
                if (oldGroup != null)
                {
                    group.IsCollapsed = oldGroup.IsCollapsed;
                }
            }
        }
    }
}
