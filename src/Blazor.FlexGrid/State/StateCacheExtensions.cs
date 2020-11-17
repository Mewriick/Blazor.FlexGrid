using Blazor.FlexGrid.Components.Configuration;
using Blazor.FlexGrid.Components.Filters;
using Blazor.FlexGrid.Components.Renderers;
using Blazor.FlexGrid.Filters;
using System.Collections.Generic;

namespace Blazor.FlexGrid.State
{
    internal static class StateCacheExtensions
    {
        internal static List<ExpressionFilterDefinition> GetFilterDefinitions(
            this IStateCache stateCache,
            ImutableGridRendererContext imutableGridRendererContext)
        {
            var definitions = new List<ExpressionFilterDefinition>();
            foreach (var property in imutableGridRendererContext.GridItemProperties)
            {
                var config = imutableGridRendererContext.GridEntityConfiguration.FindColumnConfiguration(property.Name);
                if (stateCache.TryGetStateValue<ColumnFilterState>(property.Name, out var state))
                {
                    definitions.Add(new ExpressionFilterDefinition(
                        property.Name,
                        state.FilterOperation,
                        state.FilterValue,
                        config.TextComparison));
                }
            }

            return definitions;
        }
    }
}
