using Blazor.FlexGrid.DataSet;
using Blazor.FlexGrid.DataSet.Options;
using System;

namespace Blazor.FlexGrid.DataAdapters
{
    public abstract class BaseTableDataAdapter : ITableDataAdapter
    {
        public abstract Type UnderlyingTypeOfItem { get; }

        public void Accept(IDataTableAdapterVisitor dataTableAdapterVisitor)
            => dataTableAdapterVisitor?.Visit(this);

        public abstract ITableDataSet GetTableDataSet(Action<TableDataSetOptions> configureDataSet);

        public abstract object Clone();
    }
}
