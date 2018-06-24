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
        public IEntityType GridConfiguration { get; }

        public IReadOnlyCollection<PropertyInfo> GridItemProperties { get; }

        public IPropertyValueAccessor GetPropertyValueAccessor { get; }

        public IReadOnlyDictionary<string, ValueFormatter> ValueFormatters { get; }

        public ImutableGridRendererContext(
            IEntityType gridConfiguration,
            List<PropertyInfo> itemProperties,
            IPropertyValueAccessor getPropertyValueAccessor)
        {
            GridConfiguration = gridConfiguration ?? throw new ArgumentNullException(nameof(gridConfiguration));
            GridItemProperties = itemProperties ?? throw new ArgumentNullException(nameof(itemProperties));
            GetPropertyValueAccessor = getPropertyValueAccessor ?? throw new ArgumentNullException(nameof(getPropertyValueAccessor));
            ValueFormatters = InitializeValueFormatters();
        }

        private IReadOnlyDictionary<string, ValueFormatter> InitializeValueFormatters()
        {
            var dictionary = new Dictionary<string, ValueFormatter>();

            GridItemProperties
               .Select(p => p.Name)
               .ToList()
               .ForEach(columnName =>
               {
                   var columnAnnotation = GridConfiguration.FindColumnConfiguration(columnName);
                   ValueFormatter columnValueFormatter = new DefaultValueFormatter();
                   if (columnAnnotation != null)
                   {
                       columnValueFormatter = columnAnnotation.ValueFormatter;
                   }

                   dictionary.Add(columnName, columnValueFormatter);
               });

            return dictionary;
        }
    }
}
