using System;
using System.Linq.Expressions;

namespace Blazor.FlexGrid.Components.Configuration.ValueFormatters
{
    /// <summary>
    /// Represent base class for formatting value which is displayed in grid
    /// </summary>
    public abstract class ValueFormatter
    {
        public ValueFormatterType FormatterType { get; }

        public abstract Func<object, string> FormatValue { get; }

        protected virtual LambdaExpression FormatValueExpression { get; }

        public ValueFormatter(LambdaExpression formatValueExpression, ValueFormatterType valueFormatterType = ValueFormatterType.SingleProperty)
        {
            FormatterType = valueFormatterType;
            FormatValueExpression = formatValueExpression ?? throw new ArgumentNullException(nameof(formatValueExpression));
        }
    }
}
