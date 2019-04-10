using System;
using System.Collections.Generic;
using System.Text;

namespace Blazor.FlexGrid.Components.Configuration
{
    public class GlobalGroupingOptions
    {
        public bool IsGroupingEnabled { get; set; }

    }

    public class NullGlobalGroupingOptions: GlobalGroupingOptions
    {
        public NullGlobalGroupingOptions()
        {
            IsGroupingEnabled = false;
        }
    }

}
