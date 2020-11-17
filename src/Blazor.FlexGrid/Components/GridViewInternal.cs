﻿using Blazor.FlexGrid.Components.Configuration;
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
using Blazor.FlexGrid.State;
using Blazor.FlexGrid.Triggers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
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

        private bool tableDataSetInitialized;
        private int pageSize;
        private int? cachedCurrentPage;
        private ITableDataAdapter dataAdapter;
        private ILazyLoadingOptions lazyLoadingOptions;

        private FlexGridContext fixedFlexGridContext;
        private (ImutableGridRendererContext ImutableRendererContext, PermissionContext PermissionContext) gridRenderingContexts;
        private IParameterActionTriggerCollection actionTriggerCollection;

        protected IEnumerable<IFeature> features;
        protected ITableDataSet tableDataSet;

        [Inject] IGridRendererTreeBuilder GridRendererTreeBuilder { get; set; }

        [Inject] GridContextsFactory RendererContextFactory { get; set; }

        [Inject] IMasterDetailTableDataSetFactory MasterDetailTableDataSetFactory { get; set; }

        [Inject] ISpecialColumnFragmentsCollection SpecialColumnFragmentsCollection { get; set; }

        [Inject] ConventionsSet ConventionsSet { get; set; }

        [Inject] IStateCache StateCache { get; set; }

        [Parameter]
        public ITableDataAdapter DataAdapter
        {
            get => dataAdapter;
            set
            {
                if (dataAdapter != value)
                {
                    dataAdapter = value;
                    AddTrigger(() => new RefreshDataAdapterTrigger(GetTableDataSet));
                }
            }
        }

        [Parameter]
        public ILazyLoadingOptions LazyLoadingOptions
        {
            get => lazyLoadingOptions;
            set
            {
                if (lazyLoadingOptions != value)
                {
                    lazyLoadingOptions = value;
                    AddTrigger(() => new RefreshLazyLoadingOptionsTrigger(tableDataSet, value));
                }
            }
        }

        [Parameter]
        public int PageSize
        {
            get => pageSize;
            set
            {
                if (pageSize != value)
                {
                    pageSize = value;
                    AddTrigger(() => new RefreshPageSizeTrigger(tableDataSet.PageableOptions, value));
                }
            }
        }
        [Parameter] public Action<SaveResultArgs> SaveOperationFinished { get; set; }

        [Parameter] public Action<DeleteResultArgs> DeleteOperationFinished { get; set; }

        [Parameter] public Action<ItemCreatedArgs> NewItemCreated { get; set; }

        [Parameter] public Action<ItemClickedArgs> OnItemClicked { get; set; }

        public GridViewInternal()
            : this(DefaultFeatureCollection.AllFeatures)
        {
        }

        protected GridViewInternal(IEnumerable<IFeature> features)
        {
            this.features = features ?? throw new ArgumentNullException(nameof(features));
            this.actionTriggerCollection = new TriggerActionCollection();
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);
            var rendererTreeBuilder = new BlazorRendererTreeBuilder(builder);

            RenderFragment<ImutableGridRendererContext> tableFragment =
                (ImutableGridRendererContext imutableGridRendererContext) => delegate (RenderTreeBuilder internalBuilder)
            {
                var gridRendererContext = new GridRendererContext(
                    imutableGridRendererContext,
                    new BlazorRendererTreeBuilder(internalBuilder),
                    tableDataSet,
                    SpecialColumnFragmentsCollection,
                    fixedFlexGridContext);
                GridRendererTreeBuilder.BuildRendererTree(gridRendererContext, gridRenderingContexts.PermissionContext);
            };

            RenderFragment flexGridFragment = delegate (RenderTreeBuilder interalBuilder)
            {
                var internalRendererTreeBuilder = new BlazorRendererTreeBuilder(interalBuilder);
                internalRendererTreeBuilder
                    .OpenComponent(typeof(GridViewTable))
                    .AddAttribute(nameof(ImutableGridRendererContext), gridRenderingContexts.ImutableRendererContext)
                    .AddAttribute(BlazorRendererTreeBuilder.ChildContent, tableFragment)
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
                    .AddAttribute(nameof(BlazorRendererTreeBuilder.ChildContent), flexGridFragment)
                .CloseComponent();
        }

        protected override async Task OnInitializedAsync()
        {
            fixedFlexGridContext = CreateFlexGridContext();

            if (DataAdapter != null)
            {
                ConventionsSet.ApplyConventions(DataAdapter.UnderlyingTypeOfItem);
            }

            GetTableDataSet();
            await RestoreCachedState();
            await tableDataSet.GoToPage(cachedCurrentPage.HasValue ? cachedCurrentPage.Value : 0);

            if (DataAdapter != null)
            {
                fixedFlexGridContext.FirstPageLoaded = true;
            }
        }

        protected override async Task OnParametersSetAsync()
        {
            if (!tableDataSetInitialized &&
                DataAdapter != null)
            {
                ConventionsSet.ApplyConventions(DataAdapter.UnderlyingTypeOfItem);
                GetTableDataSet();
                await RestoreCachedState();
                await tableDataSet.GoToPage(cachedCurrentPage.HasValue ? cachedCurrentPage.Value : 0);
                fixedFlexGridContext.FirstPageLoaded = true;
            }

            if (fixedFlexGridContext.FirstPageLoaded)
            {
                await actionTriggerCollection.ExecuteTriggers(() =>
                    tableDataSet.GoToPage(tableDataSet.PageableOptions.CurrentPage)
               );
            }
        }

        protected virtual FlexGridContext CreateFlexGridContext()
            => new FlexGridContext(new FilterContext(), new FeatureCollection(features));

        protected ITableDataSet GetTableDataSet()
        {
            tableDataSet = DataAdapter?.GetTableDataSet(conf =>
            {
                conf.LazyLoadingOptions = LazyLoadingOptions;
                conf.PageableOptions.PageChanged = PageChanged;
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
                DataAdapter.AfterReloadPage = fixedFlexGridContext.RequestRerenderTableRowsNotification;
                tableDataSet = MasterDetailTableDataSetFactory.ConvertToMasterTableIfIsRequired(tableDataSet);
                if (fixedFlexGridContext.IsFeatureActive<FilteringFeature>())
                {
                    fixedFlexGridContext.FilterContext.OnFilterChanged += FilterChanged;
                }

                tableDataSetInitialized = true;
            }

            gridRenderingContexts = RendererContextFactory.CreateContexts(tableDataSet);
            fixedFlexGridContext.GridConfiguration = gridRenderingContexts.ImutableRendererContext.GridConfiguration;
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

        private async Task RestoreCachedState()
        {
            if (tableDataSet == EmptyDataSet || StateCache.Count == 0)
            {
                return;
            }

            if (fixedFlexGridContext.IsFeatureActive<FilteringFeature>() &&
                fixedFlexGridContext.GridConfiguration.PreserveFiltering)
            {
                var cachedDefinitions = StateCache.GetFilterDefinitions(gridRenderingContexts.ImutableRendererContext);
                if (cachedDefinitions.Any())
                {
                    await tableDataSet.ApplyFilters(cachedDefinitions, false);
                }
            }

            if (fixedFlexGridContext.GridConfiguration.PreservePagination &&
                StateCache.TryGetStateValue<int>(StateKeys.CurrentPage, out var currentPage))
            {
                cachedCurrentPage = currentPage;
            }
            else
            {
                cachedCurrentPage = null;
            }
        }

        private void PageChanged(int e)
        {
            Console.WriteLine($"PageChanged {e}");
            StateCache.SetStateValue(StateKeys.CurrentPage, e);
        }

        private void FilterChanged(object sender, FilterChangedEventArgs e)
        {
            tableDataSet.ApplyFilters(e.Filters);
            fixedFlexGridContext.RequestRerenderTableRowsNotification?.Invoke();
        }

        private void AddTrigger(Func<IParamterChangedTrigger> createTrigger)
        {
            if (tableDataSetInitialized)
            {
                actionTriggerCollection.AddTrigger(createTrigger());
            }
        }
    }
}
