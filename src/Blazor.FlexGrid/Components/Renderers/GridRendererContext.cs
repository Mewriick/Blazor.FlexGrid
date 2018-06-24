using Blazor.FlexGrid.Components.Configuration.MetaData;
using Blazor.FlexGrid.Components.Configuration.ValueFormatters;
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

        public string ActualColumnName { get; set; }

        public object ActualItem { get; set; }

        public IEntityType GridConfiguration { get; }

        public IReadOnlyCollection<PropertyInfo> GridItemProperties { get; }

        public RenderTreeBuilder RenderTreeBuilder { get; }

        public ITableDataSet TableDataSet { get; }

        public IPropertyValueAccessor GetPropertyValueAccessor { get; }

        public IReadOnlyDictionary<string, ValueFormatter> ValueFormatters { get; }


        public GridRendererContext(
            ImutableGridRendererContext imutableGridRendererContext,
            RenderTreeBuilder renderTreeBuilder,
            ITableDataSet tableDataSet)
        {
            Sequence = 0;
            GridConfiguration = imutableGridRendererContext.GridConfiguration;
            GridItemProperties = imutableGridRendererContext.GridItemProperties;
            GetPropertyValueAccessor = imutableGridRendererContext.GetPropertyValueAccessor;
            ValueFormatters = imutableGridRendererContext.ValueFormatters;
            RenderTreeBuilder = renderTreeBuilder ?? throw new ArgumentNullException(nameof(renderTreeBuilder));
            TableDataSet = tableDataSet ?? throw new ArgumentNullException(nameof(tableDataSet));
        }
    }
}
