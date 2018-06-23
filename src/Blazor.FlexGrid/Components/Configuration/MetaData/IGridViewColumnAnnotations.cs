using Blazor.FlexGrid.Components.Configuration.ValueFormatters;

namespace Blazor.FlexGrid.Components.Configuration.MetaData
{
    /// <summary>
    ///     Properties for grid column annotations accessed through
    /// </summary>
    public interface IGridViewColumnAnnotations
    {
        string Caption { get; }

        int Order { get; }

        bool IsVisible { get; }

        ValueFormatter ValueFormatter { get; }
    }
}
