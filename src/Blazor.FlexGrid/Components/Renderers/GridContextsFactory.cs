using Blazor.FlexGrid.Components.Configuration;
using Blazor.FlexGrid.Components.Configuration.ValueFormatters;
using Blazor.FlexGrid.DataSet;
using Blazor.FlexGrid.Permission;
using Microsoft.AspNetCore.Components.RenderTree;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Blazor.FlexGrid.Components.Renderers
{
    public class GridContextsFactory
    {
        private readonly Dictionary<Type, ImutableGridRendererContext> imutableRendererContextCache;
        private readonly ITypePropertyAccessorCache propertyValueAccessorCache;
        private readonly ICurrentUserPermission currentUserPermission;
        private readonly ILogger<GridContextsFactory> logger;

        public IGridConfigurationProvider GridConfigurationProvider { get; }

        public GridContextsFactory(
            IGridConfigurationProvider gridConfigurationProvider,
            ITypePropertyAccessorCache propertyValueAccessorCache,
            ICurrentUserPermission currentUserPermission,
            ILogger<GridContextsFactory> logger)
        {
            this.GridConfigurationProvider = gridConfigurationProvider ?? throw new ArgumentNullException(nameof(gridConfigurationProvider));
            this.propertyValueAccessorCache = propertyValueAccessorCache ?? throw new ArgumentNullException(nameof(propertyValueAccessorCache));
            this.currentUserPermission = currentUserPermission ?? throw new ArgumentNullException(nameof(currentUserPermission));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.imutableRendererContextCache = new Dictionary<Type, ImutableGridRendererContext>();
        }

        public (GridRendererContext RendererContext, PermissionContext PermissionContext)
            CreateContexts(ITableDataSet tableDataSet, RenderTreeBuilder renderTreeBuilder)
        {
            var imutableRendererContext = GetImutableGridRendererContext(tableDataSet.UnderlyingTypeOfItem());

            return (new GridRendererContext(imutableRendererContext, new BlazorRendererTreeBuilder(renderTreeBuilder), tableDataSet),
                    imutableRendererContext.PermissionContext);
        }

        private ImutableGridRendererContext GetImutableGridRendererContext(Type dataSetItemType)
        {
            if (imutableRendererContextCache.TryGetValue(dataSetItemType, out var imutableGridRendererContext))
            {
                return imutableGridRendererContext;
            }

            var gridConfiguration = GridConfigurationProvider.FindGridEntityConfigurationByType(dataSetItemType);
            propertyValueAccessorCache.AddPropertyAccessor(dataSetItemType, new TypeWrapper(dataSetItemType, logger));

            imutableGridRendererContext = new ImutableGridRendererContext(
                    gridConfiguration,
                    propertyValueAccessorCache.GetPropertyAccesor(dataSetItemType),
                    currentUserPermission
                );

            imutableGridRendererContext.InitializeGridProperties(dataSetItemType.GetProperties().ToList());
            imutableRendererContextCache.Add(dataSetItemType, imutableGridRendererContext);

            return imutableGridRendererContext;
        }
    }
}
