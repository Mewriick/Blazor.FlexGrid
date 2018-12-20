namespace Blazor.FlexGrid.Components.Renderers.EditInputs
{
    public class EditInputRendererTree : AbstractEditInputRenderer
    {
        private AbstractEditInputRenderer rendererTree;

        public EditInputRendererTree()
        {
            var dateTimeInputRenderer = new DateTimeInputRenderer();
            var textInputRenderer = new TextInputRenderer();
            var numberInputRenderer = new NumberInputType();
            var selectInputRenderer = new SelectInputRenderer();

            numberInputRenderer.SetSuccessor(dateTimeInputRenderer);
            dateTimeInputRenderer.SetSuccessor(selectInputRenderer);
            selectInputRenderer.SetSuccessor(textInputRenderer);

            rendererTree = numberInputRenderer;
        }

        public override void RenderInput(GridRendererContext gridRendererContext)
            => rendererTree.RenderInput(gridRendererContext);
    }
}
