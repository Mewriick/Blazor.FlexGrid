using Blazor.FlexGrid.DataSet.Options;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blazor.FlexGrid.DataSet
{
    //public interface IGroupableTableDataSet<TItem>: IGroupableTableDataSet
    //{                
    //    IEnumerable<IGrouping<object, TItem>> GroupedItems { get; }
        
    //}

    public interface IGroupableTableDataSet
    {
        IGroupingOptions GroupingOptions { get;  }

        IEnumerable<GroupItem> GroupedItems { get; set;  }


        void ToggleGroupRow(object groupItemKey);


    }
}
