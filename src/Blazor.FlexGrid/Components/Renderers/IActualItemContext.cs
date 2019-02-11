namespace Blazor.FlexGrid.Components.Renderers
{
    public interface IActualItemContext
    {
        string ActualColumnName { get; }

        object ActualItem { get; }

        object GetActualItemColumnValue(string columnName);
    }

    public interface IActualItemContext<TItem> : IActualItemContext where TItem : class
    {
        new TItem ActualItem { get; }
    }
}
