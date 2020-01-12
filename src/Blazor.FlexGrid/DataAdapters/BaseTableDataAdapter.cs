using Blazor.FlexGrid.DataSet;
using Blazor.FlexGrid.DataSet.Options;
using System;
using System.Threading.Tasks;

namespace Blazor.FlexGrid.DataAdapters
{
    public abstract class BaseTableDataAdapter : ITableDataAdapter
    {
        protected ITableDataSet currentTableDataSet;

        public abstract Type UnderlyingTypeOfItem { get; }

        public Action AfterReloadPage { get; set; }

        public void Accept(IDataTableAdapterVisitor dataTableAdapterVisitor)
            => dataTableAdapterVisitor?.Visit(this);

        public async Task ReloadCurrentPage()
        {
            if (currentTableDataSet is null)
            {
                return;
            }

            await currentTableDataSet.GoToPage(currentTableDataSet.PageableOptions.CurrentPage);
            AfterReloadPage?.Invoke();
        }

        public abstract ITableDataSet GetTableDataSet(Action<TableDataSetOptions> configureDataSet);

        public abstract object Clone();
    }
}
