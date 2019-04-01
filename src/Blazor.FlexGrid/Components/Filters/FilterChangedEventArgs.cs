using Blazor.FlexGrid.Filters;
using System;
using System.Collections.Generic;

namespace Blazor.FlexGrid.Components.Filters
{
    public sealed class FilterChangedEventArgs
    {
        public IReadOnlyCollection<IFilterDefinition> Filters { get; }

        public FilterChangedEventArgs(List<IFilterDefinition> filters)
        {
            Filters = filters ?? throw new ArgumentNullException(nameof(filters));
        }
    }
}
