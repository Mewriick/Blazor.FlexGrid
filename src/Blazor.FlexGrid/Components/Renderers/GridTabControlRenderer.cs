using Blazor.FlexGrid.Components.Configuration;
using Blazor.FlexGrid.DataAdapters;
using Blazor.FlexGrid.DataSet;
using Blazor.FlexGrid.DataSet.Options;
using Blazor.FlexGrid.Permission;
using Microsoft.AspNetCore.Blazor;
using Microsoft.AspNetCore.Blazor.Components;
using System;

namespace Blazor.FlexGrid.Components.Renderers
{
    public class GridTabControlRenderer : GridPartRenderer
    {
        private readonly ITableDataAdapterProvider tableDataAdapterProvider;

        public GridTabControlRenderer(ITableDataAdapterProvider tableDataAdapterProvider)
        {
            this.tableDataAdapterProvider = tableDataAdapterProvider ?? throw new ArgumentNullException(nameof(tableDataAdapterProvider));
        }

        public override bool CanRender(GridRendererContext rendererContext)
            => rendererContext.GridConfiguration.IsMasterTable &&
               rendererContext.TableDataSet.ItemIsSelected(rendererContext.ActualItem);

        protected override void RenderInternal(GridRendererContext rendererContext, PermissionContext permissionContext)
        {
            if (rendererContext.TableDataSet is IMasterTableDataSet masterTableDataSet)
            {
                var selectedDataAdapter = masterTableDataSet.GetSelectedDataAdapter(rendererContext.ActualItem);

                rendererContext.OpenElement(HtmlTagNames.TableRow, rendererContext.CssClasses.TableRow);
                rendererContext.OpenElement(HtmlTagNames.TableColumn, rendererContext.CssClasses.TableCell);
                rendererContext.AddColspan();


                rendererContext.OpenElement(HtmlTagNames.Div, "tabs-header");
                rendererContext.OpenElement(HtmlTagNames.Div, "tabs-header-buttons");

                RenderTabs(rendererContext, permissionContext, masterTableDataSet, selectedDataAdapter);

                rendererContext.CloseElement();
                rendererContext.CloseElement();

                rendererContext.OpenElement(HtmlTagNames.Div, "tabs-content");
                rendererContext.AddDetailGridViewComponent(selectedDataAdapter);
                rendererContext.CloseElement();
                rendererContext.CloseElement();
                rendererContext.CloseElement();
            }
        }

        private void RenderTabs(
            GridRendererContext rendererContext,
            PermissionContext permissionContext,
            IMasterTableDataSet masterTableDataSet,
            ITableDataAdapter selectedDataAdapter)
        {
            foreach (var dataAdapter in masterTableDataSet.DetailDataAdapters)
            {
                RenderTab(rendererContext, masterTableDataSet, selectedDataAdapter, dataAdapter);
            }

            foreach (var collectionProperty in rendererContext.GridItemCollectionProperties)
            {
                if (permissionContext.HasCurrentUserReadPermission(collectionProperty.Name))
                {
                    var dataAdapter = tableDataAdapterProvider.CreateCollectionTableDataAdapter(rendererContext.ActualItem, collectionProperty);
                    RenderTab(rendererContext, masterTableDataSet, selectedDataAdapter, dataAdapter);
                }
            }
        }

        private void RenderTab(
            GridRendererContext rendererContext,
            IMasterTableDataSet masterTableDataSet,
            ITableDataAdapter selectedDataAdapter,
            ITableDataAdapter dataAdapter)
        {
            var masterDetailRelationship = rendererContext
                .GridConfiguration
                .FindRelationshipConfiguration(dataAdapter.UnderlyingTypeOfItem);

            var localActualItem = rendererContext.ActualItem;
            rendererContext.OpenElement(HtmlTagNames.Button,
                selectedDataAdapter.IsForSameUnderlyingType(dataAdapter) ? "tabs-button tabs-button-active" : "tabs-button");

            rendererContext.AddOnClickEvent(() =>
                BindMethods.GetEventHandlerValue((UIMouseEventArgs e) =>
                    masterTableDataSet.SelectDataAdapter(new MasterDetailRowArguments(dataAdapter, localActualItem)))
            );

            rendererContext.OpenElement(HtmlTagNames.Span, "tabs-button-text");
            rendererContext.AddContent(masterDetailRelationship.DetailGridViewPageCaption(dataAdapter));
            rendererContext.CloseElement();
            rendererContext.CloseElement();
        }
    }
}
