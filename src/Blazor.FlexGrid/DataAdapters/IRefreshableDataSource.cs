using System;
using System.Threading.Tasks;

namespace Blazor.FlexGrid.DataAdapters
{
    public interface IRefreshableDataSource
    {
        Action AfterReloadPage { get; set; }

        Task ReloadCurrentPage();
    }
}
