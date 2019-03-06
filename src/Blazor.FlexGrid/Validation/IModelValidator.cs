using System.Collections.Generic;

namespace Blazor.FlexGrid.Validation
{
    public interface IModelValidator
    {
        IEnumerable<ValidationResult> Validate(object @object);
    }
}
