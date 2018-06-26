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

        public IEntityType GridConfiguration { get; }

        public IReadOnlyCollection<PropertyInfo> GridItemProperties { get; private set; }

        public IPropertyValueAccessor GetPropertyValueAccessor { get; }

        public IReadOnlyDictionary<string, ValueFormatter> ValueFormatters => valueFormatters;

        public ImutableGridRendererContext(
            IEntityType gridConfiguration,
            List<PropertyInfo> itemProperties,
            IPropertyValueAccessor getPropertyValueAccessor)
        {
            valueFormatters = new Dictionary<string, ValueFormatter>();
            GridConfiguration = gridConfiguration ?? throw new ArgumentNullException(nameof(gridConfiguration));
            GridItemProperties = itemProperties ?? throw new ArgumentNullException(nameof(itemProperties));
            GetPropertyValueAccessor = getPropertyValueAccessor ?? throw new ArgumentNullException(nameof(getPropertyValueAccessor));
            InitializeGridProperties();
        }

        private void InitializeGridProperties()
        {
            var propertiesListWithOrder = new List<(int Order, PropertyInfo Prop)>();
            foreach (var property in GridItemProperties)
            {
                ValueFormatter columnValueFormatter = new DefaultValueFormatter();
                var columnOrder = GridColumnAnnotations.DefaultOrder;
                var columnConfig = GridConfiguration.FindColumnConfiguration(property.Name);
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
