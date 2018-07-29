using Blazor.FlexGrid.Components.Configuration.MetaData;
using Blazor.FlexGrid.DataSet;
using Microsoft.AspNetCore.Blazor;
using Microsoft.AspNetCore.Blazor.Components;
using Microsoft.Extensions.Logging;
using System;
using System.Reflection;

namespace Blazor.FlexGrid.Components.Renderers
{
    public class GridHeaderRenderer : GridPartRenderer
    {
        private readonly ILogger<GridHeaderRenderer> logger;

        public GridHeaderRenderer(ILogger<GridHeaderRenderer> logger)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public override void Render(GridRendererContext rendererContext)
        {
            if (!rendererContext.TableDataSet.HasItems())
            {
                return;
            }

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

            rendererContext.CloseElement();
            rendererContext.CloseElement();
        }

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
            rendererContext.OpenElement(HtmlTagNames.TableHeadCell, rendererContext.CssClasses.TableHeaderCell);
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
            rendererContext.OpenElement(HtmlTagNames.TableHeadCell, rendererContext.CssClasses.TableHeaderCell);
            rendererContext.AddContent(GetColumnCaption(columnConfiguration, property));
            rendererContext.CloseElement();
        }

        private void RenderEmptyColumnHeader(GridRendererContext rendererContext)
        {
            rendererContext.OpenElement(HtmlTagNames.TableHeadCell, rendererContext.CssClasses.TableHeaderCell);
            rendererContext.CloseElement();
        }

        private string GetColumnCaption(IGridViewColumnAnotations columnConfiguration, PropertyInfo property)
            => columnConfiguration?.Caption ?? property.Name;
    }
}
