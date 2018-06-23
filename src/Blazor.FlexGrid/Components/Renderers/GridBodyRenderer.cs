using Blazor.FlexGrid.Components.Configuration;
using Microsoft.Extensions.Logging;
using System;

namespace Blazor.FlexGrid.Components.Renderers
{
    public class GridBodyRenderer : GridPartRenderer
    {
        private readonly ILogger<GridBodyRenderer> logger;

        public GridBodyRenderer(ILogger<GridBodyRenderer> logger)
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

            builder.OpenElement(++seq, "tbody");

            try
            {
                foreach (var item in rendererContext.TableDataSet.Items)
                {
                    builder.OpenElement(++seq, "tr");
                    foreach (var property in rendererContext.GridItemProperties)
                    {
                        builder.OpenElement(++seq, "td");

                        var columnConfiguration = rendererContext.GridConfiguration.FindColumnConfiguration(property.Name);
                        if (columnConfiguration != null)
                        {
                            var formatValueFunc = columnConfiguration.ValueFormatter.FormatValue;
                            var formattedValue = formatValueFunc(property.GetValue(item));

                            builder.AddContent(++seq, formattedValue);
                        }
                        else
                        {
                            builder.AddContent(++seq, property.GetValue(item).ToString());
                        }

                        builder.CloseElement();
                    }

                    builder.CloseElement();
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"GridBodyRenderer ex: {ex}");
            }

            builder.CloseElement();

            rendererContext.Sequence = seq;
        }
    }
}
