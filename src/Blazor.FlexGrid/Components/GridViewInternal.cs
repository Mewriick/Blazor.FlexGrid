using Blazor.FlexGrid.Components.Configuration;
using Blazor.FlexGrid.Components.Configuration.MetaData.Conventions;
using Blazor.FlexGrid.Components.Events;
using Blazor.FlexGrid.Components.Renderers;
using Blazor.FlexGrid.DataAdapters;
using Blazor.FlexGrid.DataSet;
using Blazor.FlexGrid.DataSet.Options;
using Blazor.FlexGrid.Permission;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.RenderTree;
using Microsoft.JSInterop;
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



        [Parameter] ITableDataAdapter DataAdapter { get; set; }


        [Parameter] ILazyLoadingOptions LazyLoadingOptions { get; set; } = new LazyLoadingOptions();


        [Parameter] int PageSize { get; set; }


        [Parameter] Action<SaveResultArgs> SaveOperationFinished { get; set; }


        [Parameter] Action<DeleteResultArgs> DeleteOperationFinished { get; set; }


        [Parameter] Action<ItemCreatedArgs> NewItemCreated { get; set; }

        [Parameter] Action<object> OnItemClicked { get; set; }


        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);

            var rendererTreeBuilder = new BlazorRendererTreeBuilder(builder);
            var gridContexts = RendererContextFactory.CreateContexts(tableDataSet);

            RenderFragment<ImutableGridRendererContext> grid = (ImutableGridRendererContext imutableGridRendererContext)
                => delegate (RenderTreeBuilder internalBuilder)
            {
                var gridRendererContext = new GridRendererContext(gridContexts.ImutableRendererContext, 
                                                                  new BlazorRendererTreeBuilder(internalBuilder), 
                                                                  tableDataSet);
                GridRendererTreeBuilder.BuildRendererTree(gridRendererContext, gridContexts.PermissionContext);
            };

            rendererTreeBuilder
                .OpenComponent(typeof(GridViewTable))
                .AddAttribute(nameof(ImutableGridRendererContext), gridContexts.ImutableRendererContext)
                .AddAttribute(RenderTreeBuilder.ChildContent, grid)
                .CloseComponent();

            if (gridContexts.ImutableRendererContext.GridConfiguration.CreateItemOptions.IsCreateItemAllowed)
            {
                rendererTreeBuilder
                      .OpenComponent(typeof(CreateItemModal))
                      .AddAttribute(nameof(CreateItemOptions), gridContexts.ImutableRendererContext.GridConfiguration.CreateItemOptions)
                      .AddAttribute(nameof(PermissionContext), gridContexts.PermissionContext)
                      .AddAttribute(nameof(CreateFormCssClasses), gridContexts.ImutableRendererContext.CssClasses.CreateFormCssClasses)
                      .AddAttribute(nameof(NewItemCreated), NewItemCreated)
                      .CloseComponent();
            }
        }

        protected override async Task OnInitAsync()
        {
            dataAdapterWasEmptyInOnInit = DataAdapter == null;
            if (!dataAdapterWasEmptyInOnInit)
            {
                ConventionsSet.ApplyConventions(DataAdapter.UnderlyingTypeOfItem);
            }

            tableDataSet = GetTableDataSet();

            await tableDataSet.GoToPage(0);
        }

        protected override async Task OnParametersSetAsync()
        {
            if (dataAdapterWasEmptyInOnInit && DataAdapter != null)
            {
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
                    NewItemCreated = this.NewItemCreated,
                    OnItemClicked = OnItemClicked != null 
                                 ? (Action<ItemClickedArgs>)((args) => this.OnItemClicked(args.Item))
                                 : null
                                    
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
