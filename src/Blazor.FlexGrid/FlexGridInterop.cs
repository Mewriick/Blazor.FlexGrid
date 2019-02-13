using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace Blazor.FlexGrid
{
    public class FlexGridInterop
    {
        public static Task<bool> ShowModal(string modalName)
        {
            return JSRuntime.Current.InvokeAsync<bool>("flexGrid.showModal", modalName);
        }

        public static Task<bool> HideModal(string modalName)
        {
            return JSRuntime.Current.InvokeAsync<bool>("flexGrid.hideModal", modalName);
        }
    }
}
