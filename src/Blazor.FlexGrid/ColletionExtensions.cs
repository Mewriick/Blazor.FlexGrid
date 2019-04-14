using Blazor.FlexGrid.DataSet;
using System.Collections.Generic;
using System.Linq;

namespace Blazor.FlexGrid
{
    public static class ColletionExtensions
    {
        private static readonly GroupingKeyEqualityComparer GroupingKeyEqualityComparer = new GroupingKeyEqualityComparer();

        public static void PreserveGroupCollapsing(this IList<GroupItem> newGroupedItems, IList<GroupItem> oldGroupedItems)
        {
            if (oldGroupedItems == null)
            {
                return;
            }

            foreach (var group in newGroupedItems)
            {
                var oldGroup = oldGroupedItems.FirstOrDefault(g => GroupingKeyEqualityComparer.Equals(g.Key, group.Key));
                if (oldGroup != null)
                {
                    group.IsCollapsed = oldGroup.IsCollapsed;
                }
            }
        }

        public static void ToggleGroup(this IList<GroupItem> groupItems, object groupKey)
        {
            var group = groupItems.FirstOrDefault(g => GroupingKeyEqualityComparer.Equals(g.Key, groupKey));
            if (group != null)
            {
                group.IsCollapsed = !group.IsCollapsed;
            }
        }
    }
}
