using Blazor.FlexGrid.Components.Configuration;
using Blazor.FlexGrid.Components.Configuration.MetaData;
using Blazor.FlexGrid.Components.Configuration.ValueFormatters;
using Blazor.FlexGrid.Permission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Blazor.FlexGrid.Components.Renderers
{
    public class ImutableGridRendererContext
    {
        private readonly ICurrentUserPermission currentUserPermission;
        private Dictionary<string, IValueFormatter<object>> valueFormatters;
        private Dictionary<string, RenderFragmentAdapter> specialColumnValues;
        private List<PropertyInfo> gridItemCollectionProperties;

        public IEntityType GridEntityConfiguration { get; }

        public IReadOnlyCollection<PropertyInfo> GridItemProperties { get; private set; }

        public ITypePropertyAccessor GetPropertyValueAccessor { get; }

        public IReadOnlyDictionary<string, IValueFormatter<object>> ValueFormatters => valueFormatters;

        public IReadOnlyDictionary<string, RenderFragmentAdapter> SpecialColumnValues => specialColumnValues;

        public GridCssClasses CssClasses { get; }

        public PermissionContext PermissionContext { get; }

        public ImutableGridRendererContext(
            IEntityType gridEntityConfiguration,
            ITypePropertyAccessor propertyValueAccessor,
            ICurrentUserPermission currentUserPermission)
        {
            valueFormatters = new Dictionary<string, IValueFormatter<object>>();
            specialColumnValues = new Dictionary<string, RenderFragmentAdapter>();
            gridItemCollectionProperties = new List<PropertyInfo>();

            GridEntityConfiguration = gridEntityConfiguration ?? throw new ArgumentNullException(nameof(gridEntityConfiguration));
            GetPropertyValueAccessor = propertyValueAccessor ?? throw new ArgumentNullException(nameof(propertyValueAccessor));
            this.currentUserPermission = currentUserPermission ?? throw new ArgumentNullException(nameof(currentUserPermission));

            PermissionContext = new PermissionContext(currentUserPermission, gridEntityConfiguration);
        }

        public void InitializeGridProperties(List<PropertyInfo> itemProperties)
        {
            if (itemProperties is null)
            {
                throw new ArgumentNullException(nameof(itemProperties));
            }

            var collectionProperties = GridEntityConfiguration.ClrTypeCollectionProperties;
            var propertiesListWithOrder = new List<(int Order, PropertyInfo Prop)>();

            foreach (var property in itemProperties)
            {
                var columnConfig = GridEntityConfiguration.FindColumnConfiguration(property.Name);
                PermissionContext.ResolveColumnPermission(columnConfig, property.Name);

                if (columnConfig?.ValueFormatter == null && collectionProperties.Contains(property))
                {
                    continue;
                }

                var columnVisibility = columnConfig?.IsVisible;
                if (columnVisibility.HasValue && !columnVisibility.Value)
                {
                    continue;
                }

                var columnOrder = columnConfig == null ? GridColumnAnotations.DefaultOrder : columnConfig.Order;
                var columnValueFormatter = columnConfig?.ValueFormatter ?? new DefaultValueFormatter();

                propertiesListWithOrder.Add((Order: columnOrder, Prop: property));
                valueFormatters.Add(property.Name, columnValueFormatter);

                if (columnConfig?.SpecialColumnValue != null)
                {
                    specialColumnValues.Add(property.Name, columnConfig.SpecialColumnValue);
                }
            }

            GridItemProperties = propertiesListWithOrder.OrderBy(p => p.Order)
                .Select(p => p.Prop)
                .ToList();
        }
    }
}
