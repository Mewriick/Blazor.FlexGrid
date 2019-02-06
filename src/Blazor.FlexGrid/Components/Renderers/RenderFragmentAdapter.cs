using Microsoft.AspNetCore.Components;
using System;

namespace Blazor.FlexGrid.Components.Renderers
{
    public abstract class RenderFragmentAdapter
    {
        public abstract RenderFragment GetColumnFragment(object item);
    }

    public class RenderFragmentAdapter<TItem> : RenderFragmentAdapter
    {
        private readonly RenderFragment<TItem> renderFragment;

        public RenderFragmentAdapter(RenderFragment<TItem> renderFragment)
        {
            this.renderFragment = renderFragment ?? throw new ArgumentNullException(nameof(renderFragment));
        }

        public override RenderFragment GetColumnFragment(object item)
            => renderFragment((TItem)item);
    }
}
