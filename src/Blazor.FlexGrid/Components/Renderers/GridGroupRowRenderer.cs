using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Blazor.FlexGrid.Permission;

namespace Blazor.FlexGrid.Components.Renderers
{
    public class GridGroupRowRenderer : GridCompositeRenderer
    {
        public override bool CanRender(GridRendererContext rendererContext)
        {
            return rendererContext.ActualItem.GetType().GetInterfaces()
                .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IGrouping<,>));
        }

        protected override void BuildRenderTreeInternal(GridRendererContext rendererContext, PermissionContext permissionContext)
        {

            rendererContext.OpenElement(HtmlTagNames.TableRow, rendererContext.CssClasses.TableGroupRow);
            rendererContext.OpenElement(HtmlTagNames.TableColumn, rendererContext.CssClasses.TableGroupRowCell);

            int numberOfColumns = rendererContext.GridItemProperties.Count;
            if (rendererContext.GridConfiguration.InlineEditOptions.InlineEditIsAllowed)
                numberOfColumns++;
            if (rendererContext.GridConfiguration.IsMasterTable)
                numberOfColumns++;
            rendererContext.AddAttribute(HtmlAttributes.Colspan, numberOfColumns);

            var keyProperty = rendererContext.ActualItem.GetType().GetProperty("Key");
            var keyValue = keyProperty != null ? keyProperty.GetValue(rendererContext.ActualItem) : null;
            var key = keyValue != null && !string.IsNullOrEmpty(keyValue.ToString()) ? keyValue.ToString() : "(null)";
            rendererContext.AddContent(key);

            rendererContext.CloseElement();
            rendererContext.CloseElement();

            var subItems = (IEnumerable)rendererContext.ActualItem;
            foreach (var item in subItems)
            {
                rendererContext.ActualItem = item;
                gridPartRenderers.ForEach(renderer => renderer.BuildRendererTree(rendererContext, permissionContext));

            }


        }
    }
}
