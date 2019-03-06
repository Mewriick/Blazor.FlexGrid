using Blazor.FlexGrid.Validation;
using System;
using System.Collections.Generic;

namespace Blazor.FlexGrid.Components.Renderers.CreateItemForm
{
    public class CreateItemFormViewModel<TItem> : ICreateItemFormViewModel<TItem> where TItem : class
    {
        private readonly IModelValidator _modelValidator;

        public TItem Model { get; }

        public IEnumerable<ValidationResult> ValidationResults { get; private set; }

        public CreateItemFormViewModel(IModelValidator modelValidator)
        {
            _modelValidator = modelValidator ?? throw new ArgumentNullException(nameof(modelValidator));
            Model = Activator.CreateInstance(typeof(TItem)) as TItem;
        }

        public void ValidateModel()
        {
            ValidationResults = _modelValidator.Validate(Model);
        }

        public bool SaveItem()
        {
            throw new System.NotImplementedException();
        }
    }
}
