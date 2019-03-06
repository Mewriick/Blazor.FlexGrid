using Blazor.FlexGrid.Validation;
using System;

namespace Blazor.FlexGrid.Components.Renderers.CreateItemForm
{
    public class CreateItemFormViewModel<TItem> : ICreateItemFormViewModel<TItem> where TItem : class
    {
        private readonly IModelValidator _modelValidator;

        public TItem Model { get; }

        public CreateItemFormViewModel(IModelValidator modelValidator)
        {
            _modelValidator = modelValidator ?? throw new ArgumentNullException(nameof(modelValidator));
            Model = Activator.CreateInstance(typeof(TItem)) as TItem;
        }

        public bool IsModelValid()
        {
            throw new System.NotImplementedException();
        }

        public bool SaveItem()
        {
            throw new System.NotImplementedException();
        }
    }
}
