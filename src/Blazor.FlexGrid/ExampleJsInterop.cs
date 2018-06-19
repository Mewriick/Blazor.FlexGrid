using System;
using Microsoft.AspNetCore.Blazor.Browser.Interop;

namespace Blazor.FlexGrid
{
    public class ExampleJsInterop
    {
        public static string Prompt(string message)
        {
            return RegisteredFunction.Invoke<string>(
                "Blazor.FlexGrid.ExampleJsInterop.Prompt",
                message);
        }
    }
}
