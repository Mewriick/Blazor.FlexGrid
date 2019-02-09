namespace Blazor.FlexGrid.Components.Renderers
{
    public interface IActualItemContext
    {
        string ActualColumnName { get; }

        object ActualItem { get; }

        object GetActualItemColumnValue(string columnName);
    }
}
