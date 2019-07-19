using Blazor.FlexGrid.Components.Events;
using Blazor.FlexGrid.DataSet;
using Blazor.FlexGrid.Permission;
using Microsoft.AspNetCore.Components;

namespace Blazor.FlexGrid.Components.Renderers
{
    public class GridRowRenderer : GridCompositeRenderer
    {
        public override bool CanRender(GridRendererContext rendererContext)
            => !(rendererContext.ActualItem is GroupItem
            || rendererContext.GetType().BaseType == typeof(GroupItem));

        protected override void BuildRenderTreeInternal(GridRendererContext rendererContext, PermissionContext permissionContext)
        {
            rendererContext.OpenElement(HtmlTagNames.TableRow, rendererContext.CssClasses.TableRow);

            if (!rendererContext.IsActualItemEdited)
            {
                var localActualItem = rendererContext.ActualItem;

                rendererContext.AddOnClickEvent(() =>
                    BindMethods.GetEventHandlerValue((UIMouseEventArgs e) =>
                    {
                        rendererContext.TableDataSet
                         .GridViewEvents
                         .OnItemClicked?.Invoke(new ItemClickedArgs { Item = localActualItem });
                    }));
            }

            foreach (var property in rendererContext.GridItemProperties)
            {
                rendererContext.ActualColumnName = property.Name;
                rendererContext.ActualColumnPropertyCanBeEdited = property.CanWrite;

                gridPartRenderers.ForEach(renderer => renderer.BuildRendererTree(rendererContext, permissionContext));
            }

            rendererContext.CloseElement();

            gridPartRenderersAfter.ForEach(renderer => renderer.BuildRendererTree(rendererContext, permissionContext));
        }
    }
}
