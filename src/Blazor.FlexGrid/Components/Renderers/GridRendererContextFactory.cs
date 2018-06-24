using Blazor.FlexGrid.Components.Configuration;
using Blazor.FlexGrid.Components.Configuration.MetaData;
using Blazor.FlexGrid.Components.Configuration.ValueFormatters;
using Blazor.FlexGrid.DataSet;
using Microsoft.AspNetCore.Blazor.RenderTree;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Blazor.FlexGrid.Components.Renderers
{
    public class GridRendererContextFactory
    {
        private readonly Dictionary<Type, ImutableGridRendererContext> imutableRendererContextCache;
        private readonly IGridConfigurationProvider gridConfigurationProvider;

        public GridRendererContextFactory(IGridConfigurationProvider gridConfigurationProvider)
        {
            this.gridConfigurationProvider = gridConfigurationProvider ?? throw new ArgumentNullException(nameof(gridConfigurationProvider));
            this.imutableRendererContextCache = new Dictionary<Type, ImutableGridRendererContext>();
        }

        public GridRendererContext CreateRendererContext(ITableDataSet tableDataSet, RenderTreeBuilder renderTreeBuilder)
        {
            var itemType = tableDataSet.GetType().GenericTypeArguments[0];
            var rendererContext = new GridRendererContext(
                GetImutableGridRendererContext(itemType),
                renderTreeBuilder,
                tableDataSet
            );

            return rendererContext;
        }

        private ImutableGridRendererContext GetImutableGridRendererContext(Type dataSetItemType)
        {
            if (imutableRendererContextCache.TryGetValue(dataSetItemType, out var imutableGridRendererContext))
            {
                return imutableGridRendererContext;
            }

            imutableGridRendererContext = new ImutableGridRendererContext(
                    gridConfigurationProvider.FindGridConfigurationByType(dataSetItemType) ?? NullEntityType.Instance,
                    dataSetItemType.GetProperties().ToList(),
                    new TypeWrapper(dataSetItemType)
                );

            imutableRendererContextCache.Add(dataSetItemType, imutableGridRendererContext);


            return imutableGridRendererContext;
        }
    }
}
