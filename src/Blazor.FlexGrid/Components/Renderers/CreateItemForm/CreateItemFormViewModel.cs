using Microsoft.AspNetCore.Components.Forms;
using System;

namespace Blazor.FlexGrid.Components.Renderers.CreateItemForm
{
    public class CreateItemFormViewModel<TItem> : ICreateItemFormViewModel<TItem> where TItem : class
    {
        public TItem Model { get; }

        public EditContext EditContext { get; }

        public CreateItemFormViewModel()
        {
            Model = Activator.CreateInstance(typeof(TItem)) as TItem;
            EditContext = new EditContext(Model);
            EditContext.OnFieldChanged += OnFieldChanged;
        }

        public void ValidateModel()
            => EditContext.Validate();

        public void SaveItem()
        {
            Console.WriteLine(Model.ToString());
        }

        private void OnFieldChanged(object sender, FieldChangedEventArgs e)
        {
            ValidateModel();
        }
    }
}
