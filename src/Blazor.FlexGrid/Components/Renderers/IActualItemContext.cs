namespace Blazor.FlexGrid.Components.Renderers
{
    public interface IActualItemContext<out TItem> where TItem : class
    {
        string ActualColumnName { get; }

        TItem ActualItem { get; }

        object GetActualItemColumnValue(string columnName);

        void SetActualItemColumnValue(string columnName, object value);
    }
}
