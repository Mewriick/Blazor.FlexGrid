using Blazor.FlexGrid.DataSet;
using Blazor.FlexGrid.DataSet.Options;
using System;

namespace Blazor.FlexGrid.DataAdapters
{
    /// <summary>
    /// Define contract which create and configure <seealso cref="ITableDataSet"/> for GridComponent 
    /// </summary>
    public interface ITableDataAdapter : ICloneable
    {
        Type UnderlyingTypeOfItem { get; }

        ITableDataSet GetTableDataSet(Action<TableDataSetOptions> configureDataSet);

        void Accept(IDataTableAdapterVisitor dataTableAdapterVisitor);
    }
}
