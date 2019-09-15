using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace Blazor.FlexGrid
{
    public class FlexGridInterop
    {
        private readonly IJSRuntime jSRuntime;

        public FlexGridInterop(IJSRuntime jSRuntime)
        {
            this.jSRuntime = jSRuntime ?? throw new ArgumentNullException(nameof(jSRuntime));
        }

        public ValueTask<bool> ShowModal(string modalName)
        {
            return jSRuntime.InvokeAsync<bool>("flexGrid.showModal", modalName);
        }

        public ValueTask<bool> HideModal(string modalName)
        {
            return jSRuntime.InvokeAsync<bool>("flexGrid.hideModal", modalName);
        }

        public ValueTask<bool> AppendCssClass(string elementName, string cssClass)
        {
            return jSRuntime.InvokeAsync<bool>("flexGrid.appendCssClass", elementName, cssClass);
        }


    }
}
