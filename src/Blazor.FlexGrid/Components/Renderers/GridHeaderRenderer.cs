using Blazor.FlexGrid.Components.Configuration;
using Blazor.FlexGrid.Components.Configuration.MetaData;
using Blazor.FlexGrid.DataSet;
using Blazor.FlexGrid.Permission;
using Microsoft.AspNetCore.Components;
using System.Reflection;

namespace Blazor.FlexGrid.Components.Renderers
{
    public class GridHeaderRenderer : GridPartRenderer
    {
        protected override void BuildRendererTreeInternal(GridRendererContext rendererContext, PermissionContext permissionContext)
        {
            var canRenderCreateItemButton = rendererContext.GridConfiguration.CreateItemOptions.IsCreateItemAllowed && permissionContext.HasCreateItemPermission;

            rendererContext.OpenElement(HtmlTagNames.TableHead, rendererContext.CssClasses.TableHeader);
            rendererContext.OpenElement(HtmlTagNames.TableRow, rendererContext.CssClasses.TableHeaderRow);

            if (rendererContext.GridConfiguration.IsMasterTable)
            {
                RenderEmptyColumnHeader(rendererContext);
            }

            foreach (var property in rendererContext.GridItemProperties)
            {
                rendererContext.ActualColumnName = property.Name;
                RenderColumnHeader(rendererContext, property);
            }

            if (canRenderCreateItemButton)
            {
                BuildCreateItemButtonRendererTree(rendererContext, permissionContext);
            }

            if (rendererContext.GridConfiguration.InlineEditOptions.InlineEditIsAllowed && !canRenderCreateItemButton)
            {
                RenderEmptyColumnHeader(rendererContext);
            }

            rendererContext.CloseElement();
            rendererContext.CloseElement();
        }

        public override bool CanRender(GridRendererContext rendererContext)
            => rendererContext.TableDataSet.HasItems();

        private void RenderColumnHeader(GridRendererContext rendererContext, PropertyInfo property)
        {
            var columnConfiguration = rendererContext.ActualColumnConfiguration;
            if (columnConfiguration == null)
            {
                RenderSimpleColumnHeader(rendererContext, property, columnConfiguration);

                return;
            }

            if (columnConfiguration.IsSortable)
            {
                RenderSortableColumnHeader(rendererContext, property, columnConfiguration);
            }
            else
            {
                RenderSimpleColumnHeader(rendererContext, property, columnConfiguration);
            }
        }

        private void RenderSortableColumnHeader(GridRendererContext rendererContext, PropertyInfo property, IGridViewColumnAnotations columnConfiguration)
        {
            rendererContext.OpenElement(HtmlTagNames.TableHeadCell, rendererContext.CssClasses.TableHeaderCell, GetColumnStyle(columnConfiguration));
            rendererContext.OpenElement(HtmlTagNames.Span,
                rendererContext.SortingByActualColumnName ? "table-cell-head-sortable table-cell-head-sortable-active" : "table-cell-head-sortable");
            rendererContext.AddOnClickEvent(() =>
                BindMethods.GetEventHandlerValue(async (UIMouseEventArgs e) =>
                    await rendererContext.TableDataSet.SetSortExpression(property.Name))
            );

            if (rendererContext.SortingByActualColumnName)
            {
                var arrowDirection = rendererContext.TableDataSet.SortingOptions.SortDescending ? "fas fa-arrow-down" : "fas fa-arrow-up";
                rendererContext.AddContent(GetColumnCaption(columnConfiguration, property));
                rendererContext.OpenElement(HtmlTagNames.I, $"table-cell-head-arrow {arrowDirection}");
                rendererContext.CloseElement();
            }
            else
            {
                rendererContext.AddContent(GetColumnCaption(columnConfiguration, property));
            }

            rendererContext.CloseElement();
            rendererContext.CloseElement();
        }

        private void RenderSimpleColumnHeader(GridRendererContext rendererContext, PropertyInfo property, IGridViewColumnAnotations columnConfiguration)
        {
            rendererContext.OpenElement(HtmlTagNames.TableHeadCell, rendererContext.CssClasses.TableHeaderCell, GetColumnStyle(columnConfiguration));
            rendererContext.AddContent(GetColumnCaption(columnConfiguration, property));
            rendererContext.CloseElement();
        }

        private void RenderEmptyColumnHeader(GridRendererContext rendererContext)
        {
            rendererContext.OpenElement(HtmlTagNames.TableHeadCell, rendererContext.CssClasses.TableHeaderCell);
            rendererContext.CloseElement();
        }

        private void BuildCreateItemButtonRendererTree(GridRendererContext rendererContext, PermissionContext permissionContext)
        {
            rendererContext.OpenElement(HtmlTagNames.TableHeadCell, rendererContext.CssClasses.TableHeaderCell);
            rendererContext.OpenElement(HtmlTagNames.Div, "create-button-wrapper");

            rendererContext.OpenElement(HtmlTagNames.Button, "action-button");
            rendererContext.AddOnClickEvent(() =>
                BindMethods.GetEventHandlerValue((UIMouseEventArgs e) =>
                    FlexGridInterop.ShowModal(CreateItemOptions.CreateItemModalName))
            );

            rendererContext.OpenElement(HtmlTagNames.Span, "action-button-span");
            rendererContext.OpenElement(HtmlTagNames.I, "fas fa-plus");
            rendererContext.CloseElement();
            rendererContext.CloseElement();
            rendererContext.CloseElement();
            rendererContext.CloseElement();
            rendererContext.CloseElement();
        }

        private string GetColumnCaption(IGridViewColumnAnotations columnConfiguration, PropertyInfo property)
            => columnConfiguration?.Caption ?? property.Name;

        private string GetColumnStyle(IGridViewColumnAnotations columnConfiguration)
            => columnConfiguration?.HeaderStyle ?? string.Empty;
    }
}
