using System;

namespace Blazor.FlexGrid.Components.Configuration.ValueFormatters
{
    public interface IValueFormatter<in TValue>
    {
        ValueFormatterType FormatterType { get; }

        Func<TValue, string> FormatValue { get; }
    }
}
