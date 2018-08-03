using Blazor.FlexGrid.Components.Configuration;
using Blazor.FlexGrid.Components.Configuration.MetaData;
using Blazor.FlexGrid.Components.Configuration.ValueFormatters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Blazor.FlexGrid.Components.Renderers
{
    public class ImutableGridRendererContext
    {
        private Dictionary<string, ValueFormatter> valueFormatters;
        private List<PropertyInfo> gridItemCollectionProperties;

        public IEntityType GridEntityConfiguration { get; }

        public IReadOnlyCollection<PropertyInfo> GridItemProperties { get; private set; }

        public IPropertyValueAccessor GetPropertyValueAccessor { get; }

        public IReadOnlyDictionary<string, ValueFormatter> ValueFormatters => valueFormatters;

        public GridCssClasses CssClasses { get; }

        public ImutableGridRendererContext(
            IEntityType gridEntityConfiguration,
            List<PropertyInfo> itemProperties,
            IPropertyValueAccessor propertyValueAccessor
            )
        {
            valueFormatters = new Dictionary<string, ValueFormatter>();
            gridItemCollectionProperties = new List<PropertyInfo>();
            GridEntityConfiguration = gridEntityConfiguration ?? throw new ArgumentNullException(nameof(gridEntityConfiguration));
            GridItemProperties = itemProperties ?? throw new ArgumentNullException(nameof(itemProperties));
            GetPropertyValueAccessor = propertyValueAccessor ?? throw new ArgumentNullException(nameof(propertyValueAccessor));
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

                ValueFormatter columnValueFormatter = new DefaultValueFormatter();
                var columnOrder = GridColumnAnotations.DefaultOrder;
                var columnConfig = GridEntityConfiguration.FindColumnConfiguration(property.Name);
                if (columnConfig != null)
                {
                    columnOrder = columnConfig.Order;
                    columnValueFormatter = columnConfig.ValueFormatter;
                }

                propertiesListWithOrder.Add((Order: columnOrder, Prop: property));
                valueFormatters.Add(property.Name, columnValueFormatter);
            }

            GridItemProperties = propertiesListWithOrder.OrderBy(p => p.Order)
                .Select(p => p.Prop)
                .ToList();
        }
    }
}
