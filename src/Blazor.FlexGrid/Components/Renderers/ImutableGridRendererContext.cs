using Blazor.FlexGrid.Components.Configuration.MetaData;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Blazor.FlexGrid.Components.Renderers
{
    public class ImutableGridRendererContext
    {
        public IEntityType GridConfiguration { get; }

        public IReadOnlyCollection<PropertyInfo> GridItemProperties { get; }

        public ImutableGridRendererContext(
            IEntityType gridConfiguration,
            List<PropertyInfo> itemProperties)
        {
            GridConfiguration = gridConfiguration ?? throw new ArgumentNullException(nameof(gridConfiguration));
            GridItemProperties = itemProperties ?? throw new ArgumentNullException(nameof(itemProperties));
        }
    }
}
