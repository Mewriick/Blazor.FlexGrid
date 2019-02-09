using Blazor.FlexGrid.Components.Renderers.EditInputs;
using Blazor.FlexGrid.Permission;
using System;

namespace Blazor.FlexGrid.Components.Renderers
{
    public class GridCellRenderer : GridPartRenderer
    {
        private readonly EditInputRendererTree editInputRendererTree;

        public GridCellRenderer(EditInputRendererTree editInputRendererTree)
        {
            this.editInputRendererTree = editInputRendererTree ?? throw new ArgumentNullException(nameof(editInputRendererTree));
        }

        public override bool CanRender(GridRendererContext rendererContext)
            => true;

        protected override void RenderInternal(GridRendererContext rendererContext, PermissionContext permissionContext)
        {
            rendererContext.OpenElement(HtmlTagNames.TableColumn, rendererContext.CssClasses.TableCell);

            if (!rendererContext.IsActualItemEdited)
            {
                rendererContext.AddActualColumnValue(permissionContext);
                rendererContext.CloseElement();

                return;
            }

            if (rendererContext.ActualColumnPropertyCanBeEdited && permissionContext.HasCurrentUserWritePermission(rendererContext.ActualColumnName))
            {
                editInputRendererTree.RenderInput(
                    rendererContext.RendererTreeBuilder,
                    rendererContext,
                    rendererContext.TableDataSet.EditItemProperty);
            }
            else
            {
                rendererContext.AddActualColumnValue(permissionContext);
            }

            rendererContext.CloseElement();
        }
    }
}
