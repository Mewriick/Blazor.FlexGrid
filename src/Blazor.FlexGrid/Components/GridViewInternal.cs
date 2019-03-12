using Blazor.FlexGrid.Components.Configuration;
using Blazor.FlexGrid.Components.Configuration.MetaData.Conventions;
using Blazor.FlexGrid.Components.Events;
using Blazor.FlexGrid.Components.Renderers;
using Blazor.FlexGrid.DataAdapters;
using Blazor.FlexGrid.DataSet;
using Blazor.FlexGrid.DataSet.Options;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.RenderTree;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Blazor.FlexGrid.Components
{
    public class GridViewInternal : ComponentBase
    {
        private ITableDataSet tableDataSet;
        private bool dataAdapterWasEmptyInOnInit;

        [Inject]
        private IGridRendererTreeBuilder GridRendererTreeBuilder { get; set; }

        [Inject]
        private GridContextsFactory RendererContextFactory { get; set; }

        [Inject]
        private IMasterDetailTableDataSetFactory MasterDetailTableDataSetFactory { get; set; }

        [Inject]
        private ConventionsSet ConventionsSet { get; set; }

        [Parameter] CreateItemContext CreateItemContext { get; set; } = new CreateItemContext(NullCreateItemOptions.Instance);


        [Parameter] ITableDataAdapter DataAdapter { get; set; }


        [Parameter] ILazyLoadingOptions LazyLoadingOptions { get; set; } = new LazyLoadingOptions();


        [Parameter] int PageSize { get; set; }


        [Parameter] Action<SaveResultArgs> SaveOperationFinished { get; set; }


        [Parameter] Action<DeleteResultArgs> DeleteOperationFinished { get; set; }


        [Parameter] Action<ItemCreatedArgs> NewItemCreated { get; set; }


        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            Console.WriteLine("Render");

            base.BuildRenderTree(builder);
            var gridContexts = RendererContextFactory.CreateContexts(tableDataSet, builder);
            gridContexts.RendererContext.RequestRerender = StateHasChanged;

            GridRendererTreeBuilder.BuildRendererTree(gridContexts.RendererContext, gridContexts.PermissionContext);
        }

        protected override async Task OnInitAsync()
        {
            dataAdapterWasEmptyInOnInit = DataAdapter == null;
            if (!dataAdapterWasEmptyInOnInit)
            {
                ConventionsSet.ApplyConventions(DataAdapter.UnderlyingTypeOfItem);

                var createItemOptions = RendererContextFactory
                    .GridConfigurationProvider
                    .GetGridConfigurationByType(DataAdapter.UnderlyingTypeOfItem)
                    .CreateItemOptions;

                CreateItemContext = new CreateItemContext(createItemOptions);
            }

            tableDataSet = GetTableDataSet();

            await tableDataSet.GoToPage(0);
        }

        protected override async Task OnParametersSetAsync()
        {
            if (dataAdapterWasEmptyInOnInit && DataAdapter != null)
            {
                if (CreateItemContext is null)
                {
                    var createItemOptions = RendererContextFactory
                        .GridConfigurationProvider
                        .GetGridConfigurationByType(DataAdapter.UnderlyingTypeOfItem)
                        .CreateItemOptions;

                    CreateItemContext = new CreateItemContext(createItemOptions);
                }

                ConventionsSet.ApplyConventions(DataAdapter.UnderlyingTypeOfItem);
                tableDataSet = GetTableDataSet();

                await tableDataSet.GoToPage(0);
            }
        }

        private ITableDataSet GetTableDataSet()
        {
            var tableDataSet = DataAdapter?.GetTableDataSet(conf =>
            {
                conf.LazyLoadingOptions = LazyLoadingOptions;
                conf.PageableOptions.PageSize = PageSize;
                conf.GridViewEvents = new GridViewEvents
                {
                    SaveOperationFinished = this.SaveOperationFinished,
                    DeleteOperationFinished = this.DeleteOperationFinished,
                    NewItemCreated = this.NewItemCreated
                };
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
