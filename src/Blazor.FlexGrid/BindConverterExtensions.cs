using Microsoft.AspNetCore.Components;
using System.Globalization;

namespace Blazor.FlexGrid
{
    public static class BindConverterExtensions
    {
        public static T ConvertTo<T>(object obj, T defaultValue)
        {
            if (BindConverter.TryConvertTo<T>(obj, CultureInfo.CurrentCulture, out var convertedValue))
            {
                return convertedValue;
            }

            return defaultValue;
        }
    }
}
