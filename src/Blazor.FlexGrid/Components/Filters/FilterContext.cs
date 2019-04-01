using Blazor.FlexGrid.Filters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Blazor.FlexGrid.Components.Filters
{
    public sealed class FilterContext
    {
        private readonly List<IFilterDefinition> filterDefinitions = new List<IFilterDefinition>();

        public event EventHandler<FilterChangedEventArgs> OnFilterChanged;

        public void ToggleFilterDefinition(IFilterDefinition filterDefinition)
        {
            var definition = filterDefinitions.FirstOrDefault(fd => fd.ColumnName == filterDefinition.ColumnName);
            if (definition is null)
            {
                filterDefinitions.Add(filterDefinition);
            }
            else
            {
                filterDefinitions.Remove(definition);
            }

            OnFilterChanged?.Invoke(this, new FilterChangedEventArgs(filterDefinitions));
        }
    }
}
