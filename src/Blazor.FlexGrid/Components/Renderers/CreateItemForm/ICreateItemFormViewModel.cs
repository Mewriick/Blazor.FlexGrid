using Microsoft.AspNetCore.Components.Forms;
using System;

namespace Blazor.FlexGrid.Components.Renderers.CreateItemForm
{
    public interface ICreateItemFormViewModel<TModel>
        where TModel : class
    {
        bool IsModelValid { get; }

        TModel Model { get; }

        EditContext EditContext { get; }

        Action<TModel> SaveAction { get; set; }

        void ValidateModel();

        void ClearModel();
    }
}
