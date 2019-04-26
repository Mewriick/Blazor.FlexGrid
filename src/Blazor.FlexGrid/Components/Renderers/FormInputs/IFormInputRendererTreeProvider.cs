namespace Blazor.FlexGrid.Components.Renderers.FormInputs
{
    public interface IFormInputRendererTreeProvider
    {
        IFormInputRendererBuilder GetFormInputRendererTreeBuilder(FormField formField);
    }
}
