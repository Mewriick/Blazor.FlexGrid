using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Blazor.FlexGrid.Validation
{
    public class ValidatableObjectAdapter : IModelValidator
    {
        public IEnumerable<ValidationResult> Validate(object @object)
        {
            var validationResults = new List<System.ComponentModel.DataAnnotations.ValidationResult>();

            if (Validator.TryValidateObject(@object, new ValidationContext(@object), validationResults))
            {
                return validationResults.Select(vr => new ValidationResult(vr.MemberNames.First(), vr.ErrorMessage));
            }

            return new List<ValidationResult>();
        }
    }
}
