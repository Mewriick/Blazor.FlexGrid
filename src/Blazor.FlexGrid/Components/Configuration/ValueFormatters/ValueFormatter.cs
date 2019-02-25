using System;

namespace Blazor.FlexGrid.Components.Configuration.ValueFormatters
{
    /// <summary>
    /// Represent base class for formatting value which is displayed in grid
    /// </summary>
    public abstract class ValueFormatter
    {
        public ValueFormatterType FormatterType { get; }

        public abstract Func<object, string> FormatValue { get; }

        public ValueFormatter(ValueFormatterType valueFormatterType = ValueFormatterType.SingleProperty)
        {
            FormatterType = valueFormatterType;
        }
    }
}
