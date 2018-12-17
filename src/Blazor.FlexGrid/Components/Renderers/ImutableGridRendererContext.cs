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
        private Dictionary<string, bool> columnPermissions;
        private List<PropertyInfo> gridItemCollectionProperties;

        public IEntityType GridEntityConfiguration { get; }

        public IReadOnlyCollection<PropertyInfo> GridItemProperties { get; private set; }

        public IPropertyValueAccessor GetPropertyValueAccessor { get; }

        public IReadOnlyDictionary<string, ValueFormatter> ValueFormatters => valueFormatters;

        public IReadOnlyDictionary<string, RenderFragmentAdapter> SpecialColumnValues => specialColumnValues;

        public IReadOnlyDictionary<string, bool> ColumnReadPermmisions => columnPermissions;

        public GridCssClasses CssClasses { get; }

        public ImutableGridRendererContext(
            IEntityType gridEntityConfiguration,
            List<PropertyInfo> itemProperties,
            IPropertyValueAccessor propertyValueAccessor,
            ICurrentUserPermission currentUserPermission)
        {
            valueFormatters = new Dictionary<string, ValueFormatter>();
            specialColumnValues = new Dictionary<string, RenderFragmentAdapter>();
            columnPermissions = new Dictionary<string, bool>();
            gridItemCollectionProperties = new List<PropertyInfo>();

            GridEntityConfiguration = gridEntityConfiguration ?? throw new ArgumentNullException(nameof(gridEntityConfiguration));
            GridItemProperties = itemProperties ?? throw new ArgumentNullException(nameof(itemProperties));
            GetPropertyValueAccessor = propertyValueAccessor ?? throw new ArgumentNullException(nameof(propertyValueAccessor));
            this.currentUserPermission = currentUserPermission ?? throw new ArgumentNullException(nameof(currentUserPermission));
            InitializeGridProperties();
        }

        private void InitializeGridProperties()
        {
            var collectionProperties = GridEntityConfiguration.ClrTypeCollectionProperties;
            var propertiesListWithOrder = new List<(int Order, PropertyInfo Prop)>();

            foreach (var property in GridItemProperties)
            {
                if (collectionProperties.Contains(property))
                {
                    continue;
                }

                var columnConfig = GridEntityConfiguration.FindColumnConfiguration(property.Name);
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

                columnPermissions.Add(property.Name, columnConfig?.ReadPermissionRestrictionFunc(currentUserPermission) ?? true);
            }

            GridItemProperties = propertiesListWithOrder.OrderBy(p => p.Order)
                .Select(p => p.Prop)
                .ToList();
        }
    }
}
