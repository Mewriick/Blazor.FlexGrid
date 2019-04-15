using Blazor.FlexGrid.Components.Configuration;
using System.Collections.Generic;
using System.Reflection;

namespace Blazor.FlexGrid.DataSet.Options
{
    public class GroupingOptions : IGroupingOptions
    {
        public PropertyInfo GroupedProperty { get; private set; }

        public IReadOnlyCollection<PropertyInfo> GroupableProperties { get; private set; }

        public bool IsGroupingActive => GroupedProperty != null;

        public bool IsGroupingEnabled { get; private set; }

        public void DeactivateGrouping()
        {
            GroupedProperty = null;
        }

        public void SetConfiguration(GlobalGroupingOptions globalGroupingOptions, IReadOnlyCollection<PropertyInfo> groupableProperties)
        {
            IsGroupingEnabled = globalGroupingOptions != null
                ? globalGroupingOptions.IsGroupingEnabled
                : false;

            GroupableProperties = groupableProperties;
        }

        public bool SetGroupedProperty(string propertyName)
        {
            foreach (var property in GroupableProperties)
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
}
