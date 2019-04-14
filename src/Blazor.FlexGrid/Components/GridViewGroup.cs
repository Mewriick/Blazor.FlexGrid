using Blazor.FlexGrid.Components.Filters;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Blazor.FlexGrid.Components
{
    /// <summary>
    /// Workaround GridViewComponent for rendering table for items of group 
    /// </summary>
    /// <typeparam name="T"></typeparam>

    [Route("/gridviewgroup")]
    internal class GridViewGroup<T> : GridViewInternal
    {
        protected override FlexGridContext CreateFlexGridContext()
            => new FlexGridContext(new FilterContext()) { IsTableForItemsGroup = true };

        protected override Task OnInitAsync()
        {
            return base.OnInitAsync();
        }

        protected override async Task OnParametersSetAsync()
        {
            tableDataSet = GetTableDataSet();
            await tableDataSet.GoToPage(0);
        }
    }
}
