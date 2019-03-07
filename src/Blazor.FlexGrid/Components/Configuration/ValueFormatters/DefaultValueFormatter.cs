using System;
using System.Linq.Expressions;

namespace Blazor.FlexGrid.Components.Configuration.ValueFormatters
{
    public class DefaultValueFormatter : IValueFormatter<object>
    {
        public Func<object, string> FormatValue { get; }

        public ValueFormatterType FormatterType => ValueFormatterType.SingleProperty;

        public DefaultValueFormatter()
            : this(v => v == null ? string.Empty : v.ToString())
        {
        }

        public DefaultValueFormatter(Expression<Func<object, string>> formatValueExpression)
        {
            FormatValue = formatValueExpression.Compile();
        }
    }
}
