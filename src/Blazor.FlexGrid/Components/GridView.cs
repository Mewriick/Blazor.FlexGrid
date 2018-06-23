using Blazor.FlexGrid.Components.Renderers;
using Blazor.FlexGrid.DataAdapters;
using Blazor.FlexGrid.DataSet;
using Blazor.FlexGrid.DataSet.Options;
using Microsoft.AspNetCore.Blazor.Components;
using Microsoft.AspNetCore.Blazor.RenderTree;
using System.Threading.Tasks;

namespace Blazor.FlexGrid.Components
{
    [Route("/gridview")]
    public class GridView : BlazorComponent
    {
        private ITableDataSet tableDataSet;

        [Inject]
        private IGridRenderer GridRenderer { get; set; }

        [Inject]
        private GridRendererContextFactory RendererContextFactory { get; set; }



        [Parameter]
        private ITableDataAdapter DataAdapter { get; set; }

        [Parameter]
        private ILazyLoadingOptions LazyLoadingOptions { get; set; }


        public GridView()
        {
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            GridRenderer.Render(
                RendererContextFactory.CreateRendererContext(tableDataSet, builder)
                );

            base.BuildRenderTree(builder);
        }

        protected override Task OnInitAsync()
        {
            tableDataSet = DataAdapter.GetTableDataSet(conf => conf.LazyLoadingOptions.DataUri = LazyLoadingOptions.DataUri);

            return tableDataSet.GoToPage(0);
        }
    }
}
