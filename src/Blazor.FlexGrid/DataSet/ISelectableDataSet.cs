namespace Blazor.FlexGrid.DataSet
{
    public interface ISelectableDataSet : IBaseTableDataSet
    {
        void ToggleRowItem(object item);

        bool ItemIsSelected(object item);
    }
}
