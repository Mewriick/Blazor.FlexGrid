using Blazor.FlexGrid.DataSet;
using System.Linq;

namespace Blazor.FlexGrid.Components.Renderers
{
    public class GridRowRenderer : GridCompositeRenderer
    {
        public override void Render(GridRendererContext rendererContext)
        {
            rendererContext.OpenElement(HtmlTagNames.TableRow, rendererContext.CssClasses.TableRow);

            foreach (var property in rendererContext.GridItemProperties)
            {
                rendererContext.ActualColumnName = property.Name;
                gridPartRenderers.ForEach(renderer => renderer.Render(rendererContext));
            }

            rendererContext.CloseElement();

            // Temporary this is only for tesing
            if (rendererContext.TableDataSet.ItemIsSelected(rendererContext.ActualItem))
            {
                rendererContext.OpenElement(HtmlTagNames.TableRow, rendererContext.CssClasses.TableRow);
                rendererContext.OpenElement(HtmlTagNames.TableColumn, rendererContext.CssClasses.TableCell);
                rendererContext.AddColspan();

                rendererContext.AddGridViewComponent(
                        (rendererContext.TableDataSet as IMasterTableDataSet)?.DetailDataAdapters.First());

                rendererContext.CloseElement();
                rendererContext.CloseElement();
            }
        }
    }
}
