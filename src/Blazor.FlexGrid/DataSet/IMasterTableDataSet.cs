using Blazor.FlexGrid.DataAdapters;
using System.Collections.Generic;

namespace Blazor.FlexGrid.DataSet
{
    /// <summary>
    /// Represents a collection of items, which can have associated related <seealso cref="ITableDataAdapter"/>
    /// </summary>
    public interface IMasterTableDataSet : ITableDataSet
    {
        IEnumerable<ITableDataAdapter> DetailDataAdapters { get; }
    }
}
