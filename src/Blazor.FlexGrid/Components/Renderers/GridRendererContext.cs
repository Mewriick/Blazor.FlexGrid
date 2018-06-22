using Blazor.FlexGrid.Components.Configuration.MetaData;
using Blazor.FlexGrid.DataSet;
using Microsoft.AspNetCore.Blazor.RenderTree;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Blazor.FlexGrid.Components.Renderers
{
    public class GridRendererContext
    {
        public int Sequence { get; set; }

        public IEntityType GridConfiguration { get; }

        public IReadOnlyCollection<PropertyInfo> GridItemProperties { get; }

        public RenderTreeBuilder RenderTreeBuilder { get; }

        public ITableDataSet TableDataSet { get; }

        public GridRendererContext(
            IEntityType gridConfiguration,
            List<PropertyInfo> itemProperties,
            RenderTreeBuilder renderTreeBuilder,
            ITableDataSet tableDataSet)
        {
            Sequence = 0;
            GridConfiguration = gridConfiguration ?? throw new ArgumentNullException(nameof(gridConfiguration));
            GridItemProperties = itemProperties ?? throw new ArgumentNullException(nameof(itemProperties));
            RenderTreeBuilder = renderTreeBuilder ?? throw new ArgumentNullException(nameof(renderTreeBuilder));
            TableDataSet = tableDataSet ?? throw new ArgumentNullException(nameof(tableDataSet));
        }
    }
}
