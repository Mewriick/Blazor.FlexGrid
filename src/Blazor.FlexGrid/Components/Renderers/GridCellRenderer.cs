using Blazor.FlexGrid.Components.Renderers.EditInputs;
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

        protected override void RenderInternal(GridRendererContext rendererContext)
        {
            rendererContext.OpenElement(HtmlTagNames.TableColumn, rendererContext.CssClasses.TableCell);

            if (!rendererContext.IsActualItemEdited)
            {
                rendererContext.AddActualColumnValue();
                rendererContext.CloseElement();

                return;
            }

            if (rendererContext.ActualColumnPropertyIsEditable && rendererContext.HasCurrentUserWritePermission(rendererContext.ActualColumnName))
            {
                editInputRendererTree.RenderInput(rendererContext);
            }
            else
            {
                rendererContext.AddActualColumnValue();
            }

            rendererContext.CloseElement();
        }
    }
}
