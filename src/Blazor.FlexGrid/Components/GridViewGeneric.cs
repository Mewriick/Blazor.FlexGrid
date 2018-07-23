using Microsoft.AspNetCore.Blazor.Components;

namespace Blazor.FlexGrid.Components
{
    /// <summary>
    /// Workaround GridViewComponent for rendering Master/Detail grids in tabs 
    /// </summary>
    /// <typeparam name="T"></typeparam>

    [Route("/gridviewgeneric")]
    internal class GridViewGeneric<T> : GridViewInternal
    {
    }
}
