using Blazor.FlexGrid.DataAdapters;

namespace Blazor.FlexGrid.DataSet.Options
{
    public interface IMasterDetailRowArguments
    {
        ITableDataAdapter DataAdapter { get; }

        object SelectedItem { get; }
    }
}
