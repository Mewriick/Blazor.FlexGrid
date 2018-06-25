using Blazor.FlexGrid.Components.Configuration;
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

            rendererContext.RenderTreeBuilder.OpenElement(++rendererContext.Sequence, HtmlTagNames.TableHead);
            rendererContext.RenderTreeBuilder.AddAttribute(++rendererContext.Sequence, HtmlAttributes.Class, "table-head");
            rendererContext.RenderTreeBuilder.OpenElement(++rendererContext.Sequence, HtmlTagNames.TableRow);
            rendererContext.RenderTreeBuilder.AddAttribute(++rendererContext.Sequence, HtmlAttributes.Class, "table-head-row");

            foreach (var property in rendererContext.GridItemProperties)
            {
                rendererContext.RenderTreeBuilder.OpenElement(++rendererContext.Sequence, HtmlTagNames.TableHeadCell);
                rendererContext.RenderTreeBuilder.AddAttribute(++rendererContext.Sequence, HtmlAttributes.Class, "table-cell-head");

                var columnCaption = GetColumnCaption(rendererContext, property);
                rendererContext.RenderTreeBuilder.AddContent(++rendererContext.Sequence, columnCaption);

                rendererContext.RenderTreeBuilder.CloseElement();
            }

            rendererContext.RenderTreeBuilder.CloseElement();
            rendererContext.RenderTreeBuilder.CloseElement();

        }

        private string GetColumnCaption(GridRendererContext rendererContext, PropertyInfo property)
        {
            var columnConfiguration = rendererContext.GridConfiguration.FindColumnConfiguration(property.Name);
            if (columnConfiguration != null)
            {
                return columnConfiguration.Caption;
            }

            return property.Name;
        }
    }
}
