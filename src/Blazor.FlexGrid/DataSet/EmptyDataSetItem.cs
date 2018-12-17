namespace Blazor.FlexGrid.DataSet
{
    public class EmptyDataSetItem
    {
        public static EmptyDataSetItem Instance = new EmptyDataSetItem();

        public string EmptyProperty { get; } = "Empty";
    }
}
