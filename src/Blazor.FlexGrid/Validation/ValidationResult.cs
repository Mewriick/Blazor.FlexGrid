namespace Blazor.FlexGrid.Validation
{
    public class ValidationResult
    {
        /// <summary>
        /// Initializes a new instance of <see cref="ModelValidationResult"/>.
        /// </summary>
        /// <param name="memberName">The name of the entry on which validation was performed.</param>
        /// <param name="message">The validation message.</param>
        public ValidationResult(string memberName, string message)
        {
            MemberName = memberName ?? string.Empty;
            Message = message ?? string.Empty;
        }

        /// <summary>
        /// Gets the name of the entry on which validation was performed.
        /// </summary>
        public string MemberName { get; }

        /// <summary>
        /// Gets the validation message.
        /// </summary>
        public string Message { get; }
    }
}
