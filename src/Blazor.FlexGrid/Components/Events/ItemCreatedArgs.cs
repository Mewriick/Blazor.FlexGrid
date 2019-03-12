using System;

namespace Blazor.FlexGrid.Components.Events
{
    public sealed class ItemCreatedArgs
    {
        public object CreatedItem { get; }

        public ItemCreatedArgs(object createdItem)
        {
            CreatedItem = createdItem ?? throw new ArgumentNullException(nameof(createdItem));
        }
    }
}
