using System;
using System.Linq.Expressions;

namespace Blazor.FlexGrid.Components.Configuration.ValueFormatters
{
    public class ValueFormatter<TInputValue> : ValueFormatter
    {
        public override Func<object, string> FormatValue { get; }

        protected new virtual Expression<Func<TInputValue, string>> FormatValueExpression
            => (Expression<Func<TInputValue, string>>)base.FormatValueExpression;


        public ValueFormatter(Expression<Func<TInputValue, string>> formatValueExpression, ValueFormatterType valueFormatterType = ValueFormatterType.SingleProperty, bool allowNull = false)
            : base(formatValueExpression, valueFormatterType)
        {
            FormatValue = SanitizeConverter(formatValueExpression, allowNull);
        }

        private Func<object, string> SanitizeConverter(Expression<Func<TInputValue, string>> formatValueExpression, bool allowNull)
        {
            var compiled = formatValueExpression.Compile();

            if (allowNull)
            {
                return v => compiled((TInputValue)v);
            }

            return v => v == null
                ? string.Empty
                : compiled((TInputValue)v);
        }
    }
}
