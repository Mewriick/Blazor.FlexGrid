using System;

namespace Blazor.FlexGrid.Components.Renderers.CreateItemForm
{
    public class CreateItemFormViewModel<TItem> : ICreateItemFormViewModel<TItem> where TItem : class
    {
        public TItem Model { get; }

        public CreateItemFormViewModel()
        {
            Model = Activator.CreateInstance(typeof(TItem)) as TItem;
        }

        public bool IsModelValid()
        {
            throw new System.NotImplementedException();
        }

        public bool SaveItem()
        {
            throw new System.NotImplementedException();
        }
    }
}
