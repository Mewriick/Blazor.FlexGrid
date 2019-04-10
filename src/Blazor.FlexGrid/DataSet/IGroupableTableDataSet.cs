using Blazor.FlexGrid.DataSet.Options;
using System.Collections.Generic;

namespace Blazor.FlexGrid.DataSet
{
    public interface IGroupableTableDataSet
    {
        IGroupingOptions GroupingOptions { get; }

        IList<GroupItem> GroupedItems { get; }

        void ToggleGroupRow(object groupItemKey);
    }
}
