using Blazor.FlexGrid.DataSet;
using Microsoft.AspNetCore.Blazor;
using Microsoft.AspNetCore.Blazor.Components;

namespace Blazor.FlexGrid.Components.Renderers
{
    public class GridPaginationRenderer : GridPartRenderer
    {
        public override void Render(GridRendererContext rendererContext)
        {
            if (!rendererContext.TableDataSet.HasItems())
            {
                return;
            }

            if (!rendererContext.TableDataSet.PageableOptions.IsFirstPage)
            {
                rendererContext.RenderTreeBuilder.OpenElement(++rendererContext.Sequence, HtmlTagNames.Button);
                rendererContext.RenderTreeBuilder.AddAttribute(++rendererContext.Sequence, HtmlJSEvents.OnClick,
                    BindMethods.GetEventHandlerValue((UIMouseEventArgs async) => rendererContext.TableDataSet.GoToPreviousPage()));
                rendererContext.RenderTreeBuilder.AddContent(++rendererContext.Sequence, "Previous");
                rendererContext.RenderTreeBuilder.CloseElement();
            }

            RenderPagesNumbers(rendererContext);

            if (!rendererContext.TableDataSet.PageableOptions.IsLastPage)
            {
                rendererContext.RenderTreeBuilder.OpenElement(++rendererContext.Sequence, HtmlTagNames.Button);
                rendererContext.RenderTreeBuilder.AddAttribute(++rendererContext.Sequence, HtmlJSEvents.OnClick,
                    BindMethods.GetEventHandlerValue((UIMouseEventArgs async) => rendererContext.TableDataSet.GoToNextPage()));
                rendererContext.RenderTreeBuilder.AddContent(++rendererContext.Sequence, "Next");
                rendererContext.RenderTreeBuilder.CloseElement();
            }
        }

        private static void RenderPagesNumbers(GridRendererContext rendererContext)
        {
            rendererContext.RenderTreeBuilder.OpenElement(++rendererContext.Sequence, HtmlTagNames.Paragraph);
            rendererContext.RenderTreeBuilder.AddContent(++rendererContext.Sequence,
                $"{rendererContext.TableDataSet.PageableOptions.CurrentPage + 1} / {rendererContext.TableDataSet.PageableOptions.PagesCount}");
            rendererContext.RenderTreeBuilder.CloseElement();
        }
    }
}
