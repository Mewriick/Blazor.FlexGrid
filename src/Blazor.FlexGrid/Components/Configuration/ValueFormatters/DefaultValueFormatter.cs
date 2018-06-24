using System;
using System.Linq.Expressions;

namespace Blazor.FlexGrid.Components.Configuration.ValueFormatters
{
    public class DefaultValueFormatter : ValueFormatter
    {
        public override Func<object, string> FormatValue { get; }


        public DefaultValueFormatter()
            : this(v => v == null ? string.Empty : v.ToString())
        {

        }

        public DefaultValueFormatter(Expression<Func<object, string>> formatValueExpression)
            : base(formatValueExpression)
        {
            FormatValue = formatValueExpression.Compile();
        }
    }
}
