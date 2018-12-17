using Blazor.FlexGrid.DataSet.Options;

namespace Blazor.FlexGrid.DataSet
{
    public interface IRowEditableDataSet : IBaseTableDataSet
    {
        IRowEditOptions RowEditOptions { get; }

        void EditItem(object item);

        bool SaveItem();
    }
}
