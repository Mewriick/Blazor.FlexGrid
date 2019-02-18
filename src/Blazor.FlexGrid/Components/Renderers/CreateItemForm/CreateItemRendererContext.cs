using Blazor.FlexGrid.Components.Configuration.ValueFormatters;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Blazor.FlexGrid.Components.Renderers.CreateItemForm
{
    public class CreateItemRendererContext<TItem> : IActualItemContext<TItem> where TItem : class
    {
        private readonly ITypePropertyAccessor typePropertyAccessor;

        public string ActualColumnName { get; set; }

        public TItem ActualItem => ViewModel.Model;

        public ICreateItemFormViewModel<TItem> ViewModel { get; }

        public CreateItemRendererContext(ICreateItemFormViewModel<TItem> createItemFormViewModel, ITypePropertyAccessorCache typePropertyAccessorCache)
        {
            this.typePropertyAccessor = typePropertyAccessorCache?.GetPropertyAccesor(typeof(TItem))
                ?? throw new ArgumentNullException(nameof(typePropertyAccessorCache));

            this.ViewModel = createItemFormViewModel ?? throw new ArgumentNullException(nameof(createItemFormViewModel));
        }

        public object GetActualItemColumnValue(string columnName)
            => typePropertyAccessor.GetValue(ViewModel.Model, columnName);

        public void SetActulItemColumnValue(string columnName, object value)
            => typePropertyAccessor.SetValue(ViewModel.Model, columnName, value);

        public IEnumerable<PropertyInfo> GetModelFields()
            => typePropertyAccessor.Properties;
    }
}
