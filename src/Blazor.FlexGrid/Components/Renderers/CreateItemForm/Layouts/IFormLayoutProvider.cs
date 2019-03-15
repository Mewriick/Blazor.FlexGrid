namespace Blazor.FlexGrid.Components.Renderers.CreateItemForm.Layouts
{
    public interface IFormLayoutProvider<TModel> where TModel : class
    {
        IFormLayout<TModel> GetLayoutBuilder();
    }
}
