using Blazor.FlexGrid.DataSet;
using Blazor.FlexGrid.DataSet.Options;
using System;

namespace Blazor.FlexGrid.DataAdapters
{
    public interface ITableDataAdapter
    {
        ITableDataSet GetTableDataSet(Action<TableDataSetOptions> configureDataSet);
    }
}
