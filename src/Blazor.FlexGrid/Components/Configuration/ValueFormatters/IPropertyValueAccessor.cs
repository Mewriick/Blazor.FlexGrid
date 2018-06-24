namespace Blazor.FlexGrid.Components.Configuration.ValueFormatters
{
    public interface IPropertyValueAccessor
    {
        object GetValue(object @object, string name);
    }
}
