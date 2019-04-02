using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Blazor.FlexGrid.DataSet;
using Blazor.FlexGrid.Permission;
using Microsoft.AspNetCore.Components;

namespace Blazor.FlexGrid.Components.Renderers
{
    public class GridGroupRowRenderer : GridCompositeRenderer
    {
        
        public override bool CanRender(GridRendererContext rendererContext)
        {
            return rendererContext.ActualItem is GroupItem 
                || rendererContext.GetType().BaseType == typeof(GroupItem);
        }

        protected override void BuildRenderTreeInternal(GridRendererContext rendererContext, PermissionContext permissionContext)
        {
            var actualGroupItem = (GroupItem)rendererContext.ActualItem;
            bool isCollapsed = actualGroupItem.IsCollapsed;
            rendererContext.OpenElement(HtmlTagNames.TableBody, isCollapsed? "table-group-row-wrapper-collapsed" : "table-group-row-wrapper");
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

                rendererContext.AddOnClickEvent(() =>
                    BindMethods.GetEventHandlerValue((UIMouseEventArgs e) =>
                    {
                        rendererContext.TableDataSet.ToggleGroupRow(actualGroupItem.Key);
                        rendererContext.RequestRerenderNotification?.Invoke();
                    })
                );

                rendererContext.OpenElement(HtmlTagNames.I, !isCollapsed ? "fas fa-angle-down" : "fas fa-angle-right");
                rendererContext.CloseElement();

                var keyProperty = rendererContext.ActualItem.GetType().GetProperty("Key");
                var keyValue = keyProperty != null ? keyProperty.GetValue(rendererContext.ActualItem) : null;
                var key = keyValue != null ? keyValue.ToString() : "(null)";
                if (string.IsNullOrEmpty(key)) key = @"""";
                rendererContext.AddContent($"       {key}   ");

                rendererContext.OpenElement(HtmlTagNames.I);
                rendererContext.AddContent($"({actualGroupItem.Count})");
                rendererContext.CloseElement();

                rendererContext.CloseElement();
                rendererContext.CloseElement();

                var subItems = (IEnumerable)(GroupItem)rendererContext.ActualItem;
                foreach (var item in subItems)
                {
                    rendererContext.ActualItem = item;
                    gridPartRenderers.ForEach(renderer => renderer.BuildRendererTree(rendererContext, permissionContext));

                }
            }
            catch(Exception ex)
            {
                throw ex;
            }

            rendererContext.CloseElement();

        }
    }
}
