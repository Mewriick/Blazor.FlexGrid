using Blazor.FlexGrid.DataAdapters;

namespace Blazor.FlexGrid.Features
{
    public interface IMasterTableFeature : IFeature
    {
        ITableDataAdapter TableDataAdapter { get; }
    }
}
