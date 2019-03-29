using Blazor.FlexGrid.Components.Renderers.EditInputs;
using Blazor.FlexGrid.DataSet;
using Blazor.FlexGrid.Permission;
using Microsoft.AspNetCore.Components;
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

        protected override void BuildRendererTreeInternal(GridRendererContext rendererContext, PermissionContext permissionContext)
        {
            rendererContext.OpenElement(HtmlTagNames.TableColumn, rendererContext.CssClasses.TableCell);

            //Action onRowClickAction = null;
            //if (rendererContext.TableDataSet.GetType().GetGenericTypeDefinition() == typeof(TableDataSet<>))
            //{

            //    var onRowClicked = rendererContext.TableDataSet.OnRowClicked;

            //    onRowClickAction = onRowClicked?.Invoke(rendererContext);
            //    rendererContext.AddOnClickEvent(
            //           () => BindMethods.GetEventHandlerValue((UIMouseEventArgs e) =>
            //           {
            //               onRowClickAction();
            //           })
            //    );

            //}

            if (!rendererContext.IsActualItemEdited)
            {
                rendererContext.AddActualColumnValue(permissionContext);
                rendererContext.CloseElement();

                return;
            }

            if (rendererContext.ActualColumnPropertyCanBeEdited && permissionContext.HasCurrentUserWritePermission(rendererContext.ActualColumnName))
            {
                editInputRendererTree.BuildInputRendererTree(
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
