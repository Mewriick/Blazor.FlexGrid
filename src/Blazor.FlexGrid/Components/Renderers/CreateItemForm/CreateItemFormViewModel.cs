using Blazor.FlexGrid.Components.Configuration;
using Microsoft.AspNetCore.Components.Forms;
using System;

namespace Blazor.FlexGrid.Components.Renderers.CreateItemForm
{
    public class CreateItemFormViewModel<TModel> : ICreateItemFormViewModel<TModel> where TModel : class
    {
        private readonly CreateItemOptions createItemOptions;

        public TModel Model { get; }

        public EditContext EditContext { get; }

        public bool IsModelValid { get; private set; }

        public Action<TModel> SaveAction { get; set; }

        public CreateItemFormViewModel(CreateItemOptions createItemOptions)
        {
            this.createItemOptions = createItemOptions ?? throw new ArgumentNullException(nameof(createItemOptions));
            Model = Activator.CreateInstance(typeof(TModel)) as TModel;
            EditContext = new EditContext(Model);
            EditContext.OnFieldChanged += OnFieldChanged;
        }

        public void ValidateModel()
            => IsModelValid = EditContext.Validate();

        private void OnFieldChanged(object sender, FieldChangedEventArgs e)
        {
            ValidateModel();
        }
    }
}
