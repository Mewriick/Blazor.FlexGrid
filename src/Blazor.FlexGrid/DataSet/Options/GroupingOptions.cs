using Blazor.FlexGrid.Components.Configuration;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Blazor.FlexGrid.DataSet.Options
{
    public class GroupingOptions : GlobalGroupingOptions, IGroupingOptions
    {
        //public IList<PropertyInfo> GroupedProperties { get; set; }
        public PropertyInfo GroupedProperty { get; set; }

        public IList<PropertyInfo> GroupableProperties { get; set; }

        public bool IsGroupingActive => GroupedProperty != null;


        public void DeactivateGrouping()
        {
            GroupedProperty = null;
        }

        public void SetConfiguration(GlobalGroupingOptions globalGroupingOptions)
        {
            this.IsGroupingEnabled = globalGroupingOptions != null 
                ? globalGroupingOptions.IsGroupingEnabled
                : false;
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
