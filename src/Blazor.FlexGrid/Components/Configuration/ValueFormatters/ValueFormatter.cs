using System;

namespace Blazor.FlexGrid.Components.Configuration.ValueFormatters
{
    public interface IValueFormatter
    {
        ValueFormatterType FormatterType { get; }

        Func<object, string> FormatValue { get; }
    }


    public interface IValueFormatter<in TValue> : IValueFormatter
    {
    }
}
