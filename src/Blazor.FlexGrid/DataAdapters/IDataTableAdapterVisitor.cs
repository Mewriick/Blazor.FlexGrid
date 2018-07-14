namespace Blazor.FlexGrid.DataAdapters
{
    public interface IDataTableAdapterVisitor
    {
        void Visit(ITableDataAdapter tableDataAdapter);
    }
}
