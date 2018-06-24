using System;

namespace Blazor.FlexGrid.Components.Configuration.ValueFormatters
{
    public interface IColumnValueAccessor
    {
        void AppendColumnValueAccessor(Type type);

        object GetColumnValue();
    }
}
