using System;

namespace Blazor.FlexGrid.Components.Events
{
    public class SaveResultArgs : EventArgs
    {
        public bool SaveResult { get; set; }

        public object Item { get; set; }
    }
}
