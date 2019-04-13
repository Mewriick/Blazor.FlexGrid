using Blazor.FlexGrid.DataAdapters;
using Blazor.FlexGrid.DataSet;
using Blazor.FlexGrid.Permission;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace Blazor.FlexGrid.Components.Renderers
{
    public class GridBodyGroupedRenderer : GridCompositeRenderer
    {
        private readonly ILogger<GridBodyGroupedRenderer> logger;

        public GridBodyGroupedRenderer(ILogger<GridBodyGroupedRenderer> logger)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public override bool CanRender(GridRendererContext rendererContext)
            => rendererContext.TableDataSet.GroupedItems != null;

        protected override void BuildRenderTreeInternal(GridRendererContext rendererContext, PermissionContext permissionContext)
        {
            using (new MeasurableScope(sw => logger.LogInformation($"Grid grouped body rendering duration {sw.ElapsedMilliseconds}ms")))
            {
                rendererContext.OpenElement(HtmlTagNames.TableBody, rendererContext.CssClasses.TableBody);
                foreach (var group in rendererContext.TableDataSet.GroupedItems)
                {
                    try
                    {
                        rendererContext.OpenElement(HtmlTagNames.TableRow, rendererContext.CssClasses.TableGroupRow);
                        rendererContext.OpenElement(HtmlTagNames.TableColumn, rendererContext.CssClasses.TableGroupRowCell);

                        int numberOfColumns = rendererContext.GridItemProperties.Count;
                        if (rendererContext.GridConfiguration.InlineEditOptions.InlineEditIsAllowed)
                            numberOfColumns++;
                        if (rendererContext.GridConfiguration.IsMasterTable)
                            numberOfColumns++;
                        rendererContext.AddAttribute(HtmlAttributes.Colspan, numberOfColumns);
                        rendererContext.OpenElement(HtmlTagNames.I, group.IsCollapsed ? "fas fa-plus" : "fas fa-minus");
                        rendererContext.AddOnClickEvent(() =>
                            BindMethods.GetEventHandlerValue((UIMouseEventArgs e) =>
                            {
                                rendererContext.TableDataSet.ToggleGroupRow(group.Key);
                                rendererContext.RequestRerenderNotification?.Invoke();
                            })
                        );
                        rendererContext.CloseElement();
                        rendererContext.AddMarkupContent($"\t<b>{rendererContext.TableDataSet.GroupingOptions.GroupedProperty.Name}:</b> {group.Key.ToString()}\t");
                        rendererContext.OpenElement(HtmlTagNames.I);
                        rendererContext.AddContent($"({group.Count})");
                        rendererContext.CloseElement();


                        if (!group.IsCollapsed)
                        {
                            var subItemsListType = typeof(List<>).MakeGenericType(rendererContext.TableDataSet.UnderlyingTypeOfItem());
                            var subItemsList = Activator.CreateInstance(subItemsListType, new object[] { group });
                            var dataAdapterType = typeof(CollectionTableDataAdapter<>).MakeGenericType(rendererContext.TableDataSet.UnderlyingTypeOfItem());
                            var dataAdapter = Activator.CreateInstance(dataAdapterType, new object[] { subItemsList }) as ITableDataAdapter;

                            rendererContext.AddGridViewComponent(dataAdapter);
                        }

                        rendererContext.CloseElement();
                        rendererContext.CloseElement();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }

                rendererContext.CloseElement();
            }
        }
    }
}
