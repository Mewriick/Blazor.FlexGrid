using Blazor.FlexGrid.Components.Configuration;
using Blazor.FlexGrid.Components.Configuration.MetaData;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace Blazor.FlexGrid.Components.Renderers
{
    public class GridBodyRenderer : GridPartRenderer
    {
        private readonly ILogger<GridBodyRenderer> logger;
        private readonly Dictionary<string, IGridViewColumnAnnotations> columnAnnotationsCache;

        public GridBodyRenderer(ILogger<GridBodyRenderer> logger)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.columnAnnotationsCache = new Dictionary<string, IGridViewColumnAnnotations>();
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

                        var columnConfiguration = GetColumnAnnotations(rendererContext, property.Name);
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

        private IGridViewColumnAnnotations GetColumnAnnotations(GridRendererContext gridRendererContext, string columnName)
        {
            var configurationKey = $"{gridRendererContext.GridConfiguration.Name}_{columnName}";
            if (columnAnnotationsCache.TryGetValue(configurationKey, out var columnConfiguration))
            {
                return columnConfiguration;
            }

            columnConfiguration = gridRendererContext.GridConfiguration.FindColumnConfiguration(columnName);
            if (columnConfiguration == null)
            {
                return null;
            }

            columnAnnotationsCache.Add(configurationKey, columnConfiguration);

            return columnConfiguration;
        }
    }
}
