using Blazor.FlexGrid.DataSet;
using Blazor.FlexGrid.DataSet.Options;
using System;

namespace Blazor.FlexGrid.DataAdapters
{
    public abstract class BaseTableDataAdapter : ITableDataAdapter
    {
        public void Accept(IDataTableAdapterVisitor dataTableAdapterVisitor)
            => dataTableAdapterVisitor?.Visit(this);

        public abstract ITableDataSet GetTableDataSet(Action<TableDataSetOptions> configureDataSet);
    }
}
