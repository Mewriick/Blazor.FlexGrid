using System;
using System.Linq.Expressions;

namespace Blazor.FlexGrid.Components.Configuration.ValueFormatters
{
    public class ValueFormatter<TProperty> : ValueFormatter
    {
        public override Func<object, string> FormatValue { get; }

        public new virtual Expression<Func<TProperty, string>> FormatValueExpression
            => (Expression<Func<TProperty, string>>)base.FormatValueExpression;


        public ValueFormatter(Expression<Func<TProperty, string>> formatValueExpression)
            : base(formatValueExpression)
        {
            FormatValue = SanitizeConverter(formatValueExpression);
        }

        private Func<object, string> SanitizeConverter(Expression<Func<TProperty, string>> formatValueExpression)
        {
            var compiled = formatValueExpression.Compile();

            return v => v == null
                ? string.Empty
                : compiled((TProperty)v);
        }
    }
}
