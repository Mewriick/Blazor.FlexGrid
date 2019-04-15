using Blazor.FlexGrid.Components.Configuration;
using Blazor.FlexGrid.Components.Configuration.MetaData.Conventions;
using Blazor.FlexGrid.Components.Events;
using Blazor.FlexGrid.Components.Filters;
using Blazor.FlexGrid.Components.Renderers;
using Blazor.FlexGrid.DataAdapters;
using Blazor.FlexGrid.DataSet;
using Blazor.FlexGrid.DataSet.Options;
using Blazor.FlexGrid.Features;
using Blazor.FlexGrid.Filters;
using Blazor.FlexGrid.Permission;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.RenderTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazor.FlexGrid.Components
{
    public class GridViewInternal : ComponentBase
    {
        private readonly static ITableDataSet EmptyDataSet = new TableDataSet<EmptyDataSetItem>(
            Enumerable.Empty<EmptyDataSetItem>().AsQueryable(), new FilterExpressionTreeBuilder<EmptyDataSetItem>());

        private bool dataAdapterWasEmptyInOnInit;
        private FlexGridContext fixedFlexGridContext;
        private (ImutableGridRendererContext ImutableRendererContext, PermissionContext PermissionContext) gridRenderingContexts;
        protected IEnumerable<IFeature> features;
        protected ITableDataSet tableDataSet;

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

        public GridViewInternal()
            : this(DefaultFeatureCollection.AllFeatures)
        {
        }

        protected GridViewInternal(IEnumerable<IFeature> features)
        {
            this.features = features ?? throw new ArgumentNullException(nameof(features));
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);
            var rendererTreeBuilder = new BlazorRendererTreeBuilder(builder);

            RenderFragment<ImutableGridRendererContext> tableFragment =
                (ImutableGridRendererContext imutableGridRendererContext) => delegate (RenderTreeBuilder internalBuilder)
            {
                var gridRendererContext = new GridRendererContext(imutableGridRendererContext, new BlazorRendererTreeBuilder(internalBuilder), tableDataSet, fixedFlexGridContext);
                GridRendererTreeBuilder.BuildRendererTree(gridRendererContext, gridRenderingContexts.PermissionContext);
            };

            RenderFragment flexGridFragment = delegate (RenderTreeBuilder interalBuilder)
            {
                var internalRendererTreeBuilder = new BlazorRendererTreeBuilder(interalBuilder);
                internalRendererTreeBuilder
                    .OpenComponent(typeof(GridViewTable))
                    .AddAttribute(nameof(ImutableGridRendererContext), gridRenderingContexts.ImutableRendererContext)
                    .AddAttribute(RenderTreeBuilder.ChildContent, tableFragment)
                    .CloseComponent();

                if (gridRenderingContexts.ImutableRendererContext.CreateItemIsAllowed() &&
                    fixedFlexGridContext.IsFeatureActive<CreateItemFeature>())
                {
                    internalRendererTreeBuilder
                          .OpenComponent(typeof(CreateItemModal))
                          .AddAttribute(nameof(CreateItemOptions), gridRenderingContexts.ImutableRendererContext.GridConfiguration.CreateItemOptions)
                          .AddAttribute(nameof(PermissionContext), gridRenderingContexts.PermissionContext)
                          .AddAttribute(nameof(CreateFormCssClasses), gridRenderingContexts.ImutableRendererContext.CssClasses.CreateFormCssClasses)
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
            => new FlexGridContext(new FilterContext(), new FeatureCollection(features));

        protected ITableDataSet GetTableDataSet()
        {
            var tableDataSet = DataAdapter?.GetTableDataSet(conf =>
            {
                conf.LazyLoadingOptions = LazyLoadingOptions;
                conf.PageableOptions.PageSize = PageSize;
                conf.GridViewEvents = new GridViewEvents
                {
                    SaveOperationFinished = SaveOperationFinished,
                    DeleteOperationFinished = DeleteOperationFinished,
                    NewItemCreated = NewItemCreated,
                    OnItemClicked = OnItemClicked
                };
            });

            if (tableDataSet is null)
            {
                tableDataSet = EmptyDataSet;
            }
            else
            {
                tableDataSet = MasterDetailTableDataSetFactory.ConvertToMasterTableIfIsRequired(tableDataSet);
                if (fixedFlexGridContext.IsFeatureActive<FilteringFeature>())
                {
                    fixedFlexGridContext.FilterContext.OnFilterChanged += FilterChanged;
                }
            }

            gridRenderingContexts = RendererContextFactory.CreateContexts(tableDataSet);
            if (fixedFlexGridContext.IsFeatureActive<GroupingFeature>())
            {
                tableDataSet.GroupingOptions.SetConfiguration(
                    gridRenderingContexts.ImutableRendererContext.GridConfiguration.GroupingOptions,
                    gridRenderingContexts.ImutableRendererContext.GridItemProperties);
            }

            if (tableDataSet is IMasterTableDataSet)
            {
                fixedFlexGridContext.Features.Set<IMasterTableFeature>(new MasterTableFeature(DataAdapter));
            }

            return tableDataSet;
        }

        private void FilterChanged(object sender, FilterChangedEventArgs e)
        {
            tableDataSet.ApplyFilters(e.Filters);
            fixedFlexGridContext.RequestRerenderTableRowsNotification?.Invoke();
        }
    }
}
