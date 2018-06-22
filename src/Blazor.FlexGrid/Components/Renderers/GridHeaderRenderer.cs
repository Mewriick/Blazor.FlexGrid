using Blazor.FlexGrid.Components.Configuration;
using Blazor.FlexGrid.Components.Configuration.MetaData;

namespace Blazor.FlexGrid.Components.Renderers
{
    public class GridHeaderRenderer : IGridRenderer
    {
        public void Render(GridRendererContext rendererContext)
        {
            var builder = rendererContext.RenderTreeBuilder;
            var seq = rendererContext.Sequence;

            builder.OpenElement(++seq, "thead");
            builder.OpenElement(++seq, "tr");
            foreach (var property in rendererContext.GridItemProperties)
            {
                builder.OpenElement(++seq, "th");

                var columnConfiguration = rendererContext.GridConfiguration.FindProperty(property.Name);
                if (columnConfiguration != null)
                {
                    var columnCaption = columnConfiguration[GridViewAnnotationNames.ColumnCaption] as Annotation;
                    if (columnCaption is null)
                    {
                        builder.AddContent(++seq, property.Name);
                    }
                    else
                    {
                        builder.AddContent(++seq, columnCaption.Value.ToString());
                    }
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
