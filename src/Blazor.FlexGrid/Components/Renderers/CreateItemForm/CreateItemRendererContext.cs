using Blazor.FlexGrid.Components.Configuration.ValueFormatters;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Blazor.FlexGrid.Components.Renderers.CreateItemForm
{
    public class CreateItemRendererContext<TModel> : IActualItemContext<TModel> where TModel : class
    {
        private readonly ITypePropertyAccessor typePropertyAccessor;

        public string ActualColumnName { get; set; }

        public TModel ActualItem => ViewModel.Model;

        public ICreateItemFormViewModel<TModel> ViewModel { get; }

        public CreateItemRendererContext(ICreateItemFormViewModel<TModel> createItemFormViewModel, ITypePropertyAccessorCache typePropertyAccessorCache)
        {
            this.typePropertyAccessor = typePropertyAccessorCache?.GetPropertyAccesor(typeof(TModel))
                ?? throw new ArgumentNullException(nameof(typePropertyAccessorCache));

            this.ViewModel = createItemFormViewModel ?? throw new ArgumentNullException(nameof(createItemFormViewModel));
        }

        public object GetActualItemColumnValue(string columnName)
            => typePropertyAccessor.GetValue(ViewModel.Model, columnName);

        public void SetActualItemColumnValue(string columnName, object value)
            => typePropertyAccessor.SetValue(ViewModel.Model, columnName, value);

        public IEnumerable<PropertyInfo> GetModelFields()
            => typePropertyAccessor.Properties;
    }
}
