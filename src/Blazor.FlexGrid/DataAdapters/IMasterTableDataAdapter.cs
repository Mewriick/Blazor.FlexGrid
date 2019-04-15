using System.Collections.Generic;

namespace Blazor.FlexGrid.DataAdapters
{
    public interface IMasterTableDataAdapter
    {
        IReadOnlyCollection<ITableDataAdapter> DetailTableDataAdapters { get; }

        void AddDetailTableSet(ITableDataAdapter tableDataAdapter);
    }
}
