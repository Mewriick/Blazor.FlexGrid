using Blazor.FlexGrid.Components.Configuration;
using Blazor.FlexGrid.Components.Configuration.MetaData.Conventions;
using Blazor.FlexGrid.Components.Events;
using Blazor.FlexGrid.Components.Filters;
using Blazor.FlexGrid.Components.Renderers;
using Blazor.FlexGrid.DataAdapters;
using Blazor.FlexGrid.DataSet;
using Blazor.FlexGrid.DataSet.Options;
using Blazor.FlexGrid.Filters;
using Blazor.FlexGrid.Permission;
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
        private FlexGridContext fixedFlexGridContext;

        [Inject] IGridRendererTreeBuilder GridRendererTreeBuilder { get; set; }

        [Inject] GridContextsFactory RendererContextFactory { get; set; }

        [Inject] IMasterDetailTableDataSetFactory MasterDetailTableDataSetFactory { get; set; }

        [Inject] ConventionsSet ConventionsSet { get; set; }

        [Parameter] ITableDataAdapter DataAdapter { get; set; }

        [Parameter] ILazyLoadingOptions LazyLoadingOptions { get; set; } = new LazyLoadingOptions();

        [Parameter] int PageSize { get; set; }

        [Parameter] Action<SaveResultArgs> SaveOperationFinished { get; set; }

        [Parameter] Action<DeleteResultArgs> DeleteOperationFinished { get; set; }

        [Parameter] Action<ItemCreatedArgs> NewItemCreated { get; set; }

        [Parameter] Action<ItemClickedArgs> OnItemClicked { get; set; }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);

            var rendererTreeBuilder = new BlazorRendererTreeBuilder(builder);
            var gridContexts = RendererContextFactory.CreateContexts(tableDataSet);

            RenderFragment<ImutableGridRendererContext> tableFragment =
                (ImutableGridRendererContext imutableGridRendererContext) => delegate (RenderTreeBuilder internalBuilder)
            {
                var gridRendererContext = new GridRendererContext(imutableGridRendererContext, new BlazorRendererTreeBuilder(internalBuilder), tableDataSet, fixedFlexGridContext);
                GridRendererTreeBuilder.BuildRendererTree(gridRendererContext, gridContexts.PermissionContext);
            };

            RenderFragment flexGridFragment = delegate (RenderTreeBuilder interalBuilder)
            {
                var internalRendererTreeBuilder = new BlazorRendererTreeBuilder(interalBuilder);
                internalRendererTreeBuilder
                    .OpenComponent(typeof(GridViewTable))
                    .AddAttribute(nameof(ImutableGridRendererContext), gridContexts.ImutableRendererContext)
                    .AddAttribute(RenderTreeBuilder.ChildContent, tableFragment)
                    .CloseComponent();

                if (gridContexts.ImutableRendererContext.CreateItemIsAllowed() &&
                   !fixedFlexGridContext.IsTableForItemsGroup)
                {
                    internalRendererTreeBuilder
                          .OpenComponent(typeof(CreateItemModal))
                          .AddAttribute(nameof(CreateItemOptions), gridContexts.ImutableRendererContext.GridConfiguration.CreateItemOptions)
                          .AddAttribute(nameof(PermissionContext), gridContexts.PermissionContext)
                          .AddAttribute(nameof(CreateFormCssClasses), gridContexts.ImutableRendererContext.CssClasses.CreateFormCssClasses)
                          .AddAttribute(nameof(NewItemCreated), NewItemCreated)
                          .CloseComponent();
                }
            };

            rendererTreeBuilder
                .OpenComponent(typeof(CascadingValue<FlexGridContext>))
                    .AddAttribute("IsFixed", true)
                    .AddAttribute("Value", fixedFlexGridContext)
                    .AddAttribute(nameof(RenderTreeBuilder.ChildContent), flexGridFragment)
                    .CloseComponent();
        }

        protected override async Task OnInitAsync()
        {
            fixedFlexGridContext = CreateFlexGridContext();

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

        protected virtual FlexGridContext CreateFlexGridContext()
            => new FlexGridContext(new FilterContext());

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
                    OnItemClicked = this.OnItemClicked
                };
            });

            if (tableDataSet is null)
            {
                return new TableDataSet<EmptyDataSetItem>(Enumerable.Empty<EmptyDataSetItem>().AsQueryable(), new FilterExpressionTreeBuilder<EmptyDataSetItem>());
            }

            tableDataSet = MasterDetailTableDataSetFactory.ConvertToMasterTableIfIsRequired(tableDataSet);
            fixedFlexGridContext.FilterContext.OnFilterChanged += FilterChanged;


            return tableDataSet;
        }

        private void FilterChanged(object sender, FilterChangedEventArgs e)
        {
            tableDataSet.ApplyFilters(e.Filters);
            fixedFlexGridContext.RequestRerenderTableRowsNotification?.Invoke();
        }
    }
}
