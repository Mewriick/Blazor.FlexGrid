using Blazor.FlexGrid.Components.Configuration;
using Microsoft.Extensions.Logging;
using System;

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
            if (rendererContext.TableDataSet.Items == null ||
                rendererContext.TableDataSet.Items.Count <= 0)
            {
                return;
            }

            var builder = rendererContext.RenderTreeBuilder;
            var seq = rendererContext.Sequence;

            builder.OpenElement(++seq, "thead");
            builder.OpenElement(++seq, "tr");
            foreach (var property in rendererContext.GridItemProperties)
            {
                builder.OpenElement(++seq, "th");
                var columnConfiguration = rendererContext.GridConfiguration.FindColumnConfiguration(property.Name);
                if (columnConfiguration != null)
                {
                    builder.AddContent(++seq, columnConfiguration.Caption);
                }
                else
                {
                    builder.AddContent(++seq, property.Name);
                }

                builder.CloseElement();
            }

            builder.CloseElement();
            builder.CloseElement();

            rendererContext.Sequence = seq;
        }
    }
}
