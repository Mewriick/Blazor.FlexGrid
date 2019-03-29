using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Blazor.FlexGrid.DataSet.Options
{
    public interface IGroupingOptions
    {
        PropertyInfo GroupedProperty { get; set; }

        IList<PropertyInfo> GroupableProperties { get; set; }

        bool IsGroupingActive { get;  }

        bool SetGroupedProperty(string propertyName);

        void DisableGrouping();


    }
}
