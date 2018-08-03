namespace Blazor.FlexGrid.DataSet
{
    public interface IMasterDetailTableDataSetFactory
    {
        ITableDataSet ConvertToMasterTableIfIsRequired(ITableDataSet tableDataSet);
    }
}
