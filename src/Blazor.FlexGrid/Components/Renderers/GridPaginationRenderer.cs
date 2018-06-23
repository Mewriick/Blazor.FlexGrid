using Blazor.FlexGrid.DataSet;
using Microsoft.AspNetCore.Blazor;
using Microsoft.AspNetCore.Blazor.Components;

namespace Blazor.FlexGrid.Components.Renderers
{
    public class GridPaginationRenderer : GridPartRenderer
    {
        public override void Render(GridRendererContext rendererContext)
        {
            var builder = rendererContext.RenderTreeBuilder;
            var seq = rendererContext.Sequence;

            builder.OpenElement(++seq, "p");
            builder.AddContent(++seq, $"{rendererContext.TableDataSet.PageableOptions.CurrentPage} / {rendererContext.TableDataSet.PageableOptions.PagesCount}");
            builder.CloseElement();

            builder.OpenElement(++seq, "button");
            builder.AddAttribute(++seq, "onclick", BindMethods.GetEventHandlerValue((UIMouseEventArgs async) => rendererContext.TableDataSet.GoToNextPage()));
            builder.AddContent(++seq, "Next");
            builder.CloseElement();
        }
    }
}
