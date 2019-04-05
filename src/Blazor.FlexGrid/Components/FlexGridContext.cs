using Blazor.FlexGrid.Components.Filters;
using System;

namespace Blazor.FlexGrid.Components
{
    public class FlexGridContext
    {
        public FilterContext FilterContext { get; }

        public Action RequestRerenderTableRowsNotification { get; private set; }

        public FlexGridContext(FilterContext filterContext)
        {
            FilterContext = filterContext ?? throw new ArgumentNullException(nameof(filterContext));
        }

        public void SetRequestRendererNotification(Action requestRendererNotification)
        {
            if (requestRendererNotification is null)
            {
                return;
            }

            RequestRerenderTableRowsNotification = requestRendererNotification ?? throw new ArgumentNullException(nameof(requestRendererNotification));
        }
    }
}
