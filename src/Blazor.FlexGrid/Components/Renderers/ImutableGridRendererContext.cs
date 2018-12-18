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
        private Dictionary<string, ValueFormatter> valueFormatters;
        private Dictionary<string, RenderFragmentAdapter> specialColumnValues;
        private Dictionary<string, PermissionAccess> columnPermissions;
        private List<PropertyInfo> gridItemCollectionProperties;

        public IEntityType GridEntityConfiguration { get; }

        public IReadOnlyCollection<PropertyInfo> GridItemProperties { get; private set; }

        public IPropertyValueAccessor GetPropertyValueAccessor { get; }

        public IReadOnlyDictionary<string, ValueFormatter> ValueFormatters => valueFormatters;

        public IReadOnlyDictionary<string, RenderFragmentAdapter> SpecialColumnValues => specialColumnValues;

        public IReadOnlyDictionary<string, PermissionAccess> ColumnReadPermmisions => columnPermissions;

        public GridCssClasses CssClasses { get; }

        public ImutableGridRendererContext(
            IEntityType gridEntityConfiguration,
            IPropertyValueAccessor propertyValueAccessor,
            ICurrentUserPermission currentUserPermission)
        {
            valueFormatters = new Dictionary<string, ValueFormatter>();
            specialColumnValues = new Dictionary<string, RenderFragmentAdapter>();
            columnPermissions = new Dictionary<string, PermissionAccess>();
            gridItemCollectionProperties = new List<PropertyInfo>();

            GridEntityConfiguration = gridEntityConfiguration ?? throw new ArgumentNullException(nameof(gridEntityConfiguration));
            GetPropertyValueAccessor = propertyValueAccessor ?? throw new ArgumentNullException(nameof(propertyValueAccessor));
            this.currentUserPermission = currentUserPermission ?? throw new ArgumentNullException(nameof(currentUserPermission));
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
                if (collectionProperties.Contains(property))
                {
                    continue;
                }

                var columnConfig = GridEntityConfiguration.FindColumnConfiguration(property.Name);
                ResolveColumnPermission(columnConfig, property.Name);

                var columnVisibility = columnConfig?.IsVisible;
                if (columnVisibility.HasValue && !columnVisibility.Value)
                {
                    continue;
                }

                var columnOrder = columnConfig == null ? GridColumnAnotations.DefaultOrder : columnConfig.Order;
                ValueFormatter columnValueFormatter = columnConfig?.ValueFormatter ?? new DefaultValueFormatter();

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

        private void ResolveColumnPermission(IGridViewColumnAnotations columnConfig, string columnName)
        {
            var permissionAccess = PermissionAccess.None;
            if (columnConfig is null)
            {
                permissionAccess |= PermissionAccess.Read | PermissionAccess.Write;
            }
            else
            {
                permissionAccess |= columnConfig.ReadPermissionRestrictionFunc(currentUserPermission)
                   ? PermissionAccess.Read
                   : PermissionAccess.None;

                permissionAccess |= columnConfig.WritePermissionRestrictionFunc(currentUserPermission)
                   ? PermissionAccess.Write
                   : PermissionAccess.None;
            }

            columnPermissions.Add(columnName, permissionAccess);
        }
    }
}
