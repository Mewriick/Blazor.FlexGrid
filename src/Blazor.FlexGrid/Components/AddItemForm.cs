using Blazor.FlexGrid.Components.Renderers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.RenderTree;

namespace Blazor.FlexGrid.Components
{
    public class AddItemForm : ComponentBase
    {
        [Inject]
        private AddItemFormRenderer AddItemFormRenderer { get; set; }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);
        }
    }
}
