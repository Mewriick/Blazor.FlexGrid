using Blazor.FlexGrid.DataAdapters;
using System;

namespace Blazor.FlexGrid.DataSet.Options
{
    public class MasterDetailRowArguments : IMasterDetailRowArguments
    {
        public ITableDataAdapter DataAdapter { get; }

        public object SelectedItem { get; }

        public MasterDetailRowArguments(ITableDataAdapter dataAdapter, object selectedItem)
        {
            DataAdapter = dataAdapter ?? throw new ArgumentNullException(nameof(dataAdapter));
            SelectedItem = selectedItem ?? throw new ArgumentNullException(nameof(selectedItem));
        }
    }
}
