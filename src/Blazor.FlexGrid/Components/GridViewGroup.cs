using Blazor.FlexGrid.Features;
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
        public GridViewGroup()
            : base(DefaultFeatureCollection.GroupedItemsFeatures)
        {
        }

        protected override Task OnInitializedAsync()
        {
            return base.OnInitializedAsync();
        }

        protected override async Task OnParametersSetAsync()
        {
            GetTableDataSet();
            await tableDataSet.GoToPage(0);
        }
    }
}
