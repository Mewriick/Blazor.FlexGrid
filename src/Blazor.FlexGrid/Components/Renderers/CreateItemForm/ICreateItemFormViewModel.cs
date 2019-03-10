using Microsoft.AspNetCore.Components.Forms;

namespace Blazor.FlexGrid.Components.Renderers.CreateItemForm
{
    public interface ICreateItemFormViewModel<TItem> where TItem : class
    {
        TItem Model { get; }

        EditContext EditContext { get; }

        void SaveItem();

        void ValidateModel();
    }
}
