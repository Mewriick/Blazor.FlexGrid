using System;
using System.Linq.Expressions;

namespace Blazor.FlexGrid.Components.Configuration.ValueFormatters
{
    /// <summary>
    /// Represent base class for formatting value which is displayed in grid
    /// </summary>
    public abstract class ValueFormatter
    {
        public abstract Func<object, string> FormatValue { get; }

        public virtual LambdaExpression FormatValueExpression { get; }

        public ValueFormatter(LambdaExpression formatValueExpression)
        {
            FormatValueExpression = formatValueExpression ?? throw new ArgumentNullException(nameof(formatValueExpression));
        }
    }
}
