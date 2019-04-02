using Blazor.FlexGrid.Components.Events;
using Blazor.FlexGrid.Components.Renderers;
using System;

namespace Blazor.FlexGrid.DataSet
{

    /// <summary>
    /// Represents a collection of Items with paging, sorting and inline editation
    /// </summary>
    public interface ITableDataSet : IPageableTableDataSet, ISortableTableDataSet, ISelectableDataSet, IRowEditableDataSet, IGroupableTableDataSet
    {
        GridViewEvents GridViewEvents { get; }

        


    }
}
