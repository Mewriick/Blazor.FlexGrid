using System;
using System.Linq.Expressions;

namespace Blazor.FlexGrid.Components.Configuration.ValueFormatters
{
    public class ValueFormatter<TInputValue> : IValueFormatter<TInputValue>
    {
        public Func<TInputValue, string> FormatValue { get; }

        public ValueFormatterType FormatterType { get; }

        public ValueFormatter(Expression<Func<TInputValue, string>> formatValueExpression, ValueFormatterType valueFormatterType = ValueFormatterType.SingleProperty, string defaultValue = default)
        {
            FormatterType = valueFormatterType;
            FormatValue = SanitizeConverter(formatValueExpression, defaultValue);
        }

        private Func<TInputValue, string> SanitizeConverter(Expression<Func<TInputValue, string>> formatValueExpression, string defaultValue)
        {
            var compiled = formatValueExpression.Compile();

            return v => v == null
                ? defaultValue ?? string.Empty
                : compiled(v);
        }
    }
}
