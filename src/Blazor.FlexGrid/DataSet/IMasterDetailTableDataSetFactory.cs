namespace Blazor.FlexGrid.DataSet
{
    public interface IMasterDetailTableDataSetFactory
    {
        IMasterTableDataSet ConvertToMasterTableIfIsRequired(ITableDataSet tableDataSet);
    }
}
