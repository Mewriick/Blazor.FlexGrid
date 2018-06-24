using Microsoft.AspNetCore.Blazor.Browser.Interop;

namespace Blazor.FlexGrid
{
    public class FlexGridJsInterop
    {
        public static string Prompt(string message)
        {
            return RegisteredFunction.Invoke<string>(
                "Blazor.FlexGrid.Prompt",
                message);
        }
    }
}
