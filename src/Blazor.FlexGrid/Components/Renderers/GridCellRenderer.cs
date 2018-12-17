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
            // TODO Draft of inline editing will be refactored
            rendererContext.OpenElement(HtmlTagNames.TableColumn, rendererContext.CssClasses.TableCell);
            if (rendererContext.TableDataSet.RowEditOptions.ItemInEditMode == rendererContext.ActualItem)
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
