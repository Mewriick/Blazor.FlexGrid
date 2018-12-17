using Blazor.FlexGrid.Components.Configuration;
using Blazor.FlexGrid.Components.Configuration.ValueFormatters;
using Blazor.FlexGrid.DataSet;
using Blazor.FlexGrid.Permission;
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
        private readonly ICurrentUserPermission currentUserPermission;

        public GridRendererContextFactory(
            IGridConfigurationProvider gridConfigurationProvider,
            IPropertyValueAccessorCache propertyValueAccessorCache,
            ICurrentUserPermission currentUserPermission)
        {
            this.gridConfigurationProvider = gridConfigurationProvider ?? throw new ArgumentNullException(nameof(gridConfigurationProvider));
            this.propertyValueAccessorCache = propertyValueAccessorCache ?? throw new ArgumentNullException(nameof(propertyValueAccessorCache));
            this.currentUserPermission = currentUserPermission ?? throw new ArgumentNullException(nameof(currentUserPermission));
            this.imutableRendererContextCache = new Dictionary<Type, ImutableGridRendererContext>();
        }

        public GridRendererContext CreateRendererContext(ITableDataSet tableDataSet, RenderTreeBuilder renderTreeBuilder)
            => new GridRendererContext(
                GetImutableGridRendererContext(tableDataSet.UnderlyingTypeOfItem()),
                renderTreeBuilder,
                tableDataSet);

        private ImutableGridRendererContext GetImutableGridRendererContext(Type dataSetItemType)
        {
            if (imutableRendererContextCache.TryGetValue(dataSetItemType, out var imutableGridRendererContext))
            {
                return imutableGridRendererContext;
            }

            var gridConfiguration = gridConfigurationProvider.FindGridEntityConfigurationByType(dataSetItemType);
            propertyValueAccessorCache.AddPropertyAccessor(dataSetItemType, new TypeWrapper(dataSetItemType));

            imutableGridRendererContext = new ImutableGridRendererContext(
                    gridConfiguration,
                    dataSetItemType.GetProperties().ToList(),
                    propertyValueAccessorCache.GetPropertyAccesor(dataSetItemType),
                    currentUserPermission
                );

            imutableRendererContextCache.Add(dataSetItemType, imutableGridRendererContext);

            return imutableGridRendererContext;
        }
    }
}
