using System;

namespace Blazor.FlexGrid.Components.Events
{
    public class GridViewEvents
    {
        public Action<SaveResultArgs> SaveOperationFinished { get; set; }

        public Action<DeleteResultArgs> DeleteOperationFinished { get; set; }
    }
}
