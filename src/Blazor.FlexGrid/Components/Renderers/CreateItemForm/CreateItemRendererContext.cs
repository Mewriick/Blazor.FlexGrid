using Blazor.FlexGrid.Components.Configuration.ValueFormatters;
using System;

namespace Blazor.FlexGrid.Components.Renderers.CreateItemForm
{
    public class CreateItemRendererContext<TItem> : IActualItemContext<TItem> where TItem : class
    {
        private readonly IPropertyValueAccessor propertyValueAccessor;
        private readonly ICreateItemFormViewModel<TItem> createItemForm;

        public string ActualColumnName { get; set; }

        public object ActualItem => createItemForm.Model;

        TItem IActualItemContext<TItem>.ActualItem => createItemForm.Model;

        public CreateItemRendererContext(ICreateItemFormViewModel<TItem> createItemForm, IPropertyValueAccessorCache propertyValueAccessorCache)
        {
            this.propertyValueAccessor = propertyValueAccessorCache?.GetPropertyAccesor(typeof(TItem))
                ?? throw new ArgumentNullException(nameof(propertyValueAccessorCache));

            this.createItemForm = createItemForm ?? throw new ArgumentNullException(nameof(createItemForm));
        }

        public object GetActualItemColumnValue(string columnName)
            => propertyValueAccessor.GetValue(createItemForm.Model, columnName);

        public void SetActulItemColumnValue(string columnName, object value)
            => propertyValueAccessor.SetValue(createItemForm.Model, columnName, value);
    }
}
