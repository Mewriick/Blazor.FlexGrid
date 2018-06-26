using Blazor.FlexGrid.DataSet;
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

            rendererContext.OpenElement(HtmlTagNames.TableHead, "table-head");
            rendererContext.OpenElement(HtmlTagNames.TableRow, "table-head-row");

            foreach (var property in rendererContext.GridItemProperties)
            {
                rendererContext.ActualColumnName = property.Name;

                var columnCaption = GetColumnCaption(rendererContext, property);
                rendererContext.OpenElement(HtmlTagNames.TableHeadCell, "table-cell-head");
                rendererContext.AddContent(columnCaption);
                rendererContext.CloseElement();
            }

            rendererContext.CloseElement();
            rendererContext.CloseElement();
        }

        private string GetColumnCaption(GridRendererContext rendererContext, PropertyInfo property)
            => rendererContext.ActualColumnConfiguration?.Caption ?? property.Name;
    }
}
