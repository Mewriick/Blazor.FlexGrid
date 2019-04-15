using Blazor.FlexGrid.DataAdapters;
using System;

namespace Blazor.FlexGrid.Features
{
    public class MasterTableFeature : IMasterTableFeature
    {
        public string Name => nameof(MasterTableFeature);

        public ITableDataAdapter TableDataAdapter { get; }

        public MasterTableFeature(ITableDataAdapter tableDataAdapter)
        {
            TableDataAdapter = tableDataAdapter ?? throw new ArgumentException(nameof(tableDataAdapter));
        }
    }
}
