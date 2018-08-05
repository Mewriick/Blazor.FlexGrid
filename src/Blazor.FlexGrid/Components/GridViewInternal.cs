using Blazor.FlexGrid.Components.Configuration.MetaData.Conventions;
using Blazor.FlexGrid.Components.Renderers;
using Blazor.FlexGrid.DataAdapters;
using Blazor.FlexGrid.DataSet;
using Blazor.FlexGrid.DataSet.Options;
using Microsoft.AspNetCore.Blazor.Components;
using Microsoft.AspNetCore.Blazor.RenderTree;
using System.Linq;
using System.Threading.Tasks;

namespace Blazor.FlexGrid.Components
{
    public class GridViewInternal : BlazorComponent
    {
        private ITableDataSet tableDataSet;
        private bool dataAdapterWasEmptyInOnInit;

        [Inject]
        private IGridRenderer GridRenderer { get; set; }

        [Inject]
        private GridRendererContextFactory RendererContextFactory { get; set; }

        [Inject]
        private IMasterDetailTableDataSetFactory MasterDetailTableDataSetFactory { get; set; }

        [Inject]
        private ConventionsSet ConventionsSet { get; set; }


        [Parameter]
        private ITableDataAdapter DataAdapter { get; set; }

        [Parameter]
        private ILazyLoadingOptions LazyLoadingOptions { get; set; } = new LazyLoadingOptions();

        [Parameter]
        private int PageSize { get; set; }


        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            GridRenderer.Render(
                RendererContextFactory.CreateRendererContext(tableDataSet, builder)
                );

            base.BuildRenderTree(builder);
        }

        protected override Task OnInitAsync()
        {
            dataAdapterWasEmptyInOnInit = DataAdapter == null;
            if (!dataAdapterWasEmptyInOnInit)
            {
                ConventionsSet.ApplyConventions(DataAdapter.UnderlyingTypeOfItem);
            }

            tableDataSet = GetTableDataSet();

            return tableDataSet.GoToPage(0);
        }

        protected override Task OnParametersSetAsync()
        {
            if (dataAdapterWasEmptyInOnInit && DataAdapter != null)
            {
                ConventionsSet.ApplyConventions(DataAdapter.UnderlyingTypeOfItem);
                tableDataSet = GetTableDataSet();
                return tableDataSet.GoToPage(0);
            }

            return Task.FromResult(0);
        }

        private ITableDataSet GetTableDataSet()
        {
            var tableDataSet = DataAdapter?.GetTableDataSet(conf =>
            {
                conf.LazyLoadingOptions.DataUri = LazyLoadingOptions.DataUri;
                conf.PageableOptions.PageSize = PageSize;
            });

            if (tableDataSet is null)
            {
                return new TableDataSet<EmptyDataSetItem>(Enumerable.Empty<EmptyDataSetItem>().AsQueryable());
            }

            tableDataSet = MasterDetailTableDataSetFactory.ConvertToMasterTableIfIsRequired(tableDataSet);

            return tableDataSet;
        }
    }
}
