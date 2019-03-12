using Blazor.FlexGrid.Components.Configuration;
using Blazor.FlexGrid.Components.Events;
using System;

namespace Blazor.FlexGrid.Components
{
    public sealed class CreateItemContext
    {
        public event EventHandler<ItemCreatedArgs> OnItemCreated;

        public CreateItemOptions CreateItemOptions { get; }

        public CreateItemContext(CreateItemOptions createItemOptions)
        {
            CreateItemOptions = createItemOptions ?? throw new ArgumentNullException(nameof(createItemOptions));
        }

        public void NotifyItemCreated(object item)
        {
            OnItemCreated?.Invoke(this, new ItemCreatedArgs(item));
        }
    }
}
