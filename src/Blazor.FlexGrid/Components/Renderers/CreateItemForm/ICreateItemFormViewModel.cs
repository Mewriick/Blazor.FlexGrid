namespace Blazor.FlexGrid.Components.Renderers.CreateItemForm
{
    public interface ICreateItemFormViewModel<TItem> where TItem : class
    {
        TItem Model { get; }

        bool SaveItem();
    }
}
