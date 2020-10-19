using System;

namespace Blazor.FlexGrid.Filters
{
    public interface IFilterDefinition
    {
        string ColumnName { get; }

        object Value { get; }

        FilterOperation FilterOperation { get; }

        StringComparison TextComparasion { get; }
    }
}
