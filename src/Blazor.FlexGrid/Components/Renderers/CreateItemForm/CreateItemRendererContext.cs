using Blazor.FlexGrid.Components.Configuration.ValueFormatters;
using System;

namespace Blazor.FlexGrid.Components.Renderers.CreateItemForm
{
    public class CreateItemRendererContext<TItem> : IActualItemContext<TItem> where TItem : class
    {
        private readonly IPropertyValueAccessor propertyValueAccessor;
        private TItem model;

        public string ActualColumnName => throw new NotImplementedException();

        public object ActualItem => throw new NotImplementedException();

        TItem IActualItemContext<TItem>.ActualItem => throw new NotImplementedException();

        public CreateItemRendererContext(IPropertyValueAccessorCache propertyValueAccessorCache)
        {
            this.propertyValueAccessor = propertyValueAccessorCache?.GetPropertyAccesor(typeof(TItem))
                ?? throw new ArgumentNullException(nameof(propertyValueAccessorCache));

            model = Activator.CreateInstance(typeof(TItem)) as TItem;
        }

        public object GetActualItemColumnValue(string columnName)
            => propertyValueAccessor.GetValue(model, columnName);
    }
}
