using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Blazor.FlexGrid.DataSet.Options
{
    public class GroupingOptions : IGroupingOptions
    {
        //public IList<PropertyInfo> GroupedProperties { get; set; }
        public PropertyInfo GroupedProperty { get; set; }

        public IList<PropertyInfo> GroupableProperties { get; set; }

        public bool IsGroupingActive => GroupedProperty != null;

        public bool IsGroupingEnabled { get; set; } = true;

        public void DeactivateGrouping()
        {
            GroupedProperty = null;
        }

        public bool SetGroupedProperty(string propertyName)
        {
            foreach(var property in GroupableProperties)
            {
                if (property.Name == propertyName)
                {
                    GroupedProperty = property;
                    return true;
                }
            }

            return false;
        }
    }

    public class NullGroupingOptions: GroupingOptions
    {

    }
}
