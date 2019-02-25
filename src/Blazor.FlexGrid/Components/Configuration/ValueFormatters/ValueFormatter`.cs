using System;
using System.Linq.Expressions;

namespace Blazor.FlexGrid.Components.Configuration.ValueFormatters
{
    public class ValueFormatter<TInputValue> : ValueFormatter
    {
        public override Func<object, string> FormatValue { get; }

        public ValueFormatter(Expression<Func<TInputValue, string>> formatValueExpression, ValueFormatterType valueFormatterType = ValueFormatterType.SingleProperty, string defaultValue = default)
            : base(valueFormatterType)
        {
            FormatValue = SanitizeConverter(formatValueExpression, defaultValue);
        }

        private Func<object, string> SanitizeConverter(Expression<Func<TInputValue, string>> formatValueExpression, string defaultValue)
        {
            var compiled = formatValueExpression.Compile();

            return v => v == null
                ? defaultValue ?? string.Empty
                : compiled((TInputValue)v);
        }
    }
}
