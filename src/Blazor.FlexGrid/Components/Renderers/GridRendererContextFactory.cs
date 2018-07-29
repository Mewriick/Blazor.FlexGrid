using Blazor.FlexGrid.Components.Configuration;
using Blazor.FlexGrid.Components.Configuration.MetaData;
using Blazor.FlexGrid.Components.Configuration.MetaData.Conventions;
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
        private readonly IPropertyValueAccessorCache propertyValueAccessorCache;
        private readonly ConventionsSet conventionsSet;

        public GridRendererContextFactory(
            IGridConfigurationProvider gridConfigurationProvider,
            IPropertyValueAccessorCache propertyValueAccessorCache,
            ConventionsSet conventionsSet)
        {
            this.gridConfigurationProvider = gridConfigurationProvider ?? throw new ArgumentNullException(nameof(gridConfigurationProvider));
            this.propertyValueAccessorCache = propertyValueAccessorCache ?? throw new ArgumentNullException(nameof(propertyValueAccessorCache));
            this.conventionsSet = conventionsSet ?? throw new ArgumentNullException(nameof(conventionsSet));
            this.imutableRendererContextCache = new Dictionary<Type, ImutableGridRendererContext>();
        }

        public GridRendererContext CreateRendererContext(ITableDataSet tableDataSet, RenderTreeBuilder renderTreeBuilder)
        {
            var itemType = tableDataSet.GetType().GenericTypeArguments[0];
            //conventionsSet.RunConventions(itemType);

            var gridConfiguration = gridConfigurationProvider.FindGridEntityConfigurationByType(itemType);

            var rendererContext = new GridRendererContext(
                GetImutableGridRendererContext(itemType, gridConfiguration),
                renderTreeBuilder,
                tableDataSet);

            return rendererContext;
        }

        private ImutableGridRendererContext GetImutableGridRendererContext(Type dataSetItemType, IEntityType gridConfiguration)
        {
            if (imutableRendererContextCache.TryGetValue(dataSetItemType, out var imutableGridRendererContext))
            {
                return imutableGridRendererContext;
            }

            propertyValueAccessorCache.AddPropertyAccessor(dataSetItemType, new TypeWrapper(dataSetItemType));
            imutableGridRendererContext = new ImutableGridRendererContext(
                    gridConfiguration,
                    dataSetItemType.GetProperties().ToList(),
                    propertyValueAccessorCache.GetPropertyAccesor(dataSetItemType)
                );

            imutableRendererContextCache.Add(dataSetItemType, imutableGridRendererContext);

            return imutableGridRendererContext;
        }
    }
}
