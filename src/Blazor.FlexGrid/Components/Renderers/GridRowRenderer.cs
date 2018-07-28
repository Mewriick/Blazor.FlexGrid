using Blazor.FlexGrid.DataAdapters;
using Blazor.FlexGrid.DataSet;
using Blazor.FlexGrid.DataSet.Options;
using Microsoft.AspNetCore.Blazor;
using Microsoft.AspNetCore.Blazor.Components;

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

                if (rendererContext.TableDataSet is IMasterTableDataSet masterTableDataSet)
                {
                    rendererContext.OpenElement("ul");

                    foreach (var dataAdapter in masterTableDataSet.DetailDataAdapters)
                    {
                        var actualItem = rendererContext.ActualItem;
                        rendererContext.OpenElement("li");
                        rendererContext.AddOnClickEvent(() => BindMethods.GetEventHandlerValue((UIMouseEventArgs async) =>
                            masterTableDataSet.SelectDataAdapter(new MasterDetailRowArguments(dataAdapter, actualItem))
                        ));
                        rendererContext.AddContent(dataAdapter.GetUnderlyingType().Name);
                        rendererContext.CloseElement();
                    }

                    rendererContext.CloseElement();

                    rendererContext.OpenElement("div");
                    rendererContext.AddGridViewComponent(masterTableDataSet.GetSelectedDataAdapter(rendererContext.ActualItem));
                    rendererContext.CloseElement();
                }

                rendererContext.CloseElement();
                rendererContext.CloseElement();
            }
        }
    }
}
