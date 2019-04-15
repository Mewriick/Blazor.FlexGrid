using Blazor.FlexGrid.Components.Configuration;
using System.Collections.Generic;
using System.Reflection;

namespace Blazor.FlexGrid.DataSet.Options
{
    public interface IGroupingOptions
    {
        PropertyInfo GroupedProperty { get; }

        IReadOnlyCollection<PropertyInfo> GroupableProperties { get; }

        bool IsGroupingActive { get; }

        bool IsGroupingEnabled { get; }

        bool SetGroupedProperty(string propertyName);

        void DeactivateGrouping();

        void SetConfiguration(GlobalGroupingOptions globalGroupingOptions, IReadOnlyCollection<PropertyInfo> groupableProperties);
    }
}
